#define _WINSOCK_DEPRECATED_NO_WARNINGS

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include "conio.h"
#include "Queue.h"

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

#define SERVER_PORT 27016
#define BUFFER_SIZE 256
#define MAX_CLIENTS 10

enum MESSAGE_TYPE { PUSH, POP };
typedef struct
{
    char message_type;
    int value;
}Message;

typedef struct
{
    SOCKET socket;
    queue_t* queue;
}Params;

DWORD WINAPI clientHandle(LPVOID params);


int main()
{
    // Socket used for listening for new clients 
    SOCKET listenSocket = INVALID_SOCKET;
    SOCKET acceptedSocket[MAX_CLIENTS]; /////////////////////

    // Variable used to store function return value
    int iResult;

    // Buffer used for storing incoming data
    char dataBuffer[BUFFER_SIZE];

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
    memset((char*)&serverAddress, 0, sizeof(serverAddress));
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

    int i = 0;  // brojac klijenata
    HANDLE threadovi[MAX_CLIENTS];
    DWORD threadsId[MAX_CLIENTS];
    queue_t* queue = NULL;
    queue = create_queue();

    do
    {
        // Struct for information about connected client
        sockaddr_in clientAddr;

        int clientAddrSize = sizeof(struct sockaddr_in);

        // Accept new connections from clients 
        acceptedSocket[i] = accept(listenSocket, (struct sockaddr*) & clientAddr, &clientAddrSize);

        // Check if accepted socket is valid 
        if (acceptedSocket[i] == INVALID_SOCKET)
        {
            printf("accept failed with error: %d\n", WSAGetLastError());
            closesocket(listenSocket);
            WSACleanup();
            return 1;
        }

        printf("\nNew client request accepted. Client address: %s : %d\n", inet_ntoa(clientAddr.sin_addr), ntohs(clientAddr.sin_port));

        //pokrecem thread za prihvacenu konekciju
        Params params;
        params.socket = acceptedSocket[i];
        params.queue = queue;
        threadovi[i] = CreateThread(NULL, 0, &clientHandle, &params, 0, &threadsId[i]);


        i++;

    } while (i < MAX_CLIENTS);


    printf("Press any key to shutdown server");
    _getch();

    for (int i = 0; i < MAX_CLIENTS; i++)
    {
        iResult = shutdown(acceptedSocket[i], SD_BOTH);

        // Check if connection is succesfully shut down.
        if (iResult == SOCKET_ERROR)
        {
            printf("shutdown failed with error: %d\n", WSAGetLastError());
            closesocket(acceptedSocket[i]);
            WSACleanup();
            return 1;
        }

        closesocket(acceptedSocket[i]);
    }
    // Shutdown the connection since we're done


    //Close listen and accepted sockets
    closesocket(listenSocket);


    // Deinitialize WSA library
    WSACleanup();

    return 0;
}

DWORD WINAPI clientHandle(LPVOID params)
{ // nit prima jedan parametar, ja sam napravila strukturu koja ima soket i queue, ovde te parametre inicijaliuzujem
    Params* pars = (Params*)params;

    SOCKET acceptedSocket = pars->socket;  // koji klijent se konektovao
    queue_t* queue = pars->queue;  // isti queue

    char dataBuffer[BUFFER_SIZE]; // u ovaj bafer smjestam poruke koje klijent posalje

    do
    {
        //trebalo bi neblokirajuci rezim
        int iResult = recv(acceptedSocket, dataBuffer, BUFFER_SIZE, 0);

        if (iResult > 0)	// Check if message is successfully received
        {
            dataBuffer[iResult] = '\0';

            Message* mes = (Message*)dataBuffer;

            if (mes->message_type == PUSH)
            {
                enqueue(queue, (mes->value));
            }
            else if (mes->message_type == POP)
            {
                int popedValue = Dequeue(queue);

                iResult = send(acceptedSocket, (const char*)&popedValue, sizeof(int), 0);

                // Check result of send function
                if (iResult == SOCKET_ERROR)
                {
                    printf("send failed with error: %d\n", WSAGetLastError());
                    closesocket(acceptedSocket);
                    WSACleanup();
                    return 1;
                }
            }
            else
            {
                printf("Error...\n");
            }


        }
        else if (iResult == 0)	// Check if shutdown command is received
        {
            // Connection was closed successfully
            printf("Connection with client closed.\n");
            closesocket(acceptedSocket);
        }
        else	// There was an error during recv
        {

            printf("recv failed with error: %d\n", WSAGetLastError());
            closesocket(acceptedSocket);
        }

    } while (true);

    return 0;
}
