#include <stdio.h>
#include <stdlib.h>

#include "List.hpp"

void Init(CVOR** glava)
{
	*glava = NULL;
}

void Enqueue(CVOR** glava, int podatak)
{
	CVOR* temp = *glava;
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	if (novi == NULL)
	{
		printf("Nije bilo dovoljno memorije.\n");
		return;
	}
	novi->val = podatak;
	novi->sledeci = NULL;

	novi->sledeci = temp;
	*glava = novi;
}

void StampajListu(CVOR* glava)
{
	CVOR* temp = glava;
	while (temp != NULL)
	{
		printf("%d -> ", temp->val);
		temp = temp->sledeci;
	}
	printf("NULL\n");
}

void DestroyList(CVOR** glava)
{
	CVOR* temp = *glava;
	while (*glava != NULL)
	{
		*glava = temp->sledeci;
		free(temp);
		temp = *glava;
	}
}

int Dequeue(CVOR** glava)
{
	CVOR* temp = *glava;
	if (temp == NULL)
	{
		printf("Lista je vec prazna.\n");
		return 0;
	}

	if (temp->sledeci == NULL)  //ima samo jedan element
	{
		int ret = temp->val;
		free(temp);
		return ret;
	}

	CVOR* prethodni = temp;
	temp = temp->sledeci;

	while (temp->sledeci != NULL)
	{
		prethodni = temp;
		temp = temp->sledeci;
	}

	int ret = temp->val;
	free(temp);
	prethodni->sledeci = NULL;
	return ret;
}