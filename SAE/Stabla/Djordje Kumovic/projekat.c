#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct MREZA
{
	int rdnBr;
    char naziv[20];
    char ipAddresa[20];
    int protok;
}MREZA;

typedef struct CVOR_ST
{
	MREZA podatak;
	struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

void dodajUStablo(CVOR ** koren, MREZA inf);
void print(CVOR *koren);
void obrisiStablo(CVOR **koren);
void ucitajFajl(FILE*in, CVOR **koren);
void update(CVOR** koren, FILE *in);
void promeni(CVOR** koren, int rdnBr, int povecanje);

int main()
{
	CVOR *koren = NULL;	//OBAVEZNO
    FILE *in;
    FILE *in2;
    in = fopen("stablo.txt","r");
    ucitajFajl(in, &koren);
    print(koren);
    fclose(in);
    in2 = fopen("brojevi.txt", "r");
    update(&koren, in2);
 	fclose(in2);
    printf("Izmenjeni protoci: \n");
	print(koren);


	obrisiStablo(&koren);
    
	return 0;
}

void print(CVOR * koren)
{
	if (koren != NULL)
	{
		print(koren->levi);

            printf("%s %s %d \n",koren->podatak.naziv,  koren->podatak.ipAddresa, koren->podatak.protok);

		print(koren->desni);
	}

}


void ucitajFajl(FILE *in, CVOR **koren)
{
	MREZA podatak;
	int n;
	fscanf(in,"%d", &n);
	while( (fscanf(in, "%d %s %s %d",&podatak.rdnBr, podatak.naziv, podatak.ipAddresa, &podatak.protok) != EOF) && --n != 0)
	{
		dodajUStablo(&(*koren), podatak);
	}
}
void dodajUStablo(CVOR ** koren, MREZA podatak)
{
	if (*koren == NULL)
	{
		CVOR* pom = (CVOR*)malloc(sizeof(CVOR));
		pom->podatak = podatak;
		pom->desni = NULL;
		pom->levi = NULL;
		*koren = pom;
	}
	else
	{
		if ( (*koren)->podatak.rdnBr < podatak.rdnBr )
		{
			dodajUStablo(&(*koren)->desni, podatak);
		}
		else
		{
			dodajUStablo(&(*koren)->levi, podatak);
		}
	}
}


void update(CVOR** koren, FILE *in)
{
	int n;
	fscanf(in, "%d", &n);
	int i;
	for(i = 0; i < n; i++)
	{
		int rdnBr;
		int povecanje;
		fscanf(in, "%d %d", &rdnBr, &povecanje);
		promeni(&(*koren), rdnBr, povecanje);
	}
}

void promeni(CVOR** koren, int rdnBr, int povecanje)
{
	if(*koren == NULL)
		return;
	else
	{
		if((*koren)->podatak.rdnBr == rdnBr)
		{
			(*koren)->podatak.protok += povecanje;
			if((*koren)->podatak.protok > 100)
				(*koren)->podatak.protok = 100;

		}
		else if((*koren)->podatak.rdnBr > rdnBr)
			promeni(&(*koren)->levi, rdnBr, povecanje);
		else
			promeni(&(*koren)->desni, rdnBr, povecanje);
	}
}

void obrisiStablo(CVOR **koren)
{
    if(*koren != NULL)
    {
        
    	obrisiStablo(&(*koren)->levi);
    	obrisiStablo(&(*koren)->desni);
        if( (*koren)->levi == NULL && (*koren)->desni == NULL)
        {
            CVOR* pom = *koren;
            free(pom);
            *koren = NULL;
        }
        
    }
}