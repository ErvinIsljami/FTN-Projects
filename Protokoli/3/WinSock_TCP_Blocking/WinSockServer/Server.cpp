#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27016"

bool InitializeWindowsSockets();

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

    int brojKlijenata = 0;
    unsigned long mode = 1; //non-blocking mode
    iResult = ioctlsocket(listenSocket, FIONBIO, &mode);
    if (iResult != NO_ERROR)
        printf("ioctlsocket failed with error: %ld\n", iResult);

    do
    {
        // Wait for clients and accept client connections.
        // Returning value is acceptedSocket used for further
        // Client<->Server communication. This version of
        // server will handle only one client.

        fd_set readfdsLS;
        FD_ZERO(&readfdsLS);
        FD_SET(listenSocket, &readfdsLS);

        timeval timeValLS;
        timeValLS.tv_sec = 1;
        timeValLS.tv_usec = 0;

        int result = select(0, &readfdsLS, NULL, NULL, &timeValLS);

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
            if (FD_ISSET(listenSocket, &readfdsLS)) 
            {
                acceptedSocket[brojKlijenata] = accept(listenSocket, NULL, NULL);

                if (acceptedSocket[brojKlijenata] == INVALID_SOCKET)
                {
                    printf("accept failed with error: %d\n", WSAGetLastError());
                    closesocket(listenSocket);
                    WSACleanup();
                    return 1;
                }

                mode = 1; //non-blocking mode
                iResult = ioctlsocket(acceptedSocket[brojKlijenata], FIONBIO, &mode);
                if (iResult != NO_ERROR)
                    printf("ioctlsocket failed with error: %ld\n", iResult);

                brojKlijenata++;
            }
        }

        fd_set readfds;
        FD_ZERO(&readfds);

        for (int i = 0; i < brojKlijenata; i++)
        {
            FD_SET(acceptedSocket[i], &readfds);
        }

        timeval timeVal;
        timeVal.tv_sec = 1;
        timeVal.tv_usec = 0;

        result = select(0, &readfds, NULL, NULL, &timeVal);

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
            for (int i = 0; i < brojKlijenata; i++)
            {
                // rezultat je jednak broju soketa koji su zadovoljili uslov
                if (FD_ISSET(acceptedSocket[i], &readfds))
                {
                    // Receive data until the client shuts down the connection
                    iResult = recv(acceptedSocket[i], recvbuf, DEFAULT_BUFLEN, 0);
                    if (iResult > 0)
                    {
                        printf("Message received from client: %s.\n", recvbuf);

                        sprintf(recvbuf, "Primio sam poruku od tebe, velicine: %d", strlen(recvbuf));
                        iResult = send(acceptedSocket[i], recvbuf, (int)strlen(recvbuf) + 1, 0);

                        if (iResult == SOCKET_ERROR)
                        {
                            printf("send failed with error: %d\n", WSAGetLastError());
                            closesocket(acceptedSocket[i]);
                            WSACleanup();
                            return 1;
                        }
                    }
                    else if (iResult == 0)
                    {
                        // connection was closed gracefully
                        printf("Connection with client closed.\n");
                        closesocket(acceptedSocket[brojKlijenata]);
                    }
                    else
                    {
                        // there was an error during recv
                        printf("recv failed with error: %d\n", WSAGetLastError());
                        closesocket(acceptedSocket[brojKlijenata]);
                    }
                }
            }
        }


    } while (1);

    // shutdown the connection since we're done
    /*iResult = shutdown(acceptedSocket, SD_SEND);
    if (iResult == SOCKET_ERROR)
    {
        printf("shutdown failed with error: %d\n", WSAGetLastError());
        closesocket(acceptedSocket);
        WSACleanup();
        return 1;
    }*/

    // cleanup
    closesocket(listenSocket);
    //closesocket(acceptedSocket);
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