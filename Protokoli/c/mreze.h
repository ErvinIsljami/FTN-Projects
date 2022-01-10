#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <windows.h>
#include <winsock2.h>
#include <WS2tcpip.h>


int SendPacket(SOCKET socket, char* message, int messageSize) {

    int poslao = 0;
    int msgSize = messageSize;
    fd_set writefds;
    FD_ZERO(&writefds);
    FD_SET(socket,&writefds);
    timeval timeVal;
    timeVal.tv_sec = 1;
    timeVal.tv_usec = 0;
    unsigned long mode = 1;
    if(ioctlsocket(socket,FIONBIO,&mode) != NULL) 
        printf("greska");
    
    do
    {
        FD_SET(socket,&writefds);
        int result = select(0,NULL,&writefds,NULL,&timeVal);
        if(result > 0)
        {
            if(FD_ISSET(socket,&writefds))
            {
                int iResult = send(socket,message + poslao, messageSize - poslao, 0);
                if(iResult == SOCKET_ERROR) 
                {
                    return WSAGetLastError();
                }

                poslao += iResult;
                msgSize -= iResult;

                if(msgSize < 0)
                {
                    return -1;
                }               
            }
        }

    FD_CLR(socket,&writefds);
    }while(msgSize > 0);

    return 1;
}