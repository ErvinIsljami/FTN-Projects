#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <conio.h>

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT 27016

// Initializes WinSock2 library
// Returns true if succeeded, false otherwise.
bool InitializeWindowsSockets();

typedef struct message_st
{
    char typeOfMessage;
    int number;
}MESSAGE;

int __cdecl main() 
{
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
    serverAddress.sin_addr.s_addr = inet_addr("127.0.0.1");
    serverAddress.sin_port = htons(DEFAULT_PORT);
    // connect to server specified in serverAddress and socket connectSocket
    if (connect(connectSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR)
    {
        printf("Unable to connect to server.\n");
        closesocket(connectSocket);
        WSACleanup();
    }

    unsigned long mode = 1; //non-blocking mode
    iResult = ioctlsocket(connectSocket, FIONBIO, &mode);
    if (iResult != NO_ERROR)
        printf("ioctlsocket failed with error: %ld\n", iResult);

    fd_set readfds;
    FD_ZERO(&readfds);
    

    while (true)
    {
        puts("Unesite izbor: ");
        puts("0. Exit.");
        puts("1. Posalji podatak.");
        puts("2. Skini podatak sa servera.");
        
        int i;
        scanf_s("%d", &i);
        char messageBuffer[512];
        if (i == 0)
        {
            puts("Client is shuting down...");
            break;
        }
        if (i == 1)
        {
            int val;
            puts("Unesite broj koji zelite da posaljete na server.");
            scanf_s("%d", &val);
            MESSAGE msg;
            msg.number = val;
            msg.typeOfMessage = 0;  //PUSH

            // Send an prepared message with null terminator included
            iResult = send(connectSocket, (char*)&msg, sizeof(MESSAGE), 0);

            if (iResult == SOCKET_ERROR)
            {
                printf("send failed with error: %d\n", WSAGetLastError());
                closesocket(connectSocket);
                WSACleanup();
                return 1;
            }
        }
        else if (i == 2)
        {
            MESSAGE msg;
            msg.number = 0; //nije ni bitno
            msg.typeOfMessage = 1;  //POP

            // Send an prepared message with null terminator included
            iResult = send(connectSocket, (char*)&msg, sizeof(MESSAGE), 0);

            if (iResult == SOCKET_ERROR)
            {
                printf("send failed with error: %d\n", WSAGetLastError());
                closesocket(connectSocket);
                WSACleanup();
                return 1;
            }
            //ovde primamo rezultat
            
            FD_SET(connectSocket, &readfds);
            timeval timeVal;
            timeVal.tv_sec = 2;
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
            else if(FD_ISSET(connectSocket, &readfds))
            {
                // rezultat je jednak broju soketa koji su zadovoljili uslov
                iResult = recv(connectSocket, messageBuffer, DEFAULT_BUFLEN, 0);
                if (iResult > 0)
                {
                    printf("%s", messageBuffer);
                }
                else if (iResult == 0)
                {
                    // connection was closed gracefully
                    printf("Connection with client closed.\n");
                    closesocket(connectSocket);
                }
                else
                {
                    // there was an error during recv
                    printf("recv failed with error: %d\n", WSAGetLastError());
                    closesocket(connectSocket);
                }
            }
            FD_CLR(connectSocket, &readfds);
        }
    }

    // cleanup
    closesocket(connectSocket);
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
