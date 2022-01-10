#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <Windows.h>
#include <winsock2.h>
#include <WS2tcpip.h>

int SendPacket(SOCKET socket, char* message, int messageSize)
{
	int poslao = 0;
	int msgSize = messageSize;
	fd_set writefds;
	FD_ZERO(&writefds);
	FD_SET(socket, &writefds);
	timeval timeVal;
	timeVal.tv_sec = 1;
	timeVal.tv_usec = 0;
	unsigned long  mode = 1;
	if (ioctlsocket(socket, FIONBIO, &mode) != 0)
		printf("ioctlsocket failed with error.");
	do
	{
		FD_SET(socket, &writefds);
		int result = select(0, NULL, &writefds, NULL, &timeVal);
		if (result > 0)
		{
			if (FD_ISSET(socket, &writefds))
			{
				int iResult = send(socket, message + poslao, messageSize - poslao, 0);
				if (iResult == SOCKET_ERROR)
				{
					return WSAGetLastError();
				}
				//printf("Poslao %s\n", message + poslao);
				poslao += iResult;
				msgSize -= iResult;
				if (msgSize < 0)
				{
					return -1;
				}
			}
		}
		FD_CLR(socket, &writefds);
	} while (msgSize > 0);

	return 1;
}

int RecievePacket(SOCKET socket, char* recvBuffer, int length)
{
	int primio = 0;
	int len = length;

	fd_set readfds;
	FD_ZERO(&readfds);
	FD_SET(socket, &readfds);

	timeval timeVal;
	timeVal.tv_sec = 1;
	timeVal.tv_usec = 0;

	unsigned long  mode = 1;
	if (ioctlsocket(socket, FIONBIO, &mode) != 0)
		printf("ioctlsocket failed with error.");


	do
	{
		FD_SET(socket, &readfds);
		int result = select(0, &readfds, NULL, NULL, &timeVal);
		if (result > 0)
		{
			if (FD_ISSET(socket, &readfds))
			{
				int iResult = recv(socket, recvBuffer + primio, length - primio, 0);
				primio += iResult;
				if (iResult > 0)
				{
					//printf("Primio %s\n", recvBuffer);
				}
				else if (iResult == 0)
				{
					// connection was closed gracefully
					printf("Connection with client closed.\n");
					closesocket(socket);
				}
				else
				{
					// there was an error during recv
					printf("recv failed with error: %d\n", WSAGetLastError());
					return WSAGetLastError();
				}
				len -= iResult;
				if (len < 0)
				{
					printf("Greska primio vise nego sto treba.");
					return -1;
				}
				FD_CLR(socket, &readfds);
			}

		}

	} while (len > 0);

	return 1;
}

/*
rec(s, bufferBroj, 4);	//primim duzinu poruke
broj = atoi(bufferBroj);
char buffer[broj];
rec(s, buffer, broj);
*/

/*
void testFunction()
{
	if (FD_ISSET(socket, &readfds))
	{
		int len;
		iResult = RecievePacket(clientSockets[i], (char*)&len, 4);
		if (iResult != 1)
		{
			if (iResult == -1)
			{
				printf("Recieved more bytes than intended.\n");
			}
			else
			{
				printf("Error while getting packets. Error Code: %d\n", iResult);
				closesocket(clientSockets[i]);
				WSACleanup();
				return 1;
			}
		}
		char* recvBuffer = (char*)malloc(len + 1);
		iResult = RecievePacket(socket, recvBuffer, len);
		if (iResult != 1)
		{
			if (iResult == -1)
			{
				printf("Recieved more bytes than intended.\n");
			}
			else
			{
				printf("Error while getting packets. Error Code: %d\n", iResult);
				closesocket(clientSockets[i]);
				WSACleanup();
				return 1;
			}
		}
	}
}
*/