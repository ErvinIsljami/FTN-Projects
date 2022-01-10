#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct par
{
	char kljuc[20];
	int vrednost;
}PAR;

typedef struct cvor_st
{
	PAR inf;
	struct cvor_st* sledeci;
}CVOR;

void ucitajPromenljive1(FILE *in, CVOR **recnik);
int getHashCode(char kljuc[]);
void dodajUListu(CVOR **glava, char kljuc[], int vrednost);
void stampajRecnik(CVOR** recnik);
void ucitajPromenljive2(FILE* in, CVOR **recnik);
void pronadji(CVOR** recnik, char kljuc[]);
void ucitajPromenljive3(FILE *in, CVOR **recnik);
void obrisi(CVOR **recnik, char kljuc[]);

int main()
{
	FILE *in1, *in2, *in3;
	in1 = fopen("promenljive1.txt","r");
	in2 = fopen("promenljive2.txt","r");
	in3 = fopen("promenljive3.txt","r");
	int i;
	CVOR** recnik = (CVOR**)malloc(10*sizeof(CVOR*));
	for(i = 0; i < 10; i++)
		recnik[i] = NULL;

	ucitajPromenljive1(in1,recnik);
	stampajRecnik(recnik);
	//ucitajPromenljive2(in2, recnik);

	puts("------------------------");
	ucitajPromenljive3(in3, recnik);

	stampajRecnik(recnik);

	return 0;
}
void stampajRecnik(CVOR** recnik)
{
	int i;
	for(i = 0; i < 10; i++)
	{
		CVOR* lista = recnik[i];
		printf("hash = %d\n", i);
		while(lista != NULL)
		{
			printf("kljuc: %s vrednost: %d\n",lista->inf.kljuc, lista->inf.vrednost);
			lista = lista->sledeci;
		}
		printf("**************\n");
	}
}
void ucitajPromenljive1(FILE* in, CVOR** recnik)
{
	char kljuc[20];
	int vrednost;

	while( fscanf(in, "%s %d", kljuc, &vrednost) != EOF)
	{
		int hash = getHashCode(kljuc);
		dodajUListu( &recnik[hash], kljuc, vrednost);
	}
}

void dodajUListu(CVOR **glava, char kljuc[], int vrednost)
{
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->inf.vrednost = vrednost;
	strcpy(novi->inf.kljuc, kljuc);
	novi->sledeci = NULL;

	if(*glava == NULL)
	{
		*glava = novi;
	}
	else
	{
		CVOR* tekuci = *glava;
		while(tekuci->sledeci != NULL)
		{
			tekuci = tekuci->sledeci;
		}
		tekuci->sledeci = novi;
	}
}

int getHashCode(char kljuc[])
{
	return kljuc[0]%10;
}
void ucitajPromenljive2(FILE* in, CVOR **recnik)
{
	char kljuc[20];
	int vrednost;

	while( fscanf(in, "%s", kljuc) != EOF)
	{
		pronadji(recnik, kljuc);
	}
}
void pronadji(CVOR** recnik, char kljuc[])
{
	int hash = getHashCode(kljuc);
	CVOR* lista = recnik[hash];
	int nasao = 0;
	while(lista != NULL)
	{
		if(strcmp(lista->inf.kljuc, kljuc) == 0)
		{
			printf("[%s, %d]\n",kljuc, lista->inf.vrednost);
			nasao = 1;
			break;
		}
		lista = lista->sledeci;
	}
	if(nasao == 0)
		printf("Nije nasao %s\n",kljuc);
}
void ucitajPromenljive3(FILE *in, CVOR **recnik)
{
	char kljuc[20];
	
	while( fscanf(in, "%s", kljuc) != EOF)
	{
		obrisi(&(*recnik), kljuc);
	}
}
void obrisi(CVOR **recnik, char kljuc[])
{
	int hash = getHashCode(kljuc);
	CVOR* glava = recnik[hash];

	if(glava != NULL)
	{
		CVOR *temp = glava;
		if(strcmp(glava->inf.kljuc, kljuc) == 0)
		{
			recnik[hash] = temp->sledeci;
			free(temp);
		}
		else
		{
			temp = glava->sledeci;
			while(temp != NULL)
			{
				if(strcmp(temp->inf.kljuc, kljuc) == 0)
				{
					glava->sledeci = temp->sledeci;
					free(temp);
					break;
				}
				glava = glava->sledeci;
				temp = temp->sledeci;	
			}
		}

	}
}