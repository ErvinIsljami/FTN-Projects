// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 6 - Interpretacija sadrzaja paketa (2.deo)

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
void packet_handler(unsigned char* param, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data);

int udp_counter = 0;


int main()
{
	do	//tacka 3.
	{
		//tacka 10.
		printf("TLS koristi port 443\n");

		//tacka 1.
		pcap_if_t* devices; // List of network interfaces
		pcap_if_t* device; // Network interface
		int i = 0; // Interface counter
		char errorMsg[PCAP_ERRBUF_SIZE + 1]; // Buffer for errors
		// Retrieve the device list of network intefaces
		if (pcap_findalldevs(&devices, errorMsg) == -1)
		{
			printf("Error in pcap_findalldevs: %s\n", errorMsg);
			return 1;
		}
		// Print the list of network interfaces
		for (device = devices; device; device = device->next)
		{
			printf("%d. %s", ++i, device->name);
			if (device->description)
				printf(" (%s)\n", device->description);
			else
				printf(" (No description available)\n");
		}
		if (i == 0)
		{
			printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
			return -1;
		}
		// Pick one device from the list
		int device_number;
		printf("Enter the interface number (1-%d):", i);
		scanf_s("%d", &device_number);

		if (device_number < 1 || device_number > i)
		{
			printf("\nInterface number out of range.\n");
			return NULL;
		}

		// Jump to the selected device
		for (device = devices, i = 0; i < device_number - 1; device = device->next, i++);

		//tacka 2.
		pcap_t* device_handle;
		// Open the adapter
		if ((device_handle = pcap_open_live(device->name, // name of the device
			65536, // portion of the packet to capture.
			1, // promiscuous mode, ako teba normal rezim onda 0
			2500, // read timeout
			errorMsg // error buffer
		)) == NULL)
		{
			printf("\n Unable to open the adapter %s.\n", errorMsg);
			// Free the device list
			pcap_freealldevs(devices);
			return -1;
		}

		//tacka 3.
		if (pcap_datalink(device_handle) != DLT_EN10MB) // DLT_EN10MB oznacava Ethernet
		{
			printf("\nThis program works only on Ethernet networks.\n");
			// Free the device list
			pcap_freealldevs(devices);
			//return -1;
			continue;
		}


		//tacka 5.
		unsigned int netmask;
		char filter_exp[] = "ip and ((tcp or udp) and portrange 0-65535)";
		struct bpf_program fcode;
		if (device->addresses != NULL)
			// Retrieve the mask of the first address of the interface
			netmask = ((struct sockaddr_in*)(device->addresses->netmask))
			->sin_addr.s_addr;
		else
			// If the interface is without an address
			// we suppose to be in a C class network
			netmask = 0xffffff;
		// Compile the filter
		if (pcap_compile(device_handle, &fcode, filter_exp, 1, netmask) < 0)
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


		//tacka 4.
		pcap_loop(device_handle, 25, packet_handler, NULL);











		//tacka 3.
		pcap_freealldevs(devices);
		break;	
	} while (true);



	getchar();
	getchar();
	return 0;
}

void packet_handler(unsigned char* param, const pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	//tacka 6.
	ethernet_header* eh = (ethernet_header*)packet_data;
	printf("\n\tFizicka adresa:\t%.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->dest_address[0], eh->dest_address[1], eh->dest_address[2], eh->dest_address[3], eh->dest_address[4], eh->dest_address[5]);
	if (ntohs(eh->type) == 0x0800)
	{
		//proverava da li je posle etherneta ipv4 
	}
	if (ntohs(eh->type) == 0x86dd)
	{
		//proverava da li je posle etherneta ipv6 
	}
	if (ntohs(eh->type) == 0x0806)
	{
		//proverava da li je posle etherneta ARP 
	}

	ip_header* ih = (ip_header*)(packet_data + sizeof(ethernet_header));
	int ip_len = ih->header_length * 4; // header length is calculated using words (1 word = 4 bytes)
	printf("\n\tLogicka adresa:\t\t%u.%u.%u.%u", ih->dst_addr[0], ih->dst_addr[1], ih->dst_addr[2], ih->dst_addr[3]);

	
	//tacka 7.
	if (ih->next_protocol == 17) //provravamo da li je sledeci protokol udp
	{
		udp_header* uh = (udp_header*)((unsigned char*)ih + ip_len);
		printf("Port primaoca: %d \n", ntohs(uh->dest_port));
		printf("Port posiljaoca: %d \n", ntohs(uh->src_port));

		//tacka 8.
		if (eh->src_address[0] == 0x88 && eh->src_address[1] == 0x88 && eh->src_address[2] == 0x88 && eh->src_address[3] == 0x88 && eh->src_address[4] == 0x87 && eh->src_address[5] == 0x88)
		{
			udp_counter++;
			printf("Udp counter = %d \n", udp_counter);
		}
			

	}
	else if (ih->next_protocol == 6) //proveravamo da li je tcp protokol
	{
		//tacka 9.
		tcp_header* th = (tcp_header*)((unsigned char*)ih + ip_len);

		printf("Windows size: %d , seq num: %d", ntohs(th->windows_size), ntohl(th->sequence_num));

		if (th->dest_port)//pristgli tls paket
		{
			//11.tacka
			unsigned char* app_data = (unsigned char*)th + th->header_length * 4;
			int app_len = packet_header->len - (app_data - packet_data);
			
			for (int i = 0; i < app_len;i++)
			{
				printf("%x ", app_data[i]);
			}

		}
		else if (th->src_port)//poslati tls paket
		{
			//11.tacka
			unsigned char* app_data = (unsigned char*)th + th->header_length * 4;
			int app_len = packet_header->len - (app_data - packet_data);
			for (int i = 0; i < app_len; i++)
			{
				printf("%x ", app_data[i]);
			}
		}
	}

	printf("----------------------------------------------------\n");
}

void print_raw_data(unsigned char* data, int data_length)
{
	printf("\n-------------------------------------------------------------\n\t");
	for (int i = 0; i < data_length; i = i + 1)
	{
		printf("%.2x ", ((unsigned char*)data)[i]);

		// 16 bytes per line
		if ((i + 1) % 16 == 0)
			printf("\n\t");
	}
	printf("\n-------------------------------------------------------------");
}

void print_winpcap_header(const struct pcap_pkthdr* packet_header, int packet_counter)
{
	printf("\n\n=============================================================");
	printf("\n\tWINPCAP PSEUDO LAYER");
	printf("\n-------------------------------------------------------------");

	time_t timestamp;			// Raw time (bits) when packet is received 
	struct tm* local_time;		// Local time when packet is received
	char time_string[16];		// Local time converted to string

	// Convert the timestamp to readable format
	timestamp = packet_header->ts.tv_sec;
	local_time = localtime(&timestamp);
	strftime(time_string, sizeof time_string, "%H:%M:%S", local_time);

	// Print timestamp and length of the packet
	printf("\n\tPacket number:\t\t%u", packet_counter);
	printf("\n\tTimestamp:\t\t%s.", time_string);
	printf("\n\tPacket length:\t\t%u ", packet_header->len);
	printf("\n=============================================================");
	return;
}

void print_ethernet_header(ethernet_header* eh)
{
	printf("\n=============================================================");
	printf("\n\tDATA LINK LAYER  -  Ethernet");

	print_raw_data((unsigned char*)eh, sizeof(ethernet_header));

	printf("\n\tDestination address:\t%.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->dest_address[0], eh->dest_address[1], eh->dest_address[2], eh->dest_address[3], eh->dest_address[4], eh->dest_address[5]);
	printf("\n\tSource address:\t\t%.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->src_address[0], eh->src_address[1], eh->src_address[2], eh->src_address[3], eh->src_address[4], eh->src_address[5]);
	printf("\n\tNext protocol:\t\t0x%.4x", ntohs(eh->type));

	printf("\n=============================================================");

	return;
}

void print_ip_header(ip_header* ih)
{
	printf("\n=============================================================");
	printf("\n\tNETWORK LAYER  -  Internet Protocol (IP)");

	print_raw_data((unsigned char*)ih, ih->header_length * 4);

	printf("\n\tVersion:\t\t%u", ih->version);
	printf("\n\tHeader Length:\t\t%u", ih->header_length * 4);
	printf("\n\tType of Service:\t%u", ih->tos);
	printf("\n\tTotal length:\t\t%u", ntohs(ih->length));
	printf("\n\tIdentification:\t\t%u", ntohs(ih->identification));
	printf("\n\tFragments:\t\t%u", ntohs(ih->fragm_fo));
	printf("\n\tTime-To-Live:\t\t%u", ih->ttl);
	printf("\n\tNext protocol:\t\t%u", ih->next_protocol);
	printf("\n\tHeader checkSum:\t%u", ntohs(ih->checksum));
	printf("\n\tSource:\t\t\t%u.%u.%u.%u", ih->src_addr[0], ih->src_addr[1], ih->src_addr[2], ih->src_addr[3]);
	printf("\n\tDestination:\t\t%u.%u.%u.%u", ih->dst_addr[0], ih->dst_addr[1], ih->dst_addr[2], ih->dst_addr[3]);

	printf("\n=============================================================");

	return;
}

void print_udp_header(udp_header* uh)
{
	printf("\n=============================================================");
	printf("\n\tTRANSPORT LAYER  -  User Datagram Protocol (UDP)");

	print_raw_data((unsigned char*)uh, sizeof(udp_header));

	printf("\n\tSource Port:\t\t%u", ntohs(uh->src_port));
	printf("\n\tDestination Port:\t%u", ntohs(uh->dest_port));
	printf("\n\tDatagram Length:\t%u", ntohs(uh->datagram_length));
	printf("\n\tChecksum:\t\t%u", ntohs(uh->checksum));

	printf("\n=============================================================");

	return;
}

void print_application_data(unsigned char* data, long data_length)
{
	printf("\n=============================================================");
	printf("\n\tAPPLICATION LAYER");

	print_raw_data(data, data_length);

	printf("\n=============================================================");
}
