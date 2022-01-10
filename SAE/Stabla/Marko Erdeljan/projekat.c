#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct Korisnik
{
	int broj;
    char ime[30];
    char prezime[30];
    int godina;
}KORISNIK;

typedef struct CVOR_ST
{
	KORISNIK informacija;
    struct CVOR_ST* levi;
	struct CVOR_ST* desni;
}CVOR;

void dodajNoviElUStablo(CVOR ** koren, KORISNIK novi)
{
	if (*koren == NULL)
	{
		CVOR* noviEl = (CVOR*)malloc(sizeof(CVOR));
		noviEl->informacija = novi;
		noviEl->levi = NULL;
		noviEl->desni = NULL;

		*koren = noviEl;
	}
	else
	{
		if ( (*koren)->informacija.broj < novi.broj )
		{
			dodajNoviElUStablo(&(*koren)->desni, novi);
		}
		else
		{
			dodajNoviElUStablo(&(*koren)->levi, novi);
		}
	}
}
void ucitajPodatke(FILE *in, CVOR **koren)
{
	int n,i;
	fscanf(in,"%d",&n);
	for(i = 0; i < n; i++)
	{
		KORISNIK novi;
		fscanf(in, "%d %s %s %d",&novi.broj, novi.ime, novi.prezime, &novi.godina);
		dodajNoviElUStablo(&(*koren), novi);
	}
}



void stampajStablo(CVOR * koren)
{
	if (koren != NULL)
	{
		stampajStablo(koren->levi);

        printf("%s %s %d \n",koren->informacija.ime,  koren->informacija.prezime, koren->informacija.godina);

		stampajStablo(koren->desni);
	}

}

void obrisiCeloStablo(CVOR **koren)
{
    if(*koren == NULL)
    	return;
    else
    {
        
    	obrisiCeloStablo(&(*koren)->levi);
    	obrisiCeloStablo(&(*koren)->desni);
        if( (*koren)->levi == NULL && (*koren)->desni == NULL)
        {
            CVOR* pom = *koren;
            free(pom);
            *koren = NULL;
        }
        
    }
}

void UIntervalu(CVOR *original, CVOR **kopija, int a, int b)
{
	if(original != NULL)
	{

		if(original->informacija.broj >= a && original->informacija.broj <= b)
			dodajNoviElUStablo(&(*kopija), original->informacija);

		UIntervalu(original->desni, &(*kopija), a, b);
		
		UIntervalu(original->levi, &(*kopija), a, b);
		
	}
	
}

int main()
{
	CVOR *koren = NULL;	
    FILE *in;
    in = fopen("stablo.txt","r");
    ucitajPodatke(in, &koren);
    fclose(in);
  //stampajStablo(koren);
  
 //printf("*************************\n");
  	CVOR *kopija = NULL;
  	UIntervalu(koren, &kopija, 3, 9);
  	stampajStablo(kopija);

  	obrisiCeloStablo(&koren);
  	obrisiCeloStablo(&kopija);
	return 0;
}