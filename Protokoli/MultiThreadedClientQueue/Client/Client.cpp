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
#pragma warning (disable: 4996)

#define SERVER_IP_ADDRESS "127.0.0.1"
#define SERVER_PORT 27016
#define BUFFER_SIZE 256

enum MESSAGE_TYPE { PUSH, POP };
typedef struct
{
    char message_type;
    int value;
}Message;

// TCP client that use blocking sockets
int main()
{
    // Socket used to communicate with server
    SOCKET connectSocket = INVALID_SOCKET;

    // Variable used to store function return value
    int iResult;

    // Buffer we will use to store message
    char dataBuffer[BUFFER_SIZE];

    // WSADATA data structure that is to receive details of the Windows Sockets implementation
    WSADATA wsaData;

    // Initialize windows sockets library for this process
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
    {
        printf("WSAStartup failed with error: %d\n", WSAGetLastError());
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

    // Create and initialize address structure
    sockaddr_in serverAddress;
    serverAddress.sin_family = AF_INET;								// IPv4 protocol
    serverAddress.sin_addr.s_addr = inet_addr(SERVER_IP_ADDRESS);	// ip address of server
    serverAddress.sin_port = htons(SERVER_PORT);					// server port

    // Connect to server specified in serverAddress and socket connectSocket
    if (connect(connectSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR)
    {
        printf("Unable to connect to server.\n");
        closesocket(connectSocket);
        WSACleanup();
        return 1;
    }

    do
    {

        printf("Izaberite opciju: \n");
        printf("1. Push.\n");
        printf("2. Pop.\n");
        int izbor = -1;
        scanf_s("%d", &izbor);
        getc(stdin);//da uzme enter iz buffer-a posle scanf-a

        Message msg;
        if (izbor == 1)
        {
            msg.message_type = PUSH;
            printf("Unesite broj koji zelite da pushujete.\n");
            scanf_s("%d", &msg.value);    //unesem value koji hocu da pushujem
            getc(stdin);
        }
        else if (izbor == 2)
        {
            msg.message_type = POP;
        }
        else
        {
            system("cls");
            printf("Niste uneli dobar izbor. Unesite ponovo.\n");
            continue;
        }

        iResult = send(connectSocket, (const char*)(&msg), sizeof(Message), 0);

        // Check result of send function
        if (iResult == SOCKET_ERROR)
        {
            printf("send failed with error: %d\n", WSAGetLastError());
            closesocket(connectSocket);
            WSACleanup();
            return 1;
        }

        unsigned long mode = 1; //non-blocking mode
        iResult = ioctlsocket(connectSocket, FIONBIO, &mode);
        if (iResult != NO_ERROR)
            printf("ioctlsocket failed with error: %ld\n", iResult);

        fd_set readfds;
        FD_ZERO(&readfds);
        // maksimalni period cekanja select funkcije
        FD_SET(connectSocket, &readfds);

        timeval timeVal;
        timeVal.tv_sec = 1;
        timeVal.tv_usec = 0;

        int result = select(0, &readfds, NULL, NULL, &timeVal);

        if (result == 0)
        {
            // vreme za cekanje je isteklo
        }
        else if (result == SOCKET_ERROR)
        {
            //desila se greska prilikom poziva funkcije
        }
        else
        {
            if (FD_ISSET(connectSocket, &readfds))
            {
                //nakon poslate poruke prikazemo rezultat
                iResult = recv(connectSocket, dataBuffer, BUFFER_SIZE, 0);

                if (iResult > 0)	// Check if message is successfully received
                {
                    dataBuffer[iResult] = '\0';

                    // Log message text
                    int number = *((int*)dataBuffer);
                    printf("Server sent: %d.\n", number);

                }
                else if (iResult == 0)	// Check if shutdown command is received
                {
                    // Connection was closed successfully
                    printf("Connection with client closed.\n");
                    closesocket(connectSocket);
                }
                else	// There was an error during recv
                {

                    printf("recv failed with error: %d\n", WSAGetLastError());
                    closesocket(connectSocket);
                }
            }
        }

    } while (1);

    // Shutdown the connection since we're done
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


    // Close connected socket
    closesocket(connectSocket);

    // Deinitialize WSA library
    WSACleanup();

    return 0;
}