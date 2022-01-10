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

typedef struct meas_message_st
{
	unsigned short id;
	int value;
	char unit[4];
}meas_message;

typedef struct switch_message_st
{
	unsigned short id;
	bool value;
}switch_message;

typedef struct device_entry_st
{
	unsigned short id;
	char device_name[10];
	int value;
}device_entry;

// Function declarations
void packet_handler(unsigned char *param, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);
void print_table();
pcap_dumper_t* output_file1;	//za upis u fajl
pcap_dumper_t* output_file2;	//za upis u fajl

device_entry table[10];
int table_counter = 4;

// Main function captures packets from the file
int main()
{
	table[0].id = 0x1001;
	table[0].value = 220;
	strcpy(table[0].device_name, "Voltage 1");

	table[1].id = 0x1002;
	table[1].value = 1000;
	strcpy(table[1].device_name, "Voltage 2");

	table[2].id = 0x2001;
	table[2].value = 0;
	strcpy(table[2].device_name, "Switch 1");

	table[3].id = 0x2002;
	table[3].value = 1;
	strcpy(table[3].device_name, "Switch 2");

	print_table();

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
	output_file1 = pcap_dump_open(device_handle, "masurements.pcap");	
	if (output_file1 == NULL)
	{
		printf("\n Error opening output file %s\n", "masurements.pcap");
		return -1;
	}

	output_file2 = pcap_dump_open(device_handle, "switches.pcap");
	if (output_file2 == NULL)
	{
		printf("\n Error opening output file %s\n", "switches.pcap");
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
	
	char ip_address[] = { 192, 169,1, 0 };
	char subnet_mask[] = { 255, 255, 255, 0 };

	for (int i = 0; i < 4; i++)
	{
		if ((ip_address[i] & subnet_mask[i]) != (ih->src_addr[i] & subnet_mask[i]))
			return;	//odbacujem paket
	}






	if(ih->next_protocol != 17)
	{
		return;
	}
		udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);

	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);
	
	if (app_data[0] >> 4 == 1) //meas message
	{
		meas_message * poruka = (meas_message*)app_data;
		if (strcmp(poruka->unit, "kV") == 0)
		{
			poruka->value *= 1000;
		}
		printf("Napon je: %d V\n", poruka->value);
		pcap_dump((unsigned char*)output_file1, packet_header, packet_data);

		int id_merenja = poruka->id & 0x000f;
		for (int i = 0; i < table_counter; i++)
		{
			if (id_merenja == 1)
			{
				if (strcmp(table[i].device_name, "Voltage 1") == 0)
				{
					table[i].value = poruka->value;
					break;
				}
			}
			else
			{
				if (strcmp(table[i].device_name, "Voltage 2") == 0)
				{
					table[i].value = poruka->value;
					break;
				}
			}
		}

	}
	else	//	switch message
	{
		switch_message * poruka = (switch_message*)app_data;
		if (poruka->value == true)
		{
			printf("Closed.\n");
		}
		else
		{
			printf("Open.\n");
		}
		pcap_dump((unsigned char*)output_file2, packet_header, packet_data);

		int id_merenja = poruka->id & 0x000f;
		for (int i = 0; i < table_counter; i++)
		{
			if (id_merenja == 1)
			{
				if (strcmp(table[i].device_name, "Switch 1") == 0)
				{
					table[i].value = poruka->value;
					break;
				}
			}
			else
			{
				if (strcmp(table[i].device_name, "Switch 2") == 0)
				{
					table[i].value = poruka->value;
					break;
				}
			}
		}

	}
	

	
	
}

void print_table()
{
	printf("Devices: \n");
	for (int i = 0; i < table_counter; i++)
	{
		printf("Name: %s ", table[i].device_name);
		printf("Value: ");
		if (table[i].id >> 12 == 1)
		{
			printf("%d V\n", table[i].value);
		}
		else
		{
			if (table[i].value == true)
			{
				printf(" close.\n");
			}
			else
			{
				printf(" open.\n");
			}
		}
	}
}
