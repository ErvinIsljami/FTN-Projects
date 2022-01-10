#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <time.h>
#include "Header.h"


typedef struct params_st
{
	CVOR** glava;
}PARAMS;
DWORD WINAPI pushThread(LPVOID lpParam);
DWORD WINAPI popThread(LPVOID lpParam);

int main()
{
	CVOR* glava;

	DWORD print1ID;
	DWORD print2ID;
	HANDLE handle1;
	HANDLE handle2;

	Init(&glava);
	PARAMS params1;
	PARAMS params2;
	params1.glava = &glava;
	params2.glava = &glava;

	//InitializeCriticalSection(&cs);

	handle1 = CreateThread(NULL, 0, &pushThread, &params1, 0, &print1ID);
	handle2 = CreateThread(NULL, 0, &popThread, &params2, 0, &print2ID);

	getchar();

}

DWORD WINAPI pushThread(LPVOID lpParam)
{
	PARAMS* params = (PARAMS*)lpParam;
	CVOR* glava = NULL;
	time_t t;
	srand((unsigned)time(&t));
	while (true)
	{
		int number = rand() % 100000;
		Enqueue(&(*params->glava), number);
		int vreme = rand() % 1000;
		Sleep(vreme);
	}

	return 0;
}

DWORD WINAPI popThread(LPVOID lpParam)
{
	PARAMS* params = (PARAMS*)lpParam;
	CVOR* glava = NULL;
	while (true)
	{
		Sleep(5000);
		while (*params->glava != NULL)
		{
			int vrednost = Dequeue(&(*params->glava));
			printf("Vrednost je: %d\n", vrednost);
		}
		printf("Sleep....");
	}

	return 0;
}