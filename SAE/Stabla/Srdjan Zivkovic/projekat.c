#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct KORISNIK_ST
{
	int rbr;
    char ime[30];
    char prezime[30];
    int godina;
}KORISNIK;

typedef struct CVOR_ST
{
	KORISNIK inf;
	struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

void dodajEl(CVOR ** koren, KORISNIK inf);
void stampajStablo(CVOR *koren);
void clear(CVOR **koren);
FILE *safe_fopen(char ime_datoteke[], char mode[], int error);
void ucitaj_iz_fajla(FILE*in, CVOR **koren);
void update(CVOR** koren, FILE *in);
int promeni(CVOR** koren, int rdnBr, char prezime[]);

int main()
{
	CVOR *koren = NULL;	
    FILE *in;
    in = safe_fopen("stablo.txt","r",1);
    ucitaj_iz_fajla(in, &koren);
    fclose(in);
    in = safe_fopen("izmene.txt", "r",1);
    update(&koren, in);
    fclose(in);

	stampajStablo(koren);
	clear(&koren);
	stampajStablo(koren);
	return 0;
}

void dodajEl(CVOR ** koren, KORISNIK inf)
{
	//uvek se prvo proverava da li je koren prazan, ako jeste onda je novi element koren
	//jer koren(stablo) je prazno i nemamo na sta da dodamo
	if (*koren == NULL)
	{
		CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
		novi->inf = inf;
		novi->desni = NULL;
		novi->levi = NULL;
		*koren = novi;
	}
	else
	{
		//proverava se uslov sortiranja ako stablo nije prazno
		if ( (*koren)->inf.rbr < inf.rbr )
		{
			dodajEl(&(*koren)->desni, inf);
		}
		else
		{
			dodajEl(&(*koren)->levi, inf);
		}
	}
}

void stampajStablo(CVOR * koren)
{
	if (koren != NULL)
	{
		stampajStablo(koren->levi);

            printf("%s %s %d \n",koren->inf.ime,  koren->inf.prezime, koren->inf.godina);

		stampajStablo(koren->desni);
	}

}

void clear(CVOR **koren)
{
    if(*koren != NULL)
    {
        
    	clear(&(*koren)->levi);
    	clear(&(*koren)->desni);
        if( (*koren)->levi == NULL && (*koren)->desni == NULL)
        {
            CVOR* pom = *koren;
            free(pom);
            *koren = NULL;
        }
        
    }
}

FILE *safe_fopen(char ime_datoteke[], char mode[], int error)
{
	FILE *fp;
		fp = fopen(ime_datoteke, mode);
			if (fp == NULL)
			{
				printf("Onemogucen rad sa datotekom!\n");
				exit(error);
			}
	return fp;
}
void ucitaj_iz_fajla(FILE *in, CVOR **koren)
{
	KORISNIK inf;
	int n,i,k;
	int a, b, c, d;
	fscanf(in,"%d",&n);
	for(i = 0; i < n; i++)
	{
		fscanf(in, "%d %s %s %d",&inf.rbr, inf.ime, inf.prezime, &inf.godina);
		dodajEl(&(*koren), inf);
	}
}
void update(CVOR** koren, FILE *in)
{
	int n;
	fscanf(in, "%d", &n);
	int i;
	for(i = 0; i < n; i++)
	{
		char novoPrezime[20];
		int rdnBr;
		fscanf(in, "%d %s", &rdnBr, novoPrezime);
		if(!promeni(&(*koren), rdnBr, novoPrezime))
			printf("Nije pronadjen korisnik sa rednim brojem: %d\n", rdnBr);
	}
}

int promeni(CVOR** koren, int rdnBr, char prezime[])
{
	if(*koren != NULL)
	{
		if((*koren)->inf.rbr == rdnBr)
		{
			strcpy((*koren)->inf.prezime, prezime);	//kopira string2 u string1
			return 1;
		}
		else if((*koren)->inf.rbr > rdnBr)
			promeni(&(*koren)->levi, rdnBr, prezime);
		else
			promeni(&(*koren)->desni, rdnBr, prezime);
	}else
	return 0;
}
