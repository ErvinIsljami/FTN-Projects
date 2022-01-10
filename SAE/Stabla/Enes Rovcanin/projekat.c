#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct CVOR_ST
{
	int redniBroj;
    char naziv[20];
    char IPAdresa[20];
    int brzinaProtoka;

	struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

FILE *safefopen(char imeDatoteke[], char rezim[], int errorCode)
{
	FILE *fp;
	fp = fopen(imeDatoteke, rezim);
	if(fp == NULL)
	{
		if(strcmp(rezim, "r"))
		{
			printf("Datoteka %s nije pronadjena.\n", imeDatoteke);
		}
		else
		{
			printf("Doslo je do greske prilikom otvaranja datoteke %s. Nema dovoljno memorije.\n",imeDatoteke);
		}
		exit(errorCode);
	}
}

void ispisiBSP(CVOR * koren)
{
	if (koren == NULL)
		return;
	else
	{
		ispisiBSP(koren->levi);
        printf("%s\t%s\t%d \n",koren->naziv,  koren->IPAdresa, koren->brzinaProtoka);
		ispisiBSP(koren->desni);
	}

}

void dodajBSP(CVOR ** koren, CVOR* novi)
{
	if (*koren == NULL)
	{
		*koren = novi;
	}
	else
	{
		if ( (*koren)->redniBroj > novi->redniBroj )
		{
			dodajBSP(&(*koren)->levi, novi);
		}
		else
		{
			dodajBSP(&(*koren)->desni, novi);
		}
	}
}

void ucitajFajl(FILE *in, CVOR **koren)
{
	int n;
	fscanf(in,"%d", &n);
	printf("n  = %d\n",n);
	while(  n > 0)
	{
		CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
		novi->levi = NULL;
		novi->desni = NULL;
		fscanf(in, "%d %s %s %d",&(novi->redniBroj), novi->naziv, novi->IPAdresa, &(novi->brzinaProtoka));
		dodajBSP(&(*koren), novi);

		n--;
	}
}

int updateProtok(CVOR** koren, int redniBroj, int protok)
{
	if(*koren != NULL)
	{
		if((*koren)->redniBroj == redniBroj)
		{
			(*koren)->brzinaProtoka += protok;
			if((*koren)->brzinaProtoka > 100)
				(*koren)->brzinaProtoka = 100;
			
			return 1;
		}
		else if((*koren)->redniBroj > redniBroj)
			updateProtok(&(*koren)->levi, redniBroj, protok);
		else
			updateProtok(&(*koren)->desni, redniBroj, protok);

	}else
	return 0;
}

void update(CVOR** koren, FILE *in)
{
	int n;
	fscanf(in, "%d", &n);
	int i;
	for(i = 0; i < n; i++)
	{
		int redniBroj;
		int protok;
		fscanf(in, "%d %d", &redniBroj, &protok);
		if(updateProtok(&(*koren), redniBroj, protok) == 0)
		{
			printf("Ne postoji %d\n",redniBroj);
		}
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
            CVOR* temp = *koren;
            free(temp);
            *koren = NULL;
        }
        
    }
}

int main()
{
	CVOR *koren = NULL;	
    FILE *in;
    FILE *in2;
    in = safefopen("stablo.txt","r", 1);
    ucitajFajl(in, &koren);
    in2 = safefopen("brojevi.txt", "r", 2);

    update(&koren, in2);
 	
    printf("Izmenjeni protoci: \n");
	ispisiBSP(koren);

	//ciscenje memorije i zatvaranje datoteki
	fclose(in);
	fclose(in2);
	obrisiStablo(&koren);
    
	return 0;
}