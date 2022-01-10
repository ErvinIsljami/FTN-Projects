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

	/* DATA LINK LAYER - Ethernet */

	// Retrive the position of the ethernet header
	ethernet_header * eh = (ethernet_header *)packet_data;
	
	/* NETWORK LAYER - IPv4 */

	// Retrieve the position of the ip header
	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
	
	// TRANSPORT LAYER - UDP
	if(ih->next_protocol != 17)
		return;

	// Retrieve the position of udp header
	udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);


	/* IP CHECKSUM VERIFICATION */
	printf("Verifying IP checksum...\n");
	unsigned short ip_checksum = calculate_checksum((unsigned short*)ih, ih->header_length*4/2);
	
	if(ip_checksum == 0)
		printf("IP checksum -> OK (0x%.2x)\n", ip_checksum);
	else
		printf("IP checksum -> BAD (0x%.2x)\n", ip_checksum);


	/* UDP CHECKSUM CALCULATION */

	// Calculate size of (IP pseudo header + UDP datagram)
	int data_length = 12 + ntohs(uh->datagram_length);		// Length of pseudo header + length of datagram
	data_length = data_length + data_length % 2;			// Check sum have to be even number

	// Dynamic allocate memory of checksum data
	unsigned char* checksum_data = (unsigned char*)malloc(data_length);
	memset(checksum_data, 0, data_length);

	// Copy IP pseudo header
	memcpy(checksum_data, ih->src_addr, 4);					// IP:	Source Address		(4 bytes)
	memcpy(checksum_data + 4, ih->dst_addr, 4);				// IP:  Destination Address	(4 bytes)
															// IP:  Reserved - all 0's	(1 byte)
	checksum_data[9] = ih->next_protocol;					// IP:  Next Protocol		(1 byte)
	memcpy(checksum_data + 10, &(uh->datagram_length), 2);	// UDP:	Datagram Length		(2 bytes)	
															//	--------------------- = 12 bytes
	// Copy UDP datagram (header + application data)
	memcpy(checksum_data + 12, uh, ntohs(uh->datagram_length));

	// Initialize UDP checksum
	memset(checksum_data + 18, 0, 2);	// 18 = 12 (pseudo header) + 6 (position in udp header)
	
	printf("Calculating UDP checksum...\n");
	unsigned short udp_checksum = calculate_checksum((unsigned short*)checksum_data, data_length/2);
	printf("UDP checksum: %.2x\n\n", udp_checksum);

	// Free dynamic alocated memory
	free(checksum_data);
}

// Calculates checksum for given data
unsigned short calculate_checksum(unsigned short * data, int data_length)
{
	unsigned int sum = 0;

	// Calculate sum of 16bits values
	for(int i = 0; i < data_length; i++)
	{
		sum += ntohs(*(data + i));
	}

	unsigned int carry;

	// Carry will be added to the rest of the value
	while (carry = sum >> 16)
	{
		sum = sum & 0x0000FFFF;
		sum = sum + carry;
	}

	// Flip every bit in that value, to obtain the checksum
	unsigned short checksum = ~(unsigned short)sum; 

	return checksum;
}
