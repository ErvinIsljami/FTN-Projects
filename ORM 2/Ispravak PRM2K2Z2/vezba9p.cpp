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
unsigned char* encrypt_data(const unsigned char* packet_data, unsigned char* app_data, int app_length);					// Returns a copy of packet with encrypted application data
unsigned short calculate_checksum(unsigned short* data, int data_length);																	// Finds specific key in array of keys and returns its index

#define ETHERNET_FRAME_MAX 1518		// Maximal length of ethernet frame

// Main function captures packets from the file
int main()
{
	//tacka 1.
	do
	{
		char buffer[512];
		char sifrovaniBuffer[512];
		int pomeraj = 5;

		printf("Unesite tekst koji zelite da sifrujete(Unesite velikim slovima)\n");
		gets_s(buffer, 512);

		int i;
		for (i = 0; i < strlen(buffer); i++)
		{
			sifrovaniBuffer[i] = buffer[i] + pomeraj;
			if (sifrovaniBuffer[i] > 'Z')
				sifrovaniBuffer[i] -= 26;
		}
		sifrovaniBuffer[i] = '\0';
		printf("Sifrovana poruka: %s\n", sifrovaniBuffer);

		//tacka 4.
		int len = sizeof(ethernet_header) + 21 + sizeof(udp_header) + strlen(sifrovaniBuffer);
		unsigned  char* packet_data = (unsigned char*)malloc(len);
		for (int i = 0; i < len; i++)
		{
			printf("%.2X ", packet_data[i]);
		}

		//tacka 5.
		ethernet_header* eh = (ethernet_header*)packet_data;
		/*eh->dest_address[0] = 0x00;
		eh->dest_address[1] = 0x09;
		eh->dest_address[2] = 0x3A;
		eh->dest_address[3] = 0x0E;
		eh->dest_address[4] = 0x6E;
		eh->dest_address[5] = 0xD4;
		eh->src_address[0] = 0x88;
		eh->src_address[1] = 0x87;
		eh->src_address[2] = 0x88;
		eh->src_address[3] = 0x88;
		eh->src_address[4] = 0x88;
		eh->src_address[5] = 0x88;*/
		eh->type = htons(0x0800);	//ova su obavezna

		ip_header* ih = (ip_header*)(packet_data + sizeof(ethernet_header));
		/*ih->checksum = ntohs(0xbcded);
		ih->dst_addr[0] = 192;
		ih->dst_addr[1] = 168;
		ih->dst_addr[2] = 0;
		ih->dst_addr[3] = 104;
		ih->version = 4;
		ih->ttl = 59;
		ih->tos = 0;
		ih->identification = 0;
		ih->fragm_fo = 0;*/
		ih->next_protocol = 17;

		//tacka 7
		ih->header_length = ntohs(20);
		ih->length = 274;

		//tacka 8.
		ih->src_addr[0] = 192;
		ih->src_addr[1] = 168;
		ih->src_addr[2] = 0;
		ih->src_addr[3] = 104;

		//tacka 9
		ih->dst_addr[0] = 192;
		ih->dst_addr[1] = 168;
		ih->dst_addr[2] = 0;
		ih->dst_addr[3] = 255;

		udp_header* uh = (udp_header*)((unsigned char*)ih + 20);
		uh->src_port = ntohs(25000); //tacka 8
		uh->dest_port = ntohs(35000); //tacka 9
		//uh->checksum = ntohs(0x453a);
		
		//tacka 7
		uh->datagram_length = ntohs(254);

		 
		unsigned char* app_data = (unsigned char*)((unsigned char*)uh + sizeof(udp_header));


		//tacka 6.
		memcpy(app_data, sifrovaniBuffer, strlen(sifrovaniBuffer));




		//tacka 10.
		






		//tacka 3
		printf("Exit ? \n");
		char izbor[5];
		gets_s(izbor, 5);
		if (strcmp(izbor, "YES") == 0)
			break;
	} while (true);

	getchar();
	return 0;
}