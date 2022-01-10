// UDP server that use blocking sockets
#define _WINSOCK_DEPRECATED_NO_WARNINGS

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include "conio.h"

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

#define SERVER_PORT 15000	// Port number of server that will be used for communication with clients
#define SERVER_PORT2 15001	// Port number of server that will be used for communication with clients
#define BUFFER_SIZE 512		// Size of buffer that will be used for sending and receiving messages to clients

int main()
{
	// Server address
	sockaddr_in serverAddress;
	sockaddr_in serverAddress2;

	// Buffer we will use to send and receive clients' messages
	char dataBuffer[BUFFER_SIZE];
	char dataBuffer2[BUFFER_SIZE];

	// WSADATA data structure that is to receive details of the Windows Sockets implementation
	WSADATA wsaData;

	// Initialize windows sockets library for this process
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		printf("WSAStartup failed with error: %d\n", WSAGetLastError());
		return 1;
	}

	// Initialize serverAddress structure used by bind function
	memset((char*)& serverAddress, 0, sizeof(serverAddress));
	serverAddress.sin_family = AF_INET; 			// set server address protocol family
	serverAddress.sin_addr.s_addr = INADDR_ANY;		// use all available addresses of server
	serverAddress.sin_port = htons(SERVER_PORT);

	memset((char*)& serverAddress2, 0, sizeof(serverAddress2));
	serverAddress2.sin_family = AF_INET; 			// set server address protocol family
	serverAddress2.sin_addr.s_addr = inet_addr("127.0.0.1");		// use all available addresses of server
	serverAddress2.sin_port = htons(SERVER_PORT2);

	// Create a socket
	SOCKET serverSocket = socket(AF_INET,      // IPv4 address famly
		SOCK_DGRAM,   // datagram socket
		IPPROTO_UDP); // UDP

// Check if socket creation succeeded
	if (serverSocket == INVALID_SOCKET)
	{
		printf("Creating socket failed with error: %d\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	SOCKET serverSocket2 = socket(AF_INET,      // IPv4 address famly
		SOCK_DGRAM,   // datagram socket
		IPPROTO_UDP); // UDP

// Check if socket creation succeeded
	if (serverSocket2 == INVALID_SOCKET)
	{
		printf("Creating socket failed with error: %d\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	// Bind server address structure (type, port number and local address) to socket
	int iResult = bind(serverSocket, (SOCKADDR*)& serverAddress, sizeof(serverAddress));

	// Check if socket is succesfully binded to server datas
	if (iResult == SOCKET_ERROR)
	{
		printf("Socket bind failed with error: %d\n", WSAGetLastError());
		closesocket(serverSocket);
		WSACleanup();
		return 1;
	}

	iResult = bind(serverSocket2, (SOCKADDR*)& serverAddress2, sizeof(serverAddress2));

	// Check if socket is succesfully binded to server datas
	if (iResult == SOCKET_ERROR)
	{
		printf("Socket bind failed with error: %d\n", WSAGetLastError());
		closesocket(serverSocket2);
		WSACleanup();
		return 1;
	}

	printf("Simple UDP server started and waiting client messages.\n");

	unsigned long mode = 1; //non-blocking mode
	iResult = ioctlsocket(serverSocket, FIONBIO, &mode);
	if (iResult != NO_ERROR)
		printf("ioctlsocket failed with error: %ld\n", iResult);

	mode = 1; //non-blocking mode
	iResult = ioctlsocket(serverSocket2, FIONBIO, &mode);
	if (iResult != NO_ERROR)
		printf("ioctlsocket failed with error: %ld\n", iResult);


	timeval timeVal;
	timeVal.tv_sec = 1;
	timeVal.tv_usec = 0;

	// Main server loop
	while (1)
	{
		fd_set readfds;
		FD_ZERO(&readfds);
		FD_SET(serverSocket, &readfds);
		FD_SET(serverSocket2, &readfds);

		int result = select(0, &readfds, NULL, NULL, &timeVal);

		if (result == 0) 
		{
			// vreme za cekanje je isteklo
		}
		else if (result == SOCKET_ERROR) 
		{
			printf("select failed with error: %d\n", WSAGetLastError());
			break;
		}
		else 
		{
			// rezultat je jednak broju soketa koji su zadovoljili uslov
			if (FD_ISSET(serverSocket, &readfds)) 
			{
				// Declare and initialize client address that will be set from recvfrom
				sockaddr_in clientAddress;
				memset(&clientAddress, 0, sizeof(clientAddress));

				// Set whole buffer to zero
				memset(dataBuffer, 0, BUFFER_SIZE);

				// size of client address
				int sockAddrLen = sizeof(clientAddress);

				// Receive client message
				iResult = recvfrom(serverSocket,				// Own socket
					dataBuffer,					// Buffer that will be used for receiving message
					BUFFER_SIZE,					// Maximal size of buffer
					0,							// No flags
					(SOCKADDR*)& clientAddress,	// Client information from received message (ip address and port)
					&sockAddrLen);				// Size of sockadd_in structure

		// Check if message is succesfully received
				if (iResult == SOCKET_ERROR)
				{
					printf("recvfrom failed with error: %d\n", WSAGetLastError());
					continue;
				}

				// Set end of string
				dataBuffer[iResult] = '\0';

				char ipAddress[16]; // 15 spaces for decimal notation (for example: "192.168.100.200") + '\0'

				// Copy client ip to local char[]
				strcpy_s(ipAddress, sizeof(ipAddress), inet_ntoa(clientAddress.sin_addr));

				// Convert port number from network byte order to host byte order
				unsigned short clientPort = ntohs(clientAddress.sin_port);

				printf("Client connected from ip: %s, port: %d, sent: %s.\n", ipAddress, clientPort, dataBuffer);

				int n = strlen(dataBuffer);
				bool isPalindrom = true;
				for (int i = 0; i < n / 2; i++)
				{
					if (dataBuffer[i] != dataBuffer[n - i - 1])
						isPalindrom = false;
				}

				if (isPalindrom)
				{
					sprintf_s(dataBuffer, "String koji ste poslali JESTE palindrom.");
				}
				else
				{
					sprintf_s(dataBuffer, "String koji ste poslali NIJE palindrom.");
				}

				iResult = sendto(serverSocket,						// Own socket
					dataBuffer,						// Text of message
					strlen(dataBuffer),				// Message size
					0,									// No flags
					(SOCKADDR*)& clientAddress,		// Address structure of server (type, IP address and port)
					sizeof(clientAddress));			// Size of sockadr_in structure

			// Check if message is succesfully sent. If not, close client application
				if (iResult == SOCKET_ERROR)
				{
					printf("sendto failed with error: %d\n", WSAGetLastError());
					closesocket(serverSocket);
					WSACleanup();
					return 1;
				}
			}

			if (FD_ISSET(serverSocket2, &readfds))
			{
				// Declare and initialize client address that will be set from recvfrom
				sockaddr_in clientAddress2;
				memset(&clientAddress2, 0, sizeof(clientAddress2));

				// Set whole buffer to zero
				memset(dataBuffer2, 0, BUFFER_SIZE);

				// size of client address
				int sockAddrLen2 = sizeof(clientAddress2);

				// Receive client message
				iResult = recvfrom(serverSocket2,				// Own socket
					dataBuffer2,					// Buffer that will be used for receiving message
					BUFFER_SIZE,					// Maximal size of buffer
					0,							// No flags
					(SOCKADDR*)& clientAddress2,	// Client information from received message (ip address and port)
					&sockAddrLen2);				// Size of sockadd_in structure

		// Check if message is succesfully received
				if (iResult == SOCKET_ERROR)
				{
					printf("recvfrom failed with error: %d\n", WSAGetLastError());
					continue;
				}

				// Set end of string
				dataBuffer2[iResult] = '\0';

				char ipAddress2[16]; // 15 spaces for decimal notation (for example: "192.168.100.200") + '\0'

				// Copy client ip to local char[]
				strcpy_s(ipAddress2, sizeof(ipAddress2), inet_ntoa(clientAddress2.sin_addr));

				// Convert port number from network byte order to host byte order
				unsigned short clientPort2 = ntohs(clientAddress2.sin_port);

				printf("Client connected from ip: %s, port: %d, sent: %s.\n", ipAddress2, clientPort2, dataBuffer2);

				int n = strlen(dataBuffer2);
				bool isPalindrom2 = true;
				for (int i = 0; i < n / 2; i++)
				{
					if (dataBuffer2[i] != dataBuffer2[n - i - 1])
						isPalindrom2 = false;
				}

				if (isPalindrom2)
				{
					sprintf_s(dataBuffer2, "String koji ste poslali JESTE palindrom.");
				}
				else
				{
					sprintf_s(dataBuffer2, "String koji ste poslali NIJE palindrom.");
				}

				iResult = sendto(serverSocket2,						// Own socket
					dataBuffer2,						// Text of message
					strlen(dataBuffer2),				// Message size
					0,									// No flags
					(SOCKADDR*)& clientAddress2,		// Address structure of server (type, IP address and port)
					sizeof(clientAddress2));			// Size of sockadr_in structure

			// Check if message is succesfully sent. If not, close client application
				if (iResult == SOCKET_ERROR)
				{
					printf("sendto failed with error: %d\n", WSAGetLastError());
					closesocket(serverSocket2);
					WSACleanup();
					return 1;
				}
			}
		}
		FD_CLR(serverSocket, &readfds);
		FD_CLR(serverSocket2, &readfds);
	}

	// Close server application
	iResult = closesocket(serverSocket);
	if (iResult == SOCKET_ERROR)
	{
		printf("closesocket failed with error: %ld\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	printf("Server successfully shut down.\n");

	// Close Winsock library
	WSACleanup();

	return 0;
}