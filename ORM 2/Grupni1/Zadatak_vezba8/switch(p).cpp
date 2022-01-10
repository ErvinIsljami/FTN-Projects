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
#define ETHERNET_FRAME_MAX 1518
typedef struct table_entry_st
{
	unsigned char network_address[4];
	unsigned char subnet_mask[4];
	int output_port;
	int metric;
}table_entry;

table_entry routing_table[10];
int table_counter = 2;							// Counts free locations in table

typedef struct route_st
{
	unsigned char network_address[4];
	unsigned char subnet_mask[4];
	int metric;
}route;

/* Declarations */
void process_packet(int in, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);	// Frame analysis, address learning and forwarding frames
void send_packet(int output_port, const pcap_pkthdr* packet_header, const unsigned char* packet_data);	// Provides sending frames to the output ports of the switch
bool compare_ip_addresses(unsigned char* first_address, unsigned char* second_address);				// Checks if two physical addresses are equal
bool insert_new_address(unsigned char* address, int port);												// Inserts new address in to the switch table
void print_routing_table();


/* Global variables */

// Ports
pcap_t* input_files[SWITCH_PORTS];			// Input ports of the switch (input files)
pcap_dumper_t* output_files[SWITCH_PORTS];	// Output ports of the switch (output files)


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
	routing_table[1].metric = 5;
	print_routing_table();




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

void process_packet(int input_port, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data)
{
	unsigned char kopija[ETHERNET_FRAME_MAX];

	memset(kopija, 0, ETHERNET_FRAME_MAX);
	memcpy(kopija, packet_data, packet_header->len);	//kopiram ceo paket u kopiju

	ethernet_header* eh = (ethernet_header*)kopija;

	ip_header* ih = (ip_header*)(kopija + sizeof(ethernet_header));
	if (ih->next_protocol != 0x11) // UDP = 0x11
		return;

	int length_bytes = ih->header_length * 4; // header length is calculated

	udp_header* uh = (udp_header*)((unsigned char*)ih + length_bytes);

	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	printf("Logicka adresa primaoca: %d.%d.%d.%d \n", ih->dst_addr[0], ih->dst_addr[1], ih->dst_addr[2], ih->dst_addr[3]);
	printf("Logicka adresa posiljaoca: %d.%d.%d.%d \n", ih->src_addr[0], ih->src_addr[1], ih->src_addr[2], ih->src_addr[3]);
	
	ih->ttl--;	//osvezavam time to live
	if (ih->ttl == 0)
		return;

	int out_port = -1;
	int min_metric = 10000;

	for (int i = 0; i < table_counter; i++)
	{
		if (check_address_in_network(routing_table[i].network_address, ih->dst_addr, routing_table[i].subnet_mask))
		{
			if (min_metric > routing_table[i].metric)
			{
				min_metric = routing_table[i].metric;
				out_port = routing_table[i].output_port;
			}
		}
	}

	send_packet(out_port, packet_header, kopija);

}
void process_routing_informatinom(int input_port, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data)
{
	/*for (int output_port = 0; output_port < SWITCH_PORTS; output_port++)
	{
		if(output_port != input_port)
			send_packet(output_port, packet_header, packet_data);
	}*/
	ethernet_header * eh = (ethernet_header *)packet_data;

	ip_header* ih = (ip_header*)(packet_data + sizeof(ethernet_header));
	if (ih->next_protocol != 17)
	{
		return;
	}
	udp_header* uh = (udp_header*)((unsigned char*)ih + ih->header_length * 4);
	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	route *podaci = (route*)app_data;
	printf("App podaci: \n");
	printf("Network address: %d.%d.%d.%d \n", podaci->network_address[0], podaci->network_address[1], podaci->network_address[2], podaci->network_address[3]);
	printf("Subnet mask: %d.%d.%d.%d \n", podaci->subnet_mask[0], podaci->subnet_mask[1], podaci->subnet_mask[2], podaci->subnet_mask[3]);
	printf("Metric: %d\n", podaci->metric);
	bool nasao = false;
	for (int i = 0; i < table_counter; i++)
	{
		if (compare_ip_addresses(podaci->network_address, routing_table[i].network_address))
		{
			if (compare_ip_addresses(podaci->subnet_mask, routing_table[i].subnet_mask))
			{
				nasao = true;
				if (routing_table[i].output_port == input_port)
				{
					routing_table[i].metric = podaci->metric;	//osvezavam metriku ako su mi iste adrese, maske i portovi
				}
				else if (routing_table[i].metric > podaci->metric) //ako su adresa i maska iste a portovi nisu proveravam da li je nova metrika manja od stare
				{
					routing_table[i].metric = podaci->metric;	//ako jeste azuriram novu metriku i novi port
					routing_table[i].output_port = input_port;
				}
			}
		}
	}

	if (!nasao)
	{
		routing_table[table_counter].metric = podaci->metric;
		routing_table[table_counter].output_port = input_port;
		routing_table[table_counter].network_address[0] = podaci->network_address[0];
		routing_table[table_counter].network_address[1] = podaci->network_address[1];
		routing_table[table_counter].network_address[2] = podaci->network_address[2];
		routing_table[table_counter].network_address[3] = podaci->network_address[3];
		routing_table[table_counter].subnet_mask[0] = podaci->subnet_mask[0];
		routing_table[table_counter].subnet_mask[1] = podaci->subnet_mask[1];
		routing_table[table_counter].subnet_mask[2] = podaci->subnet_mask[2];
		routing_table[table_counter].subnet_mask[3] = podaci->subnet_mask[3];
		table_counter++;
		print_routing_table();
	}

}


bool compare_ip_addresses(unsigned char * first_address, unsigned char * second_address)
{
	for (int i = 0; i < 4; i++)
		if (first_address[i] != second_address[i])
			return false;

	return true;
}


void print_routing_table()
{
	printf("Routing table: \n");
	for (int i = 0; i < table_counter; i++)
	{
		printf("Network address: %d.%d.%d.%d \n", routing_table[i].network_address[0], routing_table[i].network_address[1], routing_table[i].network_address[2], routing_table[i].network_address[3]);
		printf("Subnet mask: %d.%d.%d.%d \n", routing_table[i].subnet_mask[0], routing_table[i].subnet_mask[1], routing_table[i].subnet_mask[2], routing_table[i].subnet_mask[3]);
		printf("Port: %d\n", routing_table[i].output_port);
		printf("Metric: %d\n", routing_table[i].metric);
		printf("-------------------------------------\n");
	}
}


bool check_address_in_network(unsigned char * hst_addr, unsigned char * net_addr, unsigned char * mask)
{
	for (int i = 0; i < 4; i++)
	{
		if (hst_addr[i] & mask[i] != net_addr[i] & mask[i])
			return false;
	}

	return true;
}