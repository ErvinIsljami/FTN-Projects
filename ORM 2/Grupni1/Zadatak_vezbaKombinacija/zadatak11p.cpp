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

// Function declarations
void packet_handler(unsigned char *param, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);


struct text_message
{
	unsigned short id;
	unsigned char length;
	unsigned char message[7];
};

struct status_message
{
	unsigned short id;
	unsigned char value;
};

pcap_dumper_t* output_file;

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


	// ovo vi pisete
	output_file = pcap_dump_open(device_handle, "status_message.pcap");

	if (output_file == NULL)
	{
		printf("\n Error opening output file %s\n", "status_message.pcap");
		return -1;
	}

	// Check the link layer. We support only Ethernet for simplicity.
	if (pcap_datalink(device_handle) != DLT_EN10MB)
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
	ip_header* ih = (ip_header*)(packet_data + sizeof(ethernet_header));
	if (ih->next_protocol != 17)
	{
		return;
	}
	udp_header* uh = (udp_header*)((unsigned char*)ih + ih->header_length * 4);
	
	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	if (app_data[0] == 0x10)    //packet with textual message  
	{
		struct text_message* tm = (struct text_message *)app_data;
		char len = tm->length;  // length of textual message
		for (int i = 0; i < len; i++)
		{
			printf("%c", tm->message[i]);   //print message
		}
		printf("\n");

		//desifrovanje cezarova sifra
		int pomeraj = 3;
		for (int i = 0; i < tm->length; i++)
		{
			tm->message[i] -= pomeraj;
			if (tm->message[i] < 'A')
				tm->message[i] += 26;
		}
		printf("Desifrovana poruka: %s \n", tm->message);
		printf("------------------------------\n");




	}

	else if (app_data[0] == 0x20)  //packet with status message   
	{
		struct status_message* sm = (struct status_message *)app_data;
		if (sm->value != 0)   // packet with status value 'true' dump in output file 
		{
			pcap_dump((unsigned char*)output_file, packet_header, packet_data);
		}
	}

}