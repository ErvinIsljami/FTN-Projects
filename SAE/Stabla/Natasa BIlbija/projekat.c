#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct Student_st
{
	int brIndeksa;
	char ime[20];
	char prezime[20];
	int godina;
	int poeni;
}STUDENT;

typedef struct CVOR_ST
{
	int kljuc;
	int indeksUnizu;
	struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

int ucitajNiz(STUDENT nizA[], FILE *in);
void nizUStablo(STUDENT niz[], int n, CVOR** koren);
void dodajUStablo(CVOR** koren, int brIndeksa, int indeksUnizu);
void pretragaObaStabla(CVOR *koren1, CVOR *koren2, STUDENT niz[]);
int pretragaJednogElementa(CVOR* koren, int k);
int main()
{
	CVOR *koren1 = NULL;	//pravi stablo
	CVOR *koren2 = NULL;
	STUDENT nizA[50];		//pravi niz
	STUDENT nizB[50];
    FILE *in;			//pravi pokazivac za fajl
    FILE *in2;
    int n1, n2;			//br elemenata za prvi i drugi niz
    in = fopen("studenti1.txt","r");	//otvara datoteku za citanje("r")
    in2 = fopen("studenti2.txt", "r");

    n1 = ucitajNiz(nizA, in);		//ucitava studente u niz
    n2 = ucitajNiz(nizB, in2);
    nizUStablo(nizA, n1, &koren1);	//pretvara niz u stablo
    nizUStablo(nizB, n2, &koren2);

    pretragaObaStabla(koren1, koren2, nizA);	//pretrazuje drugo i prvo stablo i stampa iste

   	fclose(in);		//zatvaranje fajlova
 	fclose(in2);    
	return 0;
}
int ucitajNiz(STUDENT nizA[], FILE *in)
{
	int n = 0;
	while( (fscanf(in,"%d %s %s %d %d",&nizA[n].brIndeksa, nizA[n].ime, nizA[n].prezime, &nizA[n].godina, &nizA[n].poeni)) != EOF)
	{
		n++;
	}

	return n;
}
void nizUStablo(STUDENT niz[], int n, CVOR** koren)
{
	int i;
	for(i = 0; i < n; i++)
	{
		dodajUStablo(&(*koren), niz[i].brIndeksa, i);
	}
}
void dodajUStablo(CVOR** koren, int brIndeksa, int indeksUnizu)
{
	if(*koren == NULL)
	{
		CVOR* temp = (CVOR*)malloc(sizeof(CVOR));
		temp->indeksUnizu = indeksUnizu;
		temp->kljuc = brIndeksa;
		temp->levi = NULL;
		temp->desni = NULL;

		*koren = temp;
	}
	else if((*koren)->kljuc > brIndeksa)
		dodajUStablo(&(*koren)->levi, brIndeksa, indeksUnizu);
	else
		dodajUStablo(&(*koren)->desni, brIndeksa, indeksUnizu);
}

void pretragaObaStabla(CVOR *koren1, CVOR *koren2, STUDENT niz[])
{
	if(koren2 != NULL)
	{
		int i = pretragaJednogElementa(koren1, koren2->kljuc);
		
		if( i != -1)
			printf("%d %s %s %d %d\n", niz[i].brIndeksa, niz[i].ime, niz[i].prezime, niz[i].godina, niz[i].poeni);

		pretragaObaStabla(koren1, koren2->levi, niz);
		pretragaObaStabla(koren1, koren2->desni, niz);
	}
}
int pretragaJednogElementa(CVOR* koren, int k)
{
	if(koren != NULL)
	{
		if(koren->kljuc == k)
			return koren->indeksUnizu;
		else if(koren->kljuc > k)
			return pretragaJednogElementa(koren->levi, k);
		else	
			return pretragaJednogElementa(koren->desni, k);
	}

	return -1;
}