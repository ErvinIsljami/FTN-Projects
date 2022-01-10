#define _WINSOCK_DEPRECATED_NO_WARNINGS

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include "conio.h"
#include "NetworkOperations.h"

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

#define SERVER_IP_ADDRESS "127.0.0.1"
#define SERVER_PORT 27016
#define BUFFER_SIZE 256

// TCP client that use blocking sockets
int main()
{
	SOCKET connectSocket = INVALID_SOCKET;
	int iResult;
	char dataBuffer[BUFFER_SIZE];
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		printf("WSAStartup failed with error: %d\n", WSAGetLastError());
		return 1;
	}
	connectSocket = socket(AF_INET,
		SOCK_STREAM,
		IPPROTO_TCP);
	if (connectSocket == INVALID_SOCKET)
	{
		printf("socket failed with error: %ld\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}
	sockaddr_in serverAddress;
	serverAddress.sin_family = AF_INET;								// IPv4 protocol
	serverAddress.sin_addr.s_addr = inet_addr(SERVER_IP_ADDRESS);	// ip address of server
	serverAddress.sin_port = htons(SERVER_PORT);					// server port

	if (connect(connectSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR)
	{
		printf("Unable to connect to server.\n");
		closesocket(connectSocket);
		WSACleanup();
		return 1;
	}
	/*
	///Slanje poruke, ovo nam ne treba
	// Read string from user into outgoing buffer
	printf("Enter message to send: ");
	gets_s(dataBuffer, BUFFER_SIZE);

	iResult = send(connectSocket, dataBuffer, (int)strlen(dataBuffer), 0);

	// Check result of send function
	if (iResult == SOCKET_ERROR)
	{
		printf("send failed with error: %d\n", WSAGetLastError());
		closesocket(connectSocket);
		WSACleanup();
		return 1;
	}

	printf("Message successfully sent. Total bytes: %ld\n", iResult);
	*/

	//slanje poruke
	//napraviti listu, popuniti je
	//char* buffer = Serialize(glava);
	//umesto niza salje se buffer
	//velicina se dobija sa funkcijom CalculateLen(glava);
	//int len = CalculateLen(glava);

	int niz[10000];
	int len = 10000 * sizeof(int);
	for (int i = 0; i < 10000; i++)
	{
		niz[i] = i + 1;
	}
	iResult = SendPacket(connectSocket, (char*)(&len), 4);
	if (iResult == -1)
	{
		printf("Doslo je do greske prilikom slanja duzine.\n");
		return 0;
	}
	iResult = SendPacket(connectSocket, (char*)niz, len);
	if (iResult == -1)
	{
		printf("Doslo je do greske prilikom slanja niza.\n");
	}




	
	iResult = shutdown(connectSocket, SD_BOTH);

	// Check if connection is succesfully shut down.
	if (iResult == SOCKET_ERROR)
	{
		printf("Shutdown failed with error: %d\n", WSAGetLastError());
		closesocket(connectSocket);
		WSACleanup();
		return 1;
	}

	// For demonstration purpose
	printf("\nPress any key to exit: ");
	_getch();


	closesocket(connectSocket);
	WSACleanup();

	return 0;
}