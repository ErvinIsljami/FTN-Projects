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

#define SERVER_PORT 27016
#define BUFFER_SIZE 256

// TCP server that use blocking sockets
int main()
{
	// Socket used for listening for new clients 
	SOCKET listenSocket = INVALID_SOCKET;

	// Socket used for communication with client
	SOCKET acceptedSocket = INVALID_SOCKET;
	SOCKET acceptedSocket2 = INVALID_SOCKET;

	// Variable used to store function return value
	int iResult;

	// Buffer used for storing incoming data
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


	// Initialize serverAddress structure used by bind
	sockaddr_in serverAddress;
	memset((char*)& serverAddress, 0, sizeof(serverAddress));
	serverAddress.sin_family = AF_INET;				// IPv4 address family
	serverAddress.sin_addr.s_addr = INADDR_ANY;		// Use all available addresses
	serverAddress.sin_port = htons(SERVER_PORT);	// Use specific port


	// Create a SOCKET for connecting to server
	listenSocket = socket(AF_INET,      // IPv4 address family
		SOCK_STREAM,  // Stream socket
		IPPROTO_TCP); // TCP protocol

// Check if socket is successfully created
	if (listenSocket == INVALID_SOCKET)
	{
		printf("socket failed with error: %ld\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	// Setup the TCP listening socket - bind port number and local address to socket
	iResult = bind(listenSocket, (struct sockaddr*) & serverAddress, sizeof(serverAddress));

	// Check if socket is successfully binded to address and port from sockaddr_in structure
	if (iResult == SOCKET_ERROR)
	{
		printf("bind failed with error: %d\n", WSAGetLastError());
		closesocket(listenSocket);
		WSACleanup();
		return 1;
	}

	// Set listenSocket in listening mode
	iResult = listen(listenSocket, SOMAXCONN);
	if (iResult == SOCKET_ERROR)
	{
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(listenSocket);
		WSACleanup();
		return 1;
	}

	printf("Server socket is set to listening mode. Waiting for new connection requests.\n");

	do
	{
		// Struct for information about connected client
		sockaddr_in clientAddr;

		int clientAddrSize = sizeof(struct sockaddr_in);

		// Accept new connections from clients 
		acceptedSocket = accept(listenSocket, (struct sockaddr*) & clientAddr, &clientAddrSize);

		// Check if accepted socket is valid 
		if (acceptedSocket == INVALID_SOCKET)
		{
			printf("accept failed with error: %d\n", WSAGetLastError());
			closesocket(listenSocket);
			WSACleanup();
			return 1;
		}

		printf("\nNew client request accepted. Client address: %s : %d\n", inet_ntoa(clientAddr.sin_addr), ntohs(clientAddr.sin_port));

		//tacka 1.
		// Struct for information about connected client
		sockaddr_in clientAddr2;

		int clientAddrSize2 = sizeof(struct sockaddr_in);

		// Accept new connections from clients 
		acceptedSocket2 = accept(listenSocket, (struct sockaddr*) & clientAddr2, &clientAddrSize2);

		// Check if accepted socket is valid 
		if (acceptedSocket2 == INVALID_SOCKET)
		{
			printf("accept failed with error: %d\n", WSAGetLastError());
			closesocket(listenSocket);
			WSACleanup();
			return 1;
		}

		printf("\nNew client2 request accepted. Client address: %s : %d\n", inet_ntoa(clientAddr2.sin_addr), ntohs(clientAddr2.sin_port));


		//tacka 2
		char pitanje[50];
		char odgovor[30];
		puts("Unesite pitanje: ");
		gets_s(pitanje, 50);
		puts("Unesite odgovor: ");
		gets_s(pitanje, 30);


		iResult = send(acceptedSocket, pitanje, (int)strlen(pitanje), 0);

		// Check result of send function
		if (iResult == SOCKET_ERROR)
		{
			printf("send failed with error: %d\n", WSAGetLastError());
			closesocket(acceptedSocket);
			WSACleanup();
			return 1;
		}

		iResult = send(acceptedSocket2, pitanje, (int)strlen(pitanje), 0);

		// Check result of send function
		if (iResult == SOCKET_ERROR)
		{
			printf("send failed with error: %d\n", WSAGetLastError());
			closesocket(acceptedSocket2);
			WSACleanup();
			return 1;
		}


		unsigned long mode = 1; //non-blocking mode
		iResult = ioctlsocket(acceptedSocket, FIONBIO, &mode);
		if (iResult != NO_ERROR)
			printf("ioctlsocket failed with error: %ld\n", iResult);

		mode = 1; //non-blocking mode
		iResult = ioctlsocket(acceptedSocket2, FIONBIO, &mode);
		if (iResult != NO_ERROR)
			printf("ioctlsocket failed with error: %ld\n", iResult);




		do
		{
			//tacka 4.
			iResult = recv(acceptedSocket, dataBuffer, BUFFER_SIZE, 0);

			if (iResult != SOCKET_ERROR) 
			{
				dataBuffer[iResult] = '\0';

				printf("Client1 sent: %s.\n", dataBuffer);
			}
			else
			{
				if (WSAGetLastError() == WSAEWOULDBLOCK)
				{
					Sleep(1000);
				}
				else 
				{
					printf("recv failed with error: %d\n", WSAGetLastError());
					closesocket(acceptedSocket);
				}
			}


			iResult = recv(acceptedSocket2, dataBuffer2, BUFFER_SIZE, 0);

			if (iResult != SOCKET_ERROR)
			{
				dataBuffer2[iResult] = '\0';

				printf("Client1 sent: %s.\n", dataBuffer2);
			}
			else
			{
				if (WSAGetLastError() == WSAEWOULDBLOCK)
				{
					Sleep(1000);
				}
				else
				{
					printf("recv failed with error: %d\n", WSAGetLastError());
					closesocket(acceptedSocket2);
				}
			}



		} while (true);		//obavezno promeniti na true

		// Here is where server shutdown loguc could be placed

	} while (true);

	// Shutdown the connection since we're done
	iResult = shutdown(acceptedSocket, SD_BOTH);

	// Check if connection is succesfully shut down.
	if (iResult == SOCKET_ERROR)
	{
		printf("shutdown failed with error: %d\n", WSAGetLastError());
		closesocket(acceptedSocket);
		WSACleanup();
		return 1;
	}

	//Close listen and accepted sockets
	closesocket(listenSocket);
	closesocket(acceptedSocket);

	// Deinitialize WSA library
	WSACleanup();

	return 0;
}