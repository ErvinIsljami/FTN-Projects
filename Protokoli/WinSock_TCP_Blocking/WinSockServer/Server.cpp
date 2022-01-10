#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include "List.hpp"

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27016"
bool InitializeWindowsSockets();

typedef struct params_st
{
    SOCKET* acptdSocket;
    CVOR** queue;
}PARAMS;

DWORD WINAPI handleSocket(LPVOID lpParam);

int  main(void) 
{
    // Socket used for listening for new clients 
    SOCKET listenSocket = INVALID_SOCKET;
    // Socket used for communication with client
    SOCKET acceptedSocket[10];
    for (int i = 0; i < 10; i++)
    {
        acceptedSocket[i] = INVALID_SOCKET;
    }

    // variable used to store function return value
    int iResult;
    // Buffer used for storing incoming data
    char recvbuf[DEFAULT_BUFLEN];
    
    if(InitializeWindowsSockets() == false)
    {
		// we won't log anything since it will be logged
		// by InitializeWindowsSockets() function
		return 1;
    }
    
    // Prepare address information structures
    addrinfo *resultingAddress = NULL;
    addrinfo hints;

    memset(&hints, 0, sizeof(hints));
    hints.ai_family = AF_INET;       // IPv4 address
    hints.ai_socktype = SOCK_STREAM; // Provide reliable data streaming
    hints.ai_protocol = IPPROTO_TCP; // Use TCP protocol
    hints.ai_flags = AI_PASSIVE;     // 

    // Resolve the server address and port
    iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &resultingAddress);
    if ( iResult != 0 )
    {
        printf("getaddrinfo failed with error: %d\n", iResult);
        WSACleanup();
        return 1;
    }

    // Create a SOCKET for connecting to server
    listenSocket = socket(AF_INET,      // IPv4 address famly
                          SOCK_STREAM,  // stream socket
                          IPPROTO_TCP); // TCP

    if (listenSocket == INVALID_SOCKET)
    {
        printf("socket failed with error: %ld\n", WSAGetLastError());
        freeaddrinfo(resultingAddress);
        WSACleanup();
        return 1;
    }

    // Setup the TCP listening socket - bind port number and local address 
    // to socket
    iResult = bind( listenSocket, resultingAddress->ai_addr, (int)resultingAddress->ai_addrlen);
    if (iResult == SOCKET_ERROR)
    {
        printf("bind failed with error: %d\n", WSAGetLastError());
        freeaddrinfo(resultingAddress);
        closesocket(listenSocket);
        WSACleanup();
        return 1;
    }

    // Since we don't need resultingAddress any more, free it
    freeaddrinfo(resultingAddress);

    // Set listenSocket in listening mode
    iResult = listen(listenSocket, SOMAXCONN);
    if (iResult == SOCKET_ERROR)
    {
        printf("listen failed with error: %d\n", WSAGetLastError());
        closesocket(listenSocket);
        WSACleanup();
        return 1;
    }

	printf("Server initialized, waiting for clients.\n");
    int numberOfClients = 0;
    CVOR* queue;
    Init(&queue);
    Enqueue(&queue, 1);

    do
    {
        acceptedSocket[numberOfClients] = accept(listenSocket, NULL, NULL);

        if (acceptedSocket[numberOfClients] == INVALID_SOCKET)
        {
            printf("accept failed with error: %d\n", WSAGetLastError());
            closesocket(listenSocket);
            WSACleanup();
            return 1;
        }
        
        //pokreni nit za svakog clienta

        DWORD funId;
        HANDLE handle;

        PARAMS params;
        params.acptdSocket = &acceptedSocket[numberOfClients];
        params.queue = &queue;

        CreateThread(NULL, 0, &handleSocket, &params, 0, &funId);

        numberOfClients++;
    } while (1);

    closesocket(listenSocket);

    for (int i = 0; i < 10; i++)
    {
        iResult = shutdown(acceptedSocket[i], SD_SEND);
        if (iResult == SOCKET_ERROR)
        {
            printf("shutdown failed with error: %d\n", WSAGetLastError());
            closesocket(acceptedSocket[i]);
            WSACleanup();
            return 1;
        }

        closesocket(acceptedSocket[i]);
    }
    WSACleanup();

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

DWORD WINAPI handleSocket(LPVOID lpParam)
{
    PARAMS* params = (PARAMS*)lpParam;
    SOCKET acceptedSocket = *params->acptdSocket;
    CVOR* queue = *params->queue;
    int iResult;
    char recvbuf[512];

    //unsigned long mode = 1; //non-blocking mode
    //iResult = ioctlsocket(acceptedSocket, FIONBIO, &mode);
    //if (iResult != NO_ERROR)
    //    printf("ioctlsocket failed with error: %ld\n", iResult);

    do
    {
        // Receive data until the client shuts down the connection
        iResult = recv(acceptedSocket, recvbuf, DEFAULT_BUFLEN, 0);
        if (iResult > 0)
        {
            if (recvbuf[0] == 0)    //PUSH
            {
                int number = *(recvbuf + 4);
                Enqueue(&queue, number);
            }
            else if (recvbuf[0] == 1)   //POP
            {
                int number = Dequeue(&queue);
                sprintf_s(recvbuf, "Podatak sa queue-a je: %d\n", number);
                iResult = send(acceptedSocket, recvbuf, strlen(recvbuf) + 1, 0);

                if (iResult == SOCKET_ERROR)
                {
                    printf("send failed with error: %d\n", WSAGetLastError());
                    closesocket(acceptedSocket);
                    WSACleanup();
                    return 1;
                }
            }
        }
        else if (iResult == 0)
        {
            // connection was closed gracefully
            printf("Connection with client closed.\n");
            closesocket(acceptedSocket);
        }
        else
        {
            // there was an error during recv
            printf("recv failed with error: %d\n", WSAGetLastError());
            closesocket(acceptedSocket);
        }
    } while (true);


    return 0;
}