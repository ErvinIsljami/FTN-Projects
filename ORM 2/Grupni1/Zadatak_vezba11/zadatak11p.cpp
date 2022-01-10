// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 11

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

typedef struct text_message_st
{
	unsigned short id;
	char length;
	char message[7];
}text_message;

typedef struct status_message_st
{
	unsigned short id;
	bool value;
}status_message;

// Function declarations
void packet_handler(unsigned char *param, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);

pcap_dumper_t* output_file;	//za upis u fajl

// Main function captures packets from the file
int main()
{
	pcap_t* device_handle;
	char error_buffer[PCAP_ERRBUF_SIZE];
	
	// Open the capture file 
	if ((device_handle = pcap_open_offline("input_packets.pcap", // Name of the device
								error_buffer	  // Error buffer
							)) == NULL)
	{
		printf("\n Unable to open the file %s.\n", "input_packets.pcap");
		return -1;
	}

	// Check the link layer. We support only Ethernet for simplicity.
	if(pcap_datalink(device_handle) != DLT_EN10MB)
	{
		printf("\nThis program works only on Ethernet networks.\n");
		return -1;
	}
	//za pisanje u fajl
	output_file = pcap_dump_open(device_handle, "status_message.pcap");	
	if (output_file == NULL)
	{
		printf("\n Error opening output file %s\n", "status_message.pcap");
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

	// Read and dispatch packets until EOF is reached
	pcap_loop(device_handle, 0, packet_handler, NULL);

	// Close the file associated with device_handle and deallocates resources
	pcap_close(device_handle);
	getchar();

	return 0;
}

// Callback function invoked by WinPcap for every incoming packet
void packet_handler(unsigned char* param, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	ethernet_header * eh = (ethernet_header *)packet_data;
	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
	
	if(ih->next_protocol != 17)
	{
		return;
	}
		udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);

	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);
	
	if (app_data[0] >> 4 == 1) //text
	{
		text_message* poruka = (text_message*)app_data;
		//printf("Tekst poruke: %s\n", poruka->message);
		printf("Tekst poruke: ");
		for (int i = 0; i < poruka->length; i++)
			printf("%c", poruka->message[i]);
		printf("\n");

		//sifrovanje Cezarovom sifrom
		unsigned char *sifrovanaPoruka = (unsigned char*)malloc(poruka->length + 1);
		int pomeraj = 5;

		for (int i = 0; i < poruka->length; i++)
		{
			sifrovanaPoruka[i] = poruka->message[i] - pomeraj;
			if (sifrovanaPoruka[i] < 'A')
				sifrovanaPoruka[i] += 26;
		}
		sifrovanaPoruka[poruka->length] = '\0';
		printf("Sifrovano: %s\n\n", sifrovanaPoruka);





	}
	else	//status
	{
		status_message* poruka = (status_message*)app_data;
		if(poruka->value == true)
			pcap_dump((unsigned char*)output_file, packet_header, packet_data);
	}
}