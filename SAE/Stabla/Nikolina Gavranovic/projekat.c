#include<stdio.h>
#include<stdlib.h>
#include<string.h>


typedef struct KOSARKASI_ST
{
	int brDresa;
	char ime[20];
	char prezime[20];
	int brPoena;
	int brSkokova;
	int brAsistencija;
}KOSARKAS;

typedef struct CVOR_ST
{
	int kljuc;
	int indeks;
	struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

int ucitajNiz(KOSARKAS niz[], FILE *in, CVOR** koren);
void dodajUIndeksnoStablo(CVOR** koren, int brDresa, int indeksUNizu);
void stampajStablo(CVOR * koren, KOSARKAS niz[]);
void ucitajPromene(CVOR* koren, FILE* in, KOSARKAS niz[]);
int vratiIndeks(CVOR* koren, int kljuc);
int main()
{
	CVOR *koren = NULL;
	KOSARKAS niz[50];
    FILE *in1;
    FILE *in2;
    int n;

    in1 = fopen("kosarkasi.txt","r");
    in2 = fopen("kosarkasi2.txt", "r");

    n = ucitajNiz(niz, in1, &koren);
    ucitajPromene(koren, in2, niz);
    stampajStablo(koren, niz);

   	fclose(in1);
   	fclose(in2);
	return 0;
}
int ucitajNiz(KOSARKAS niz[], FILE *in, CVOR** koren)
{
	int n = 0;
	int m;
	fscanf(in,"%d",&m);

	while( (fscanf(in,"%d %s %s %d %d %d",&niz[n].brDresa, niz[n].ime, niz[n].prezime, &niz[n].brPoena, &niz[n].brSkokova, &niz[n].brAsistencija)) != EOF)
	{
		dodajUIndeksnoStablo(&(*koren), niz[n].brDresa, n);
		n++;
		m--;
		if(m == 0)
			break;
	}

	return n;
}
void dodajUIndeksnoStablo(CVOR** koren, int brDresa, int indeksUNizu)
{
	if(*koren == NULL)
	{

		CVOR* new = (CVOR*)malloc(sizeof(CVOR));
		new->indeks = indeksUNizu;
		new->kljuc = brDresa;
		new->levi = NULL;
		new->desni = NULL;

		*koren = new;
	}
	else if((*koren)->kljuc > brDresa)
		dodajUIndeksnoStablo(&(*koren)->levi, brDresa, indeksUNizu);
	else
		dodajUIndeksnoStablo(&(*koren)->desni, brDresa, indeksUNizu);
}

void stampajStablo(CVOR * koren, KOSARKAS niz[])
{
	if(koren != NULL)
	{
		stampajStablo(koren->levi, niz);
		int i = koren->indeks;
		printf("%d %s %s %d %d %d\n",niz[i].brDresa, niz[i].ime, niz[i].prezime, niz[i].brPoena, niz[i].brSkokova, niz[i].brAsistencija);
		stampajStablo(koren->desni, niz);
	}
}
int vratiIndeks(CVOR* koren, int kljuc)
{
	if(koren!= NULL)
	{
		if(koren->kljuc == kljuc)
			return koren->indeks;
		else if(koren->kljuc < kljuc)
			return vratiIndeks(koren->desni, kljuc);
		else
			return vratiIndeks(koren->levi, kljuc);
	}
	return -1;
}
void ucitajPromene(CVOR* koren, FILE* in, KOSARKAS niz[])
{
	int dres, poeni;

	while((fscanf(in,"%d %d",&dres, &poeni)) != EOF)
	{
		int i = vratiIndeks(koren, dres);
		niz[i].brPoena += poeni;
	}
}