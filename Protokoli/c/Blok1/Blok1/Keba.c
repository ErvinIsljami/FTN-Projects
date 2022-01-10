#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <string.h>
#include <math.h>

#pragma warning(disable:4996)

#include "Liste.h"

int main1()
{
	CVOR* glava;
	InitList(&glava);
	Knjiga k1 = NapraviNovuKnjigu("Knjiga1", "autor1", 56, 32.5);
	Knjiga k2 = NapraviNovuKnjigu("Knjiga2", "autor2", 534, 2354.6);
	Knjiga k3 = NapraviNovuKnjigu("Knjiga3", "autor3", 645, 4345.9);
	Knjiga k4 = NapraviNovuKnjigu("Knjiga4", "autor4", 346, 5345.6);
	Knjiga k5 = NapraviNovuKnjigu("Knjiga5", "autor5", 8678, 4765.9);
	Knjiga k6 = NapraviNovuKnjigu("Knjiga6", "autor6", 756, 8678.9);

	Push(&glava, k1);
	Push(&glava, k2);
	Push(&glava, k3);
	Push(&glava, k4);
	PushFront(&glava, k5);
	Push(&glava, k6);
	Pop(&glava);
	PopFront(&glava);
	PrintList(glava);

	Knjiga k8 = Pop2(&glava);
	Knjiga k7 = Peak(glava);
	printf("Poslednja knjiga: %s\n", k7.naslov);
	printf("POp2 knjiga: %s\n", k8.naslov);

	SearchByIndex(glava, 2);
	SearchByIndex(glava, 13);

	printf("Unos index-a: \n");
	InsertEl(&glava, k5, 2);
	PrintList(glava);






	ClearList(&glava);
	PrintList(glava);
	return 0;
}