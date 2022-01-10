#pragma once
#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <conio.h>

int SendPacket(SOCKET socket, char* message, int messageSize);

int RecievePacket(SOCKET socket, char* recvBuffer, int length);

