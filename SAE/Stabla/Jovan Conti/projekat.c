#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct CVOR_ST
{
	int redniBroj;
    char ime[30];
    char prezime[30];
    int godina;
    struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}KORISNIK;

void Dodaj(KORISNIK ** koren, KORISNIK *novi)
{
	if (*koren == NULL)
	{
		*koren = novi;
	}
	else
	{
		if ( (*koren)->redniBroj < novi->redniBroj )
		{
			Dodaj(&(*koren)->desni, novi);
		}
		else
		{
			Dodaj(&(*koren)->levi, novi);
		}
	}
}
void ucitajKorisnike(FILE *in, KORISNIK **koren)
{
	int n,i;
	fscanf(in,"%d",&n);
	for(i = 0; i < n; i++)
	{
		KORISNIK *novi = (KORISNIK*)malloc(sizeof(KORISNIK));
		fscanf(in, "%d %s %s %d",&novi->redniBroj, novi->ime, novi->prezime, &novi->godina);
		novi->levi = NULL;
		novi->desni = NULL;
		Dodaj(&(*koren), novi);
	}
}


/*
void Stampaj(KORISNIK * koren)
{
	if (koren != NULL)
	{
		Stampaj(koren->levi);

            printf("%s %s %d \n",koren->ime,  koren->prezime, koren->godina);

		Stampaj(koren->desni);
	}

}
*/

//glavna fja, drugi zadatak
void manjiRedniBrojevi(KORISNIK *koren, int redniBroj, int *brojac)
{
	if(koren != NULL)
	{
		//if koji vrsi selekciju
		if(koren->redniBroj <= redniBroj)
		{
			printf("%d %s %s %d\n", koren->redniBroj, koren->ime, koren->prezime, koren->godina);
			(*brojac)++;
		}
		//ide u levo podstablo
		if(koren->redniBroj > redniBroj)
		{
			manjiRedniBrojevi(koren->levi, redniBroj, brojac);
		}
		else
		{//ide u desno podstablo
			manjiRedniBrojevi(koren->desni, redniBroj, brojac);
			manjiRedniBrojevi(koren->levi, redniBroj, brojac);
		}
		
	}
	
}

void obrisiCeloStablo(KORISNIK **koren)
{
    if(*koren != NULL)
    {
        
    	obrisiCeloStablo(&(*koren)->levi);
    	obrisiCeloStablo(&(*koren)->desni);
        if( (*koren)->levi == NULL && (*koren)->desni == NULL)
        {
            KORISNIK* pom = *koren;
            free(pom);
            *koren = NULL;
        }
        
    }
}
int main()
{
	KORISNIK *koren = NULL;	
    FILE *in;
    int r;
    int brojac = 0;
    in = fopen("stablo.txt","r");
    ucitajKorisnike(in, &koren);
    fclose(in);

  	printf("Unesite redni broj: \n");
  	scanf("%d",&r);
  	manjiRedniBrojevi(koren, r, &brojac);
  	printf("Ukupno korisnika sa rednim brojem ispod %d je %d\n",r, brojac);

  	obrisiCeloStablo(&koren);
	return 0;
}
