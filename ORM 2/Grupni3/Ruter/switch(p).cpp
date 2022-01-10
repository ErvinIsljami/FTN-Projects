// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 8 - Switch

// We do not want the warnings about the old deprecated and unsecure CRT functions since these examples can be compiled under *nix as well
#ifdef _MSC_VER
	#define _CRT_SECURE_NO_WARNINGS
#endif

// Include libraries
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include <winsock2.h>
#include <windows.h>
#include <ws2tcpip.h>
#include "conio.h"
#include "pcap.h"
#include "protocol_headers.h"

#define SWITCH_PORTS 4
#define SWITCH_TABLE_MAX 10
#define MAC_ADDRESS_SIZE 6

typedef struct table_entry_st
{
	unsigned char network_address[4];
	unsigned char subnet_mask[4];
	int output_port;
	int metric;
}table_entry;

table_entry routing_table[10];
int table_counter = 3;

/* Declarations */
void process_packet(int in, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);	// Frame analysis, address learning and forwarding frames
void send_packet(int output_port, const pcap_pkthdr* packet_header, const unsigned char* packet_data);	// Provides sending frames to the output ports of the switch
bool compare_mac_addresses(unsigned char* first_address, unsigned char* second_address);				// Checks if two physical addresses are equal
bool insert_new_address(unsigned char* address, int port);												// Inserts new address in to the switch table

struct table_element						// Record in switch table (maps MAC address to specific port)
{
	unsigned char address[MAC_ADDRESS_SIZE];
	int port;
};


/* Global variables */

// Ports
pcap_t* input_files[SWITCH_PORTS];			// Input ports of the switch (input files)
pcap_dumper_t* output_files[SWITCH_PORTS];	// Output ports of the switch (output files)

// Switch table
table_element switch_table[SWITCH_TABLE_MAX];	// Switch table with records
int table_counter = 0;							// Counts free locations in table

// Function which simulate receiving messages on the input ports of the switch
int main()
{
	routing_table[0].network_address[0] = 192;
	routing_table[0].network_address[1] = 168;
	routing_table[0].network_address[2] = 0;
	routing_table[0].network_address[3] = 0;
	routing_table[0].subnet_mask[0] = 255;
	routing_table[0].subnet_mask[1] = 255;
	routing_table[0].subnet_mask[2] = 0;
	routing_table[0].subnet_mask[3] = 0;
	routing_table[0].output_port = 0;
	routing_table[0].metric = 100;

	routing_table[1].network_address[0] = 192;
	routing_table[1].network_address[1] = 168;
	routing_table[1].network_address[2] = 3;
	routing_table[1].network_address[3] = 0;
	routing_table[1].subnet_mask[0] = 255;
	routing_table[1].subnet_mask[1] = 255;
	routing_table[1].subnet_mask[2] = 255;
	routing_table[1].subnet_mask[3] = 0;
	routing_table[1].output_port = 1;
	routing_table[1].metric = 1;

	routing_table[2].network_address[0] = 192;
	routing_table[2].network_address[1] = 168;
	routing_table[2].network_address[2] = 2;
	routing_table[2].network_address[3] = 0;
	routing_table[2].subnet_mask[0] = 255;
	routing_table[2].subnet_mask[1] = 255;
	routing_table[2].subnet_mask[2] = 255;
	routing_table[2].subnet_mask[3] = 0;
	routing_table[2].output_port = 2;
	routing_table[2].metric = 10;

	char error_buffer[PCAP_ERRBUF_SIZE];

	char input_filename[15] = "port_in_X.pcap";			// Generic name of input files
	char output_filename[16] = "port_out_X.pcap";		// Generic name of output files

	for(int i = 0; i < SWITCH_PORTS; i++)
	{
		input_filename[8] = i + 48;		// convert 'i' to ascii 
		output_filename[9] = i + 48;

		// Open the capture files (input ports)
		if ((input_files[i] = pcap_open_offline(input_filename, error_buffer)) == NULL)

		{
			printf("\n Unable to open the file %s.\n", input_filename);
			return -1;
		}

		// Open the dump files (output ports)
		output_files[i] = pcap_dump_open(input_files[i], output_filename);

		if (output_files[i] == NULL)
		{
			printf("\n Error opening output file %s\n", output_filename);
			return -1;
		}

		// Check the link layer. We support only Ethernet for simplicity.
		if(pcap_datalink(input_files[i]) != DLT_EN10MB)
		{
			printf("\nLink layer of input file (%d) is not Ethernet.\n", i);
			return -1;
		}
	}

	int packet_counter = 0;	// Count received packets				

	time_t t;
	srand((unsigned) time(&t));
	int input_port = rand() % SWITCH_PORTS;		// Choose port for receiving packets

	struct pcap_pkthdr* packet_header;	// Header of packet (timestamp and length)
	const unsigned char* packet_data;	// Packet content

    // Retrieve 30 packets
	while(packet_counter < 30)
	{
		if(pcap_next_ex(input_files[input_port], &packet_header, &packet_data) > 0)
		{
			// Process each packet
			process_packet(input_port, packet_header, packet_data);
			packet_counter++;
		}
		input_port = rand() % SWITCH_PORTS;		// Choose port for receiving packets
	}

	printf("\nPress enter for exit");
	_getch();

	// Close the files associated with 'input_files' variable and deallocates resources
	for(int i = 0; i < SWITCH_PORTS; i++)
	{
		pcap_close(input_files[i]);
	}

	return 0;
}

// Provides sending frames to the output ports of the switch
void send_packet(int output_port, const pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	pcap_dump((unsigned char*) output_files[output_port], packet_header, packet_data);	// Send to the port with 'out' index
}

// Frame analysis, address learning and forwarding frames
void process_packet(int input_port, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data)
{
	//tacka 1. vezbe 5 strana 9
	unsigned char kopija[1518];	//max_ether_size = 1518
	memset(kopija, 0, 1518);	//setujem memoriju na 0
	memcpy(kopija, packet_data, packet_header->len);	//kopiram paket

	ethernet_header* eh = (ethernet_header*)kopija;
	ip_header* ih = (ip_header*)(kopija + sizeof(ethernet_header));
	
	if (ih->next_protocol != 0x11) // UDP = 0x11
		return;
	//tacka 2.
	ih->ttl--;
	if (ih->ttl == 0)
		return;	//odbacuje se paket

	//tacka 3.
	//izracunati ip check sumu

	int length_bytes = ih->header_length * 4; // header length is calculated
		udp_header* uh = (udp_header*)((unsigned char*)ih + length_bytes);

	int min = 10000;
	int out_port = -1;
	for (int i = 0; i < table_counter; i++)
	{
		if (check_address_in_network(ih->dst_addr, routing_table[i].network_address, routing_table[i].subnet_mask))
		{
			if (min > routing_table[i].metric)
			{
				min = routing_table[i].metric;
				out_port = routing_table[i].output_port;
			}
		}
	}

	send_packet(out_port, packet_header, packet_data);
}

bool check_address_in_network(unsigned char* host_address, unsigned char* network_address, unsigned char* subnet_mask)
{
	for (int i = 0; i < 4; i++)
	{
		if (host_address[i] & subnet_mask[i] != network_address[i] & subnet_mask[i])
			return false;
	}

	return true;
}