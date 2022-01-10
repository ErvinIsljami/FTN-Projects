// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 9 - Switch

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


/* Declarations */


void send_packet(int output_port, const pcap_pkthdr* packet_header, const unsigned char* packet_data);		// Provides sending frames to the output ports of the switch
bool compare_mac_addresses(unsigned char* first_address, unsigned char* second_address);					// Checks if two physical addresses are equal
void process_packet(int in, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);		// Frame analysis, address learning and forwarding frames
bool insert_new_address(unsigned char* address, int port);													// Inserts new address-port pair in to the switch table

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
table_element switch_table[10];			// Switch table with records
int table_counter = 0;					// Counts free locations in table

// Function which simulate receiving messages on the input ports of the switch
int main()
{

	char error_buffer[PCAP_ERRBUF_SIZE];

	char input_filename[15] = "port_in_X.pcap";			// Generic name of input files
	char output_filename[16] = "port_out_X.pcap";		// Generic name of output files

	for (int i = 0; i < SWITCH_PORTS; i++)
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
		if (pcap_datalink(input_files[i]) != DLT_EN10MB)
		{
			printf("\nLink layer of input file (%d) is not Ethernet.\n", i);
			return -1;
		}
	}

	int packet_counter = 0;	// Count received packets				

	time_t t;
	srand((unsigned)time(&t));
	int input_port = rand() % SWITCH_PORTS;		// Choose port for receiving packets

	struct pcap_pkthdr* packet_header;	// Header of packet (timestamp and length)
	const unsigned char* packet_data;	// Packet content

	// Retrieve 30 packets
	while (packet_counter < 30)
	{
		if (pcap_next_ex(input_files[input_port], &packet_header, &packet_data) > 0)
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
	for (int i = 0; i < SWITCH_PORTS; i++)
	{
		pcap_close(input_files[i]);
	}

	return 0;
}

// Provides sending frames to the output ports of the switch
void send_packet(int output_port, const pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	pcap_dump((unsigned char*)output_files[output_port], packet_header, packet_data);	// Send to the port with 'out' index
}

// Frame analysis, address learning and forwarding frames
void process_packet(int input_port, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data)
{
	// Retrieve position of ethernet header
	ethernet_header* eh = (ethernet_header*)packet_data;

	printf("\n\n===================================================");
	printf("\nMESSAGE:\n%d: ", input_port);
	printf("%.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->src_address[0], eh->src_address[1], eh->src_address[2], eh->src_address[3], eh->src_address[4], eh->src_address[5]);
	printf(" -> %.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->dest_address[0], eh->dest_address[1], eh->dest_address[2], eh->dest_address[3], eh->dest_address[4], eh->dest_address[5]);

	/* ADDRESS LEARNING */

	bool found = false;

	// Find source MAC address in table
	for (int j = 0; j < table_counter; j++)
	{
		if (compare_mac_addresses(eh->src_address, switch_table[j].address) == true)
		{
			found = true;
			break;
		}
	}

	// Store new MAC record in table
	if (!found)
	{
		insert_new_address(eh->src_address, input_port);
	}

	printf("\n---------------------------------------------------");
	printf("\nSWITCH TABLE: (%d)", table_counter);
	for (int i = 0; i < table_counter; i++)
	{
		printf("\n%.2x:%.2x:%.2x:%.2x:%.2x:%.2x: %d",
			switch_table[i].address[0], switch_table[i].address[1], switch_table[i].address[2], switch_table[i].address[3], switch_table[i].address[4], switch_table[i].address[5],
			switch_table[i].port);
	}
	printf("\n---------------------------------------------------");

	/* PACKET FORWARDING */

	int output_port = -1;

	// Find destionation MAC in table
	for (int j = 0; j < table_counter; j++)
	{
		if (compare_mac_addresses(eh->dest_address, switch_table[j].address))
		{
			output_port = switch_table[j].port;
			break;
		}
	}

	// If MAC record exist in table
	if (output_port != -1)
	{
		// ...and its port is not the same as input port
		if (output_port != input_port)
		{
			// FORWARD - send message using the appropriate port
			send_packet(output_port, packet_header, packet_data);
			printf("\nFORWARDING: sending packet to the port: %d", output_port);
		}
		else
		{
			//DROP - ignore message
			printf("\nDROPPING");
		}
	}
	else
	{
		// FLOOD - broadcast message using all ports

		printf("\nFLOODING: sending packet to the next ports: ");

		for (output_port = 0; output_port < SWITCH_PORTS; output_port++)
		{
			if (output_port != input_port)	// do not send on the same port
			{
				printf("%d,", output_port);
				send_packet(output_port, packet_header, packet_data);
			}
		}
	}
	printf("\n===================================================");
}

// Checks if two physical addresses are equal
bool compare_mac_addresses(unsigned char* first_address, unsigned char* second_address)
{
	for (int i = 0; i < MAC_ADDRESS_SIZE; i++)
	{
		if (first_address[i] != second_address[i])
			return false;
	}

	return true;
}

// Inserts new address-port pair in to the switch table
bool insert_new_address(unsigned char* address, int port)
{
	// check if switch table is full
	if (table_counter >= SWITCH_TABLE_MAX)
	{
		return false;
	}

	// copy address and port in switch table and increment the counter
	//memcpy(switch_table[table_counter].address, address, MAC_ADDRESS_SIZE);
	switch_table[table_counter].address[0] = address[0];
	switch_table[table_counter].address[1] = address[1];
	switch_table[table_counter].address[2] = address[2];
	switch_table[table_counter].address[3] = address[3];
	switch_table[table_counter].address[4] = address[4];
	switch_table[table_counter].address[5] = address[5];
	switch_table[table_counter].port = port;
	table_counter++;
	return true;
}