// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 11 - Enkripcija

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
unsigned char* encrypt_data_cezar(const unsigned char* packet_data, unsigned char* app_data, int app_length);					// Returns a copy of packet with encrypted application data
unsigned char* encrypt_data_vizner(const unsigned char* packet_data, unsigned char* app_data, int app_length);
unsigned char* encrypt_data_transpozicija(const unsigned char* packet_data, unsigned char* app_data, int app_length);


void print_message_as_table(unsigned char* data, int rows_max, int columns_max);										// Prints application data using table format
int find_key(int key, int * keys, int keys_size);																		// Finds specific key in array of keys and returns its index

unsigned short calculate_checksum(unsigned short * data, int data_length);

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


	getchar();
	return 0;
}

// Callback function invoked by WinPcap for every incoming packet
void packet_handler(unsigned char* file_dumper, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	ethernet_header * eh = (ethernet_header *)packet_data;
	/********************************IP CHECKSUM*******************************************/

	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
	//verifikovanje ip checksum-e
	unsigned short sumaIP = calculate_checksum((unsigned short*)ih, ih->header_length * 4 / 2);
	if (sumaIP == 0)
		printf("dobra ip chsuma\n");
	else
		printf("nije dobra\n");

	//izracunavanje ip checksum-e
	ih->checksum = 0;
	unsigned short sumaIP2 = calculate_checksum((unsigned short*)ih, ih->header_length * 4 / 2);

	/************************************UDP CHECKSUM*******************************************/
	if(ih->next_protocol != 17)
	{
		return;
	}
	udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);

	int duzina = 12 + ntohs(uh->datagram_length);
	if (duzina % 2 == 1)
		duzina++;

	unsigned char * udp_checksum = (unsigned char*)malloc(duzina);
	memset(udp_checksum, 0, duzina);

	memcpy(udp_checksum, ih->src_addr, 4);
	memcpy(udp_checksum + 4, ih->dst_addr, 4);
	udp_checksum[8] = 0;
	udp_checksum[9] = ih->next_protocol;
	memcpy(udp_checksum + 10, &(uh->datagram_length), 2);
	memcpy(udp_checksum + 12, uh, ntohs(uh->datagram_length));	//kopira sve podatke iz udp-a i app data

	//racunanje checksume
	memset(udp_checksum + 18, 0, 2);	//postavlja polje checksum na nulu posto treba sad da je izracuna
	unsigned short udp_chcks = calculate_checksum((unsigned short *)udp_checksum, duzina);
	printf("udp check suma je %x \n\n\n", udp_chcks);

	udp_chcks = htons(udp_chcks);
	memcpy(udp_checksum + 18, &udp_chcks, 2);
	
	//verifikovanje udp checksume
	udp_chcks = calculate_checksum((unsigned short *)udp_checksum, duzina);

	if (udp_chcks == 0)
		printf("UDP Check suma ok\n");
	else
		printf("UDP Check suma nije ok\n");


	free(udp_checksum);
	/**************************************KRIPTOVANJE TRANSPOZICIJA**********************************************/

	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	printf("Rec je: %s\n", app_data);

	unsigned char * encrypted_packet = encrypt_data_transpozicija(packet_data, app_data, app_length);

	
	pcap_dump((unsigned char*) file_dumper, packet_header, encrypted_packet);
}

unsigned short calculate_checksum(unsigned short * data, int data_length)
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

	unsigned short checksum = ~(unsigned short)sum;

	return checksum;
}

unsigned char* encrypt_data_cezar(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	// Reserve memory for copy of the packet
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data);	//pozicioniram se na app deo kopije
	int pomeraj = 3;
	memcpy(encrypted_packet, packet_data, app_data - packet_data);	//kopiram u kopiju sve osim app dela

	for (int i = 0; i < app_length; i++)
	{
		encrypted_data[i] = app_data[i] + pomeraj;
		if (encrypted_data[i] > 'Z')	//ako prekoraci slova
			encrypted_data[i] -= 26;	//vrati ga
	}
	encrypted_data[app_length] = '\0';
	printf("Kriptovano je: %s \n", encrypted_data);



	return encrypted_packet;
}

unsigned char* encrypt_data_vizner(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	// Reserve memory for copy of the packet
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data);	//pozicioniram se na app deo kopije
	char rec[] = "ORM";
	unsigned char *kodnaRec = (unsigned char*)malloc(app_length + 1);
	memcpy(encrypted_packet, packet_data, app_data - packet_data);	//kopiram u kopiju sve osim app dela

	for (int i = 0; i < app_length; i++)
	{
		kodnaRec[i] = rec[i % 3];
	}
	kodnaRec[app_length] = '\0';
	printf("Kodna rec je: %s\n", kodnaRec);

	for (int i = 0; i < app_length; i++)
	{
		encrypted_data[i] = app_data[i] + kodnaRec[i] - 'A';
		if (encrypted_data[i] > 'Z')
			encrypted_data[i] -= 26;
	}
	encrypted_data[app_length] = '\0';
	/*
	"UNIVERZITETUNOVOMSADU"
	"ORMORMORMORMORMORMORM"
	""
	*/


	printf("Kriptovano je: %s \n", encrypted_data);



	return encrypted_packet;
}

unsigned char* encrypt_data_transpozicija(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data);	//pozicioniram se na app deo kopije

	int max_rows = 3;
	int max_columns = 7;
	int rows[] = { 1, 0, 2 };
	int columns[] = { 1, 0, 6, 2, 4, 3, 5 };


	print_message_as_table(app_data, max_rows, max_columns);
	for (int i = 0; i < max_rows; i++)
	{
		for (int j = 0; j < max_columns; j++)
		{
			int pomI = find_key(i, rows, max_rows);
			int pomJ = find_key(j, columns, max_columns);

			encrypted_data[pomI * max_columns + pomJ] = app_data[i * max_columns + j];
		}
	}
	printf("---------------------------\n");
	print_message_as_table(encrypted_data, max_rows, max_columns);

	return encrypted_packet;
}

void print_message_as_table(unsigned char* data, int rows_max, int columns_max)
{
	for (int i = 0; i < rows_max; i++)
	{
		for (int j = 0; j < columns_max; j++)
		{
			printf("%C ", data[ i * columns_max + j]);
		}
		printf("\n");
	}
}
/*
3x7
1 2 3 4 5 6 7
1 2 3 4 5 6 7
1 2 3 4 5 6 7

1 2 3 4 5 6 7 1 2 3 4 5 6 7 1 2 3 4 5 6 7

17 = trenutniRed * ukupnoKolona + trenutnaKolona

[2][3]
*/

int find_key(int key, int * keys, int keys_size)
{
	for (int i = 0; i < keys_size; i++)
	{
		if (key == keys[i])
			return i;
	}

	return -1;
}