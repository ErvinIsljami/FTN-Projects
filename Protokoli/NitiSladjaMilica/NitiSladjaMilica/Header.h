#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

typedef struct cvor_st
{
	int vrednost;
	struct cvor_st* sledeci;
}CVOR;

CRITICAL_SECTION cs;

void Init(CVOR** glava)
{
	InitializeCriticalSection(&cs);
	EnterCriticalSection(&cs);
	*glava = NULL;
	LeaveCriticalSection(&cs);
}

void Enqueue(CVOR** glava, int vrednost)
{
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	if (novi == NULL)
	{
		printf("nema memorije...\n");
		return;
	}
	novi->vrednost = vrednost;
	novi->sledeci = NULL;

	novi->sledeci = *glava;

	EnterCriticalSection(&cs);
	*glava = novi;
	LeaveCriticalSection(&cs);
}

int Dequeue(CVOR** glava)
{
	CVOR* temp = *glava;
	if (temp == NULL)
	{
		printf("Lista je vec prazna.\n");
		return -1;
	}
	if (temp->sledeci == NULL)
	{
		int ret = temp->vrednost;
		
		EnterCriticalSection(&cs);
		free(temp);
		*glava = NULL;
		LeaveCriticalSection(&cs);

		return ret;
	}
	CVOR* prethodni = temp;
	temp = temp->sledeci;
	while (temp->sledeci != NULL)
	{
		prethodni = temp;
		temp = temp->sledeci;
	}
	int ret = temp->vrednost;
	EnterCriticalSection(&cs);
	free(temp);
	prethodni->sledeci = NULL;
	LeaveCriticalSection(&cs);

	return ret;
}
