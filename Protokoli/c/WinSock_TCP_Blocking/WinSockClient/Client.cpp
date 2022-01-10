#define WIN32_LEAN_AND_MEAN
#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <conio.h>

#include "../Common/NetworkOperations.hpp"
#include "../Common/Liste.hpp"

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT 27016
#define SERVER_ADDRESS "127.0.0.1"

// Initializes WinSock2 library
// Returns true if succeeded, false otherwise.
bool InitializeWindowsSockets();

int __cdecl main(int argc, char **argv) 
{

	CVOR* glava;
	InitList(&glava);
	Knjiga k1 = NapraviNovuKnjigu("Knjiga1", "autor1", 56, 32.5);
	Knjiga k2 = NapraviNovuKnjigu("Knjiga2", "autor2", 534, 2354.6);
	Knjiga k3 = NapraviNovuKnjigu("Knjiga3", "autor3", 645, 4345.9);
	Knjiga k4 = NapraviNovuKnjigu("Knjiga4", "autor4", 346, 5345.6);
	Knjiga k5 = NapraviNovuKnjigu("Knjiga5", "autor5", 8678, 4765.9);
	Knjiga k6 = NapraviNovuKnjigu("Knjiga6", "autor6", 756, 8678.9);

	Push(&glava, k1);
	Push(&glava, k2);
	Push(&glava, k3);
	Push(&glava, k4);
	Push(&glava, k5);
	Push(&glava, k6);

	PrintList(glava);

    // socket used to communicate with server
    SOCKET connectSocket = INVALID_SOCKET;
    // variable used to store function return value
    int iResult;
    // message to send
    char *messageToSend = "this is a test";
 
    if(InitializeWindowsSockets() == false)
    {
		// we won't log anything since it will be logged
		// by InitializeWindowsSockets() function
		return 1;
    }

    // create a socket
    connectSocket = socket(AF_INET,
                           SOCK_STREAM,
                           IPPROTO_TCP);

    if (connectSocket == INVALID_SOCKET)
    {
        printf("socket failed with error: %ld\n", WSAGetLastError());
        WSACleanup();
        return 1;
    }

    // create and initialize address structure
    sockaddr_in serverAddress;
    serverAddress.sin_family = AF_INET;
    serverAddress.sin_addr.s_addr = inet_addr(SERVER_ADDRESS);
    serverAddress.sin_port = htons(DEFAULT_PORT);
    // connect to server specified in serverAddress and socket connectSocket
    if (connect(connectSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR)
    {
        printf("Unable to connect to server.\n");
        closesocket(connectSocket);
        WSACleanup();
    }

	char* buffer = Serialize(glava);
	int len = CalculateLen(glava);

	iResult = SendPacket(connectSocket, (char*)(&len), 4);

	if (iResult == -1)
	{
		printf("Doslo je do greske prilikom slanja duzine.\n");
		return 0;
	}
	iResult = SendPacket(connectSocket, buffer, len);
	if (iResult == -1)
	{
		printf("Doslo je do greske prilikom slanja liste.\n");
		
	}

    printf("Bytes Sent: %ld\n", iResult);
    // cleanup
    closesocket(connectSocket);
    WSACleanup();

	int a = getchar();

    return 0;
}

bool InitializeWindowsSockets()
{
    WSADATA wsaData;
	// Initialize windows sockets library for this process
    if (WSAStartup(MAKEWORD(2,2), &wsaData) != 0)
    {
        printf("WSAStartup failed with error: %d\n", WSAGetLastError());
        return false;
    }
	return true;
}
