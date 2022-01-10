// Elektroenergetski softverski inzenjering
// Primenjene racunarske mreze u namenskim sistemima 2
// Vezba 6 - Interpretacija sadrzaja paketa (2.deo) -resenje gde se hvataju paketi sa callback funkcijom

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

int packet_counter = 0;				// counts packets in oreder to have numerated packets

// Function declarations
void packet_handler(unsigned char *param, const struct pcap_pkthdr *packet_header, const unsigned char *packet_data);

int main()
{
	pcap_if_t* devices;
	pcap_if_t* device;
	pcap_t* device_handle;
	char error_buffer[PCAP_ERRBUF_SIZE];
	
    /* Retrieve the device list on the local machine */
    if (pcap_findalldevs(&devices, error_buffer) == -1)
	{
		printf("Error in pcap_findalldevs: %s\n", error_buffer);
		return -1;
	}

	int i=0;			// Count devices and provide jumping to the selected device 

    // Print the list
    for(device=devices; device; device=device->next)
    {
        printf("%d. %s", ++i, device->name);
        if (device->description)
            printf(" (%s)\n", device->description);
        else
            printf(" (No description available)\n");
    }
    
	// Check if list is empty
    if (i==0)
    {
        printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
        return NULL;
    }
    
	// Pick one device from the list
	int device_number;
    printf("Enter the interface number (1-%d):",i);
    scanf_s("%d", &device_number);
    
    if(device_number < 1 || device_number > i)
    {
        printf("\nInterface number out of range.\n");
        return NULL;
    }
    
	// Select first device...
	device=devices;

    // ...and then jump to chosen devices
    for (i=1; i<device_number; i++)
	{
		device=device->next;
	}

	printf("\nChosen adapter is:  %s\n", device->description);

    // Open the capture device
    if ((device_handle = pcap_open_live( device->name,		// name of the device
                              65536,						// portion of the packet to capture (65536 guarantees that the whole packet will be captured on all the link layers)
                              0,							// non-promiscuous (normal) mode
                              1000,							// read timeout
							  error_buffer					// buffer where error message is stored
							) ) == NULL)
    {
        printf("\nUnable to open the adapter. %s is not supported by WinPcap\n", device->name);
        pcap_freealldevs(devices);
        return -1;
    }

	// Check the link layer. We support only Ethernet for simplicity.
	if(pcap_datalink(device_handle) != DLT_EN10MB)
	{
		printf("\nThis program works only on Ethernet networks.\n");
		pcap_freealldevs(devices);
		return -1;
	}

	unsigned int netmask;
	char filter_exp[] = "ether src host BC-5F-F4-B7-57-84 and ip and (udp or tcp)";  //find out your own MAC address using ipconfig
	struct bpf_program fcode;

	if (device->addresses != NULL)
		// Retrieve the mask of the first address of the interface 
		netmask=((struct sockaddr_in *)(device->addresses->netmask))->sin_addr.s_addr;
	else
		// If the interface is without an address, we suppose to be in a C class network 
		netmask=0xffffff;

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

    
    // At this point, we don't need any more the device list. Free it
    pcap_freealldevs(devices);

	// Start capturing packets
	pcap_loop(device_handle, 10, packet_handler, NULL);
	
	printf("\nPress any key to close application...");
	getchar();
	
	return 0;
}



// Callback function invoked by WinPcap for every incoming packet
void packet_handler(unsigned char* param, const struct pcap_pkthdr* packet_header, const unsigned char* packet_data)
{
	printf("\n\t Packet No.  \t%d", ++packet_counter);

	/* DATA LINK LAYER - Ethernet */

	// Retrive the position of the ethernet header
	ethernet_header * eh = (ethernet_header *)packet_data;

	printf("\nEthernet\n\tDestination MAC address:\t%.2x:%.2x:%.2x:%.2x:%.2x:%.2x", eh->dest_address[0], eh->dest_address[1], eh->dest_address[2], eh->dest_address[3], eh->dest_address[4], eh->dest_address[5]);

	/* NETWORK LAYER - IPv4 */

	// Retrieve the position of the ip header
	ip_header* ih = (ip_header*) (packet_data + sizeof(ethernet_header));
		
	printf("\nIP\n\t Source IP address:\t\t%u.%u.%u.%u", ih->src_addr[0], ih->src_addr[1], ih->src_addr[2], ih->src_addr[3]);
		
	/* TRANSPORT LAYER */

	unsigned char * app_data;
	int app_length;

	/* UDP */
	if (ih->next_protocol == 17)
	{
		udp_header* uh = (udp_header*) ((unsigned char*)ih + ih->header_length * 4);

		printf("\nUDP\n\tDestination Port:\t\t%u", ntohs(uh->dest_port));

		// Retrieve the position of application data
		app_data = (unsigned char *)uh + sizeof(udp_header);

		// Total length of application header and data
		app_length = ntohs(uh->datagram_length) - sizeof(udp_header);
	}

	/* TCP */
	else if (ih->next_protocol == 6)
	{
		tcp_header* th = (tcp_header*) ((unsigned char*)ih + ih->header_length * 4);

		printf("\nTCP\n\tDestination Port:\t\t%u", ntohs(th->dest_port));

		// Retrieve the position of application data
		app_data = (unsigned char *)th + th->header_length * 4;

		// Total length of application header and data
		app_length = packet_header->len - (sizeof(ethernet_header) + ih->header_length * 4 + th->header_length * 4);
	}
	else
	{
		return;
	}
		
	/* APPLICATION LAYER */

	// Print application header and data
	printf("\n-------------------------------------------------------------\n\t");
	for(int i = 0; i < app_length; i=i+1)
	{
		printf("%.2x ", (app_data)[i]);

		// 16 bytes per line
		if ((i+1) % 16 == 0)
			printf("\n\t");
	}
	printf("\n-------------------------------------------------------------");

	// For demonstration purpose
	printf("\n\nPress enter to receive new packet\n");
	getchar();
    
}