// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 12 - Kontrolna suma

// We do not want the warnings about the old deprecated and unsecure CRT functions since these examples can be compiled under *nix as well
#ifdef _MSC_VER
	#define _CRT_SECURE_NO_WARNINGS
#endif

// Include libraries
#include <stdlib.h>
#include <stdio.h>
#include <winsock2.h>
#include <windows.h>
#include <ws2tcpip.h>
#include "conio.h"
#include "pcap.h"
#include "protocol_headers.h"

// Function declarations
void packet_handler(unsigned char *param, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);	// Callback function invoked by WinPcap for every incoming packet
unsigned short calculate_checksum(unsigned short * data, int data_length);												// Calculates checksum for given data

// Main function captures packets from the file
int main()
{
	pcap_t* device_handle;
	char error_buffer[PCAP_ERRBUF_SIZE];
	
	// Open the capture file 
	if ((device_handle = pcap_open_offline("udp_packets.pcap", // Name of the device
								error_buffer	  // Error buffer
							)) == NULL)
	{
		printf("\n Unable to open the file %s.\n", "udp_packets.pcap");
		return -1;
	}

	// Check the link layer. We support only Ethernet for simplicity.
	if(pcap_datalink(device_handle) != DLT_EN10MB)
	{
		printf("\nThis program works only on Ethernet networks.\n");
		return -1;
	}

	struct bpf_program fcode;

	// Compile the filter
	if (pcap_compile(device_handle, &fcode, "ip and udp", 1, 0xffffff) < 0)
	{
		 printf("\n Unable to compile the packet filter. Check the syntax.\n");
		 return -1;
	}

	// Set the filter
	if (pcap_setfilter(device_handle, &fcode) < 0)
	{
		printf("\n Error setting the filter.\n");
		return -1;
	}

	unsigned char packet = 0;

	// Read and dispatch packets until EOF is reached
	pcap_loop(device_handle, 0, packet_handler, &packet);

	// Close the file associated with device_handle and deallocates resources
	pcap_close(device_handle);

	return 0;
}

// Callback function invoked by WinPcap for every incoming packet
void packet_handler(unsigned char* param, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	printf("Packet #%d\n", (*param)++);
	ethernet_header * eh = (ethernet_header *)packet_data;
	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
	if(ih->next_protocol != 17)
		return;

	tcp_header* th = (tcp_header*) ((unsigned char*)ih + ih->header_length * 4);
	int app_len = 10;
	int duzina = 12 + ntohs(th->header_length * 4) + app_len;
	if (duzina % 2 == 1)
		duzina++;

	unsigned char* tcp_checksum = (unsigned char*)malloc(duzina);
	memset(tcp_checksum, 0, duzina);

	memcpy(tcp_checksum, ih->src_addr, 4);
	memcpy(tcp_checksum + 4, ih->dst_addr, 4);
	tcp_checksum[8] = 0;
	tcp_checksum[9] = ih->next_protocol;

	memcpy(tcp_checksum + 10, &(th->windows_size), 2);
	memcpy(tcp_checksum + 12, th, ntohs(th->header_length * 4));	//kopira sve podatke iz tcp-a i app data
	//memcpy(tcp_checksum + th->header_length * 4 + 12, app_data, app_len);

	//racunanje checksume
	memset(tcp_checksum + 28, 0, 2);	//postavlja polje checksum na nulu posto treba sad da je izracuna
	unsigned short udp_chcks = calculate_checksum((unsigned short*)tcp_checksum, duzina / 2);
	printf("udp check suma je %x \n\n\n", udp_chcks);

	udp_chcks = htons(udp_chcks);
	memcpy(tcp_checksum + 28, &udp_chcks, 2);

	//verifikovanje udp checksume
	udp_chcks = calculate_checksum((unsigned short*)tcp_checksum, duzina / 2);

	if (udp_chcks == 0)
		printf("TCP Check suma ok\n");
	else
		printf("TCP Check suma nije ok\n");

	



	return;
}


unsigned short calculate_checksum(unsigned short* data, int data_length)
{
	unsigned int sum = 0;

	for (int i = 0; i < data_length; i++)
	{
		sum += ntohs(*(data + i));
	}
	unsigned int carry;

	while (carry = sum >> 16)
	{
		sum = sum & 0x0000FFFF;
		sum = sum + carry;
	}

	unsigned short checksum = ~(unsigned short)sum; //komplement1

	return checksum;
}