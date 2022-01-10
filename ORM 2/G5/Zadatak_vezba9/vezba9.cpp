// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 9 - Enkripcija

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
unsigned char* encrypt_data(const unsigned char* packet_data, unsigned char* app_data, int app_length);					// Returns a copy of packet with encrypted application data
void print_message_as_table(unsigned char* data, int rows_max, int columns_max);										// Prints application data using table format
int find_key(int key, int * keys, int keys_size);																		// Finds specific key in array of keys and returns its index

#define ETHERNET_FRAME_MAX 1518		// Maximal length of ethernet frame

// Main function captures packets from the file
int main()
{
	pcap_t* device_handle;
	char error_buffer[PCAP_ERRBUF_SIZE];
	
	// Open the capture file 
	if ((device_handle = pcap_open_offline("original_packets.pcap", // Name of the device
								error_buffer	  // Error buffer
							)) == NULL)
	{
		printf("\n Unable to open the file %s.\n", "example.pcap");
		return -1;
	}

	// Open the dump file 
	pcap_dumper_t* file_dumper = pcap_dump_open(device_handle, "encrypted_packets.pcap");
	

	if (file_dumper == NULL)
	{
		printf("\n Error opening output file\n");
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

	// Read and dispatch packets until EOF is reached
	pcap_loop(device_handle, 0, packet_handler, (unsigned char*) file_dumper);

	// Close the file associated with device_handle and deallocates resources
	pcap_close(device_handle);

	return 0;
}

// Callback function invoked by WinPcap for every incoming packet
void packet_handler(unsigned char* file_dumper, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	/* DATA LINK LAYER - Ethernet */

	// Retrive the position of the ethernet header
	ethernet_header * eh = (ethernet_header *)packet_data;
	
	/* NETWORK LAYER - IPv4 */

	// Retrieve the position of the ip header
	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
	
	// TRANSPORT LAYER - UDP
	if(ih->next_protocol != 17)
	{
		return;
	}
	
	// Retrieve the position of udp header
	udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);

	// Retrieve the position of application data
	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);

	// Total length of application data
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	// Encrypt data using tranposition of rows and columns
	unsigned char * encrypted_packet = encrypt_data(packet_data, app_data, app_length);

	// Dump encrypted packets
	pcap_dump((unsigned char*) file_dumper, packet_header, encrypted_packet);
}

// Returns a copy of packet with encrypted application data
unsigned char* encrypt_data(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	// TODO 1: Define keys

	// 1st and 2nd keys - Matrix dimension
	const int row_length = 3;
	const int column_length = 7;

	// 3rd and 4th keys - Transpositions
	int row_keys[row_length] = { 1, 0, 2 };
	int column_keys[column_length] = { 1, 0, 6, 2, 4, 3, 5 };


	// TODO 2: Print original message in table format
	print_message_as_table((unsigned char*)app_data, row_length, column_length);
	

	// TODO 3: Create a copy of the packets (copy headers and initialize application data with zeros)

	// Reserve memory for copy of the packet
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];

	// Calculate position of application data
	int app_position = app_data - packet_data;
	unsigned char * encrypted_message = encrypted_packet + app_position;

	// Copy headers
	memcpy(encrypted_packet, packet_data, app_position);

	// Initialize memory reserved for application data
	memset(encrypted_message, 0, app_length);
	

	// TODO 5: Encrypt application data
	for (int row = 0; row < row_length; row = row + 1)
	{
		for (int column = 0; column < column_length; column = column+1)
		{
			// TODO 4: Find new row and column indices using old row and column indices
			int row_out = find_key(row, row_keys, row_length);
			int column_out = find_key(column, column_keys, column_length);

			// Calculate index in array using row and column indices
			//           (for input and ouput data)
			int index_in = row * column_length + column;
			int index_out = row_out * column_length + column_out;

			// Copy element to calculated position
			encrypted_message[index_out] = app_data[index_in];
		}
	}

	// TODO 6: Print encrypted message
	print_message_as_table(encrypted_message, row_length, column_length);

	return encrypted_packet;
}

// Prints application data using table format
void print_message_as_table(unsigned char* data, int rows_max, int columns_max)
{
	printf("\n\t");
	for(int row = 0; row < rows_max; row = row + 1)
	{
		for(int column = 0; column < columns_max; column = column + 1)
		{
			printf("%c ", (data)[row * columns_max + column]);
		}
		printf("\n\t");
	}
}

// Finds specific key in array of keys and returns its index.
int find_key (int key, int * keys, int keys_length)
{
	for (int index = 0; index < keys_length; index = index+1)
	{
		if(keys[index] == key)
			return index;
	}

	return -1;
}