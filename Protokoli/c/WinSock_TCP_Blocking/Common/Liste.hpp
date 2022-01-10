#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#pragma warning(disable:4996)


typedef struct Knjiga_st
{
	char naslov[30];
	char autor[30];
	int brStrana;
	float cena;
}Knjiga;

typedef struct Cvor_st
{
	Knjiga podatak;
	struct Cvor_st* sledeci;
}CVOR;



void InitList(CVOR** glava);

void Push(CVOR** glava, Knjiga k);

void Pop(CVOR** glava);

void ClearList(CVOR** glava);

void PrintList(CVOR* glava);

Knjiga NapraviNovuKnjigu(char* naslov, char* autor, int brStr, float cena);

int CalculateLen(CVOR* glava);

char* Serialize(CVOR* glava);

void Deserialize(CVOR** glava, char* data, int len);


