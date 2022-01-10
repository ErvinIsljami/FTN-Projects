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
unsigned char* encrypt_data_cezar(const unsigned char* packet_data, unsigned char* app_data, int app_length);
unsigned char* encrypt_data_vizner(const unsigned char* packet_data, unsigned char* app_data, int app_length);
unsigned char* encrypt_data_transpozicija(const unsigned char* packet_data, unsigned char* app_data, int app_length);

void print_message_as_table(unsigned char* data, int rows_max, int columns_max);										// Prints application data using table format
int find_key(int key, int * keys, int keys_size);

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
	pcap_loop(device_handle, 0, packet_handler, (unsigned char*)file_dumper);

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

	ip_header* ih = (ip_header*)(packet_data + sizeof(ethernet_header));
	//racunanje checkSume
	ih->checksum = 0;
	unsigned short ipCheckSum = calculate_checksum((unsigned short*)ih, ih->header_length * 4 / 2);
	ih->checksum = htons(ipCheckSum);

	//verifikovanje checksume
	unsigned short ipCheckSum2 = calculate_checksum((unsigned short*)ih, ih->header_length * 4 / 2);
	if (ipCheckSum2 == 0)
		printf("IP Checksuma je u redu. \n");
	else
		printf("Ip checksuma nije u redu \n");

	/************************************UDP CHECKSUM*******************************************/
	if (ih->next_protocol != 17)
	{
		return;
	}
	udp_header* uh = (udp_header*)((unsigned char*)ih + ih->header_length * 4);

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
	unsigned short udp_chcks = calculate_checksum((unsigned short *)udp_checksum, duzina / 2);
	printf("udp check suma je %x \n\n\n", udp_chcks);

	udp_chcks = htons(udp_chcks);
	//uh->checksum = htons(udp_chcks);
	memcpy(udp_checksum + 18, &udp_chcks, 2);

	//verifikovanje udp checksume
	udp_chcks = calculate_checksum((unsigned short *)udp_checksum, duzina / 2);

	if (udp_chcks == 0)
		printf("UDP Check suma ok\n");
	else
		printf("UDP Check suma nije ok\n");


	free(udp_checksum);
	/************************************** KRIPTOVANJE **********************************************/

	unsigned char* app_data = (unsigned char *)uh + sizeof(udp_header);
	int app_length = ntohs(uh->datagram_length) - sizeof(udp_header);

	printf("Rec je: %s\n", app_data);

	unsigned char * encrypted_packet = encrypt_data_transpozicija(packet_data, app_data, app_length);
	//unsigned char * encrypted_packet = encrypt_data_vizner(packet_data, app_data, app_length);
	//unsigned char * encrypted_packet = encrypt_data_cezar(packet_data, app_data, app_length);


	pcap_dump((unsigned char*)file_dumper, packet_header, encrypted_packet); //upis u fajl
}

// Returns a copy of packet with encrypted application data
unsigned char* encrypt_data_cezar(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	// Reserve memory for copy of the packet
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data); //pokazivac na app deo kopije paketa

	memcpy(encrypted_packet, packet_data, app_data - packet_data);

	for (int i = 0; i < app_length; i++)
	{
		encrypted_data[i] = app_data[i] + 3;
		if (encrypted_data[i] > 'Z')
			encrypted_data[i] -= 26;
	}
	encrypted_data[app_length] = '\0';

	printf("Kriptovan paket je: %s", encrypted_data);


	return encrypted_packet;
}

unsigned char* encrypt_data_vizner(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	// Reserve memory for copy of the packet
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data); //pokazivac na app deo kopije paketa
	memcpy(encrypted_packet, packet_data, app_data - packet_data);

	char kodnaRec[] = "ORM";
	unsigned char *produzenaRec = (unsigned char*)malloc(app_length + 1);

	for (int i = 0; i < app_length; i++)
	{
		produzenaRec[i] = kodnaRec[i % 3];
	}
	produzenaRec[app_length] = '\0';

	printf("Produzena rec: %s\n", produzenaRec);

	for (int i = 0; i < app_length; i++)
	{
		encrypted_data[i] = app_data[i] + (produzenaRec[i] - 'A');
		if (encrypted_data[i] > 'Z')
			encrypted_data[i] -= 26;
	}
	encrypted_data[app_length] = '\0';

	printf("Sifrovana poruka je: %s \n", encrypted_data);


	return encrypted_packet;
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

	unsigned short checksum = ~(unsigned short)sum; //komplement1

	return checksum;
}

/*
3x7
q w e r t y u
a s d f g h j
z x c v b n m

q w e r t y u a s d f g h j z x c v b n m

c -> 16
c -> [2][2]

[2][2] == 2 * 7 + 2
trenutniRed * maxKolona + trenutnaKolona

*/
void print_message_as_table(unsigned char* data, int rows_max, int columns_max)
{
	for (int i = 0; i < rows_max; i++)
	{
		for (int j = 0; j < columns_max; j++)
		{
			printf("%c ", data[i*columns_max + j]);
		}
		printf("\n");
	}
}
int find_key(int key, int * keys, int keys_size)
{
	for (int i = 0; i < keys_size; i++)
	{
		if (key == keys[i])
			return i;
	}
	return -1;
}

unsigned char* encrypt_data_transpozicija(const unsigned char* packet_data, unsigned char* app_data, int app_length)
{
	unsigned char encrypted_packet[ETHERNET_FRAME_MAX];	//kopija paketa
	unsigned char *encrypted_data = encrypted_packet + (app_data - packet_data); //pokazivac na app deo kopije paketa
	memcpy(encrypted_packet, packet_data, app_data - packet_data);
	int row_max = 3;
	int column_max = 7;
	int rows[] = { 1, 0, 2 };
	int columns[] = { 1, 0, 6, 2, 4, 3, 5 };

	print_message_as_table(app_data, row_max, column_max);
	
	
	for (int i = 0; i < row_max; i++)
	{
		for (int j = 0; j < column_max; j++)
		{
			int pomI = find_key(i, rows, row_max);
			int pomJ = find_key(j, columns, column_max);
			encrypted_data[pomI * column_max + pomJ] = app_data[i * column_max + j];	
		}
		printf("\n");
	}



	printf("---------------------------------------\n");
	print_message_as_table(encrypted_data, row_max, column_max);



	return encrypted_packet;
}