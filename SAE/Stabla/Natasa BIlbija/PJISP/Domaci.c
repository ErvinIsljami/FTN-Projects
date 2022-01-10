#include<stdio.h>
#include<stdlib.h>
#include <string.h>

typedef struct cena_st
{
	char oznakaGrada[3];
	char gorivo[11];
	float cena;
	struct cena_st* sledeci;
}CENA;
void ucitajIzDatoteke(FILE*, CENA**);
void dodajUListu(CENA** glava, char oznaka[], char gorivo[], float cena);
void upisiUDatoteku(FILE* out, CENA*);
void obrisiListu(CENA** glava);
FILE* safe_fopen(char ime[], char rezim[], int error_code);
int main(int brArg, char *args[])
{
	if(brArg != 3)
	{
		printf("Niste uneli dobar broj argumenata\n");
		exit(3);	//nije napisano koji broj pa je stavljena 3
	}
	CENA *glava = NULL;
	FILE *in = safe_fopen(args[1], "r", 1);
	
	ucitajIzDatoteke(in, &glava);
	
	FILE *out = safe_fopen(args[2], "w", 2);
		
	
	upisiUDatoteku(out, glava);

	fclose(in);
	fclose(out);
	obrisiListu(&glava);
	return 0;
}

void ucitajIzDatoteke(FILE *in, CENA **glava)
{
	char oznaka[3];
	char gorivo[11];
	float cena;
	while ( fscanf(in, "%s %s %f", oznaka, gorivo, &cena) != EOF)
	{
		dodajUListu(&(*glava), oznaka, gorivo, cena);
	}
}
FILE* safe_fopen(char ime[], char rezim[], int error_code)
{
	FILE *pom;
	pom = fopen(ime, rezim);
	if(pom == NULL)
		exit(error_code);

	return pom;
}
void dodajUListu(CENA ** glava, char oznaka[], char gorivo[], float cena)
{
	CENA* pom = (CENA*)malloc(sizeof(CENA));
	strcpy(pom->oznakaGrada, oznaka);
	strcpy(pom->gorivo, gorivo);
	pom->cena = cena;
	pom->sledeci = NULL;

	if (*glava == NULL)
	{
		*glava = pom;
	}
	else
	{
		CENA* tek = *glava;
		while (tek->sledeci != NULL)
		{
			tek = tek->sledeci;
		}
		tek->sledeci = pom;
	}
}

void upisiUDatoteku(FILE * out, CENA *glava)
{
	if (glava != NULL)
	{
		while (glava != NULL)
		{
			fprintf(out, "%6.2f %s %s\n", glava->cena, glava->oznakaGrada, glava->gorivo);
			glava = glava->sledeci;
		}
	}
}
void obrisiListu(CENA** glava)
{
	CENA* pom;
	
	while(*glava != NULL)
	{
		pom = (*glava);
		*glava = pom->sledeci;
		free(pom);		//ides napred, brise svaki koji je iza njega
	}
}