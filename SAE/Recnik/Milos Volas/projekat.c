#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<math.h>

#define hashSize 100
typedef struct par
{
	char kljuc[15];
	char vrednost[15];
}PAR;

typedef struct cvor_st
{
	PAR inf;
	struct cvor_st* sledeci;
}CVOR;

void ucitajSrEn(FILE *in, CVOR **recnik);
int getHashCode(char kljuc[], int velicinaHasha);
void dodajURecnik(CVOR **glava, char kljuc[], char vrednost[]);
void stampajRecnik(CVOR** recnik);
void pronadjiPrevode(CVOR **recnik1, CVOR** recnik2);
void pronadji(CVOR **recnik, char kljuc[]);
int main()
{
	FILE *in1, *in2;
	in1 = fopen("reci1.txt","r");
	in2 = fopen("reci2.txt","r");
	int i;
	CVOR** recnik = (CVOR**)malloc(hashSize*sizeof(CVOR*));
	for(i = 0; i < hashSize; i++)
		recnik[i] = NULL;

	CVOR** recnik2 = (CVOR**)malloc(hashSize*sizeof(CVOR*));
	for(i = 0; i < hashSize; i++)
		recnik2[i] = NULL;

	ucitajSrEn(in1,recnik);
	//stampajRecnik(recnik);
	ucitajSrEn(in2, recnik2);
	//stampajRecnik(recnik2);
	
	pronadjiPrevode(recnik, recnik2);

	return 0;
}
void stampajRecnik(CVOR** recnik)
{
	int i;
	for(i = 0; i < hashSize; i++)
	{
		CVOR* lista = recnik[i];
		printf("hash = %d\n", i);
		while(lista != NULL)
		{
			printf("kljuc: %s vrednost: %s\n",lista->inf.kljuc, lista->inf.vrednost);
			lista = lista->sledeci;
		}
		printf("**************\n");
	}
}
void ucitajSrEn(FILE* in, CVOR** recnik)
{
	char kljuc[15];
	char vrednost[15];

	while( fscanf(in, "%s %s", kljuc, vrednost) != EOF)
	{
		dodajURecnik(recnik, kljuc, vrednost);
	}
}

void dodajURecnik(CVOR **recnik, char kljuc[], char vrednost[])
{
	int hash = abs(getHashCode(kljuc, hashSize));

	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	strcpy(novi->inf.vrednost, vrednost);
	strcpy(novi->inf.kljuc, kljuc);
	novi->sledeci = NULL;

	if(recnik[hash] == NULL)
	{
		recnik[hash] = novi;
	}
	else
	{
		CVOR* tekuci = recnik[hash];
		while(tekuci->sledeci != NULL)
		{
			tekuci = tekuci->sledeci;
		}
		tekuci->sledeci = novi;
	}
}

int getHashCode(char kljuc[], int velicinaHasha)
{
	int i;
	int hash =0;
  	for (i =0; kljuc[i]!='\0'; i++){
    	hash = hash*kljuc[i] + kljuc[i] + i;
  	}
  return (hash%velicinaHasha);
}
void pronadjiPrevode(CVOR **recnik1, CVOR** recnik2)
{
	int i;
	for(i = 0; i < hashSize; i++)
	{
		CVOR* pom = recnik1[i];
		if(pom != NULL)
		{
			while(pom != NULL)
			{
				
				pronadji(recnik2, pom->inf.vrednost);
				pom = pom->sledeci;
			}
		}
	}
}

void pronadji(CVOR **recnik, char kljuc[])
{
	int hash = abs(getHashCode(kljuc, hashSize));
	CVOR* pom = recnik[hash];
	
	if(pom != NULL)
	{
		while(pom != NULL)
		{
			if(strcmp(pom->inf.kljuc, kljuc) == 0)
			{
				printf("Nalazi se: %s %s\n", pom->inf.vrednost, pom->inf.kljuc);
			}
			pom = pom->sledeci;
		}
	}
}