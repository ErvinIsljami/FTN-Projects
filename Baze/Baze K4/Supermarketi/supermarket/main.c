#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FAKTOR_BLOKIRANJA 4

typedef struct
{
    int sifraStavke;
    int sifraRacuna;
    char nazivArtikla[21];
    int cenaArtikla;
    int kolicnaArtikla;
    int stornirana;
}SLOG;

typedef struct
{
    SLOG slog[FAKTOR_BLOKIRANJA];
}BLOK;

void FormirajSerijskuDatoteku(char *imeFajla);
void FormirajPrazanBlok(BLOK *blok);
SLOG UnosSloga();
void UnosNovogSloga(char *imeFajla, SLOG s);
void IspisSvihSlogova(char *imeFajla);
void IspisiSlog(SLOG s);
void StornirajStavku(char *imeFajla, int kljuc);
void FormirajPrazanSlog(SLOG *slog);

int main()
{
    meni();
    return 0;
}

void meni(){

    int izbor;
    char imeFajla[30];

    do{
        printf("\n1.Formiraj serijsku datoteku\n");
        printf("2.Unos sloga\n");
        printf("3.Prikaz slogova\n");
        printf("4.Storniranje stavke\n");
        printf("5.Izlaz\n");
        scanf("%d", &izbor);
        switch(izbor){
            case 1:{
                printf("Unesite naziv datoteke: ");
                scanf("%s", &imeFajla);
                FormirajSerijskuDatoteku(imeFajla);
                break;
            }
            case 2:{
                SLOG s = UnosSloga();
                UnosNovogSloga(imeFajla, s);
                break;
            }
            case 3:{
                IspisSvihSlogova(imeFajla);
                break;
            }
            case 4:{
                int kljuc;
                printf("Unesite id stavke koju zelite da stornirate: ");
                scanf("%d", &kljuc);
                StornirajStavku(imeFajla, kljuc);
                break;
            }
            case 5:{
                printf("Izlazak iz programa");
            }
        }
    }while(izbor!=5);
}

void FormirajSerijskuDatoteku(char *imeFajla)
{
    FILE *f;
    BLOK blok;

    f = fopen(imeFajla, "wb");

    FormirajPrazanBlok(&blok);
    fwrite(&blok, sizeof(BLOK), 1, f);

    fclose(f);
}

void FormirajPrazanBlok(BLOK *blok)
{
    int i = 0;

    for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
    {
        FormirajPrazanSlog(&(blok->slog[i]));
    }
}

void FormirajPrazanSlog(SLOG *slog)
{
    (*slog).sifraStavke = -1;
    (*slog).kolicnaArtikla = 0;
    (*slog).cenaArtikla = 0;
    (*slog).sifraRacuna = -1;
    (*slog).stornirana = 0;
    strcpy((*slog).nazivArtikla, "");
}

SLOG UnosSloga()
{
    SLOG s;

    printf("Unesite sifru stavke: ");
    scanf("%d", &s.sifraStavke);

    printf("Unesite naziv artikla: ");
    scanf("%s", &s.nazivArtikla);

    printf("Unesite sifru racuna: ");
    scanf("%d", &s.sifraRacuna);

    printf("Unesite kolicina artikla: ");
    scanf("%d", &s.kolicnaArtikla);

    printf("Unesite cenu artikla: ");
    scanf("%d", &s.cenaArtikla);

    s.stornirana = 0;

    return s;
}

void UnosNovogSloga(char *imeFajla, SLOG slog)
{
    FILE *f;
    BLOK blok, blok2;
    int flag;
    int i = 0;
    f = fopen(imeFajla, "rb+");

    while (flag == 0 && fread(&blok, sizeof(BLOK), 1, f))
	{
		for (i = 0; i < FAKTOR_BLOKIRANJA; i++)
		{
			if (blok.slog[i].sifraStavke == slog.sifraStavke)
			{
				printf("U bazi vec postoji slog sa datom sifrom.");
				fclose(f);
				return;
			}
			if (blok.slog[i].sifraStavke == -1)
			{
				flag = 1;
				break;
			}
		}
	}

	fseek(f, -sizeof(BLOK), SEEK_CUR);

	if (blok.slog[i].sifraStavke == -1)
	{
		if (i == FAKTOR_BLOKIRANJA - 1)
		{
			FormirajPrazanBlok(&blok2);
			blok.slog[i] = slog;
			fwrite(&blok, sizeof(BLOK), 1, f);
			fwrite(&blok2, sizeof(BLOK), 1, f);
		}
		else
		{
			blok.slog[i] = slog;;
			fwrite(&blok, sizeof(BLOK), 1, f);
		}
	}
	else
	{
		printf("Doslo je do greske prilikom dodavanja. ");
	}

	fclose(f);
}

void IspisSvihSlogova(char *imeFajla)
{
    FILE *f;
    BLOK blok;
    int i, brojac = 1;
    f = fopen(imeFajla, "rb+");
    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        printf("BLOK%d\n\n", brojac);

        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].sifraStavke == -1)
            {
                fclose(f);
                return;
            }
            if(blok.slog[i].stornirana == 0)
            {
                printf("\nSlog %d \n", i);
                IspisiSlog(blok.slog[i]);
            }
        }
        brojac++;
    }

    fclose(f);
}

void IspisiSlog(SLOG s)
{
    printf("Sifra stavke: %d\n", s.sifraStavke);
    printf("Naziv Artikla: %s\n", s.nazivArtikla);
    printf("Kolicina Artikla: %d\n", s.kolicnaArtikla);
    printf("Cena Artikla: %d\n", s.cenaArtikla);
    printf("Sifra racuna: %d\n", s.sifraRacuna);
    printf("Stornirano: %d\n", s.stornirana);
}

void StornirajStavku(char *imeFajla, int kljuc)
{
    FILE *f;
    BLOK blok;
    int i, brojac = 1;
    f = fopen(imeFajla, "rb+");
    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].sifraStavke == kljuc)
            {
                blok.slog[i].stornirana = 1;
                fseek(f, -sizeof(BLOK), SEEK_CUR);
                fwrite(&blok, sizeof(BLOK), 1, f);
                break;
            }
        }
    }

    fclose(f);
}
