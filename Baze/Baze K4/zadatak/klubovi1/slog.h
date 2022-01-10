#ifndef SLOG_H
#define SLOG_H

#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#define FAKTOR_BLOKIRANJA 4

typedef struct
{   int sifraKluba;
    char nazivKluba[25];
    char nazivGrada[20];
    int brojPrimljenih;
    int brojPostignutih;
    int brojBodova;
    float godisnjiBudzet;
    int deleted;
} SLOG;

typedef struct blok
{
    SLOG slog[FAKTOR_BLOKIRANJA];
}BLOCK;

typedef struct
{
    char nazivGrada[20];
    float prosecniGodisnjiBudzet;
    int deleted;
}SLOG_SEKVENCIJALNA;

typedef struct
{
    SLOG_SEKVENCIJALNA slog[3];
}BLOK_SEKVENCIJALNA;

void ObrisiLogicki(char *imeFajla)
{
    BLOCK blok;
    int flag = 0, i;
    FILE *f = fopen(imeFajla, "rb+");

    while(fread(&blok, sizeof(BLOCK), 1, f))
    {
        flag = 0;
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].brojBodova < 12)
            {
                blok.slog[i].deleted = 1;
                flag = 1;
            }
        }
        if(flag == 1)
        {
            fseek(f, -sizeof(BLOCK), SEEK_CUR);
            fwrite(&blok, sizeof(BLOCK), 1, f);
            fseek(f, 0L, SEEK_CUR);
            fflush(f);
        }
    }
    fclose(f);
}

void StampajSve(char *imeFajla)
{
    BLOCK blok;
    int i, cnt = 1;
    FILE *f = fopen(imeFajla, "rb+");

    printf("Klubovi\n");
    while(fread(&blok, sizeof(BLOCK), 1, f))
    {
        printf("BLOCK%d\n", cnt);
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].deleted != 1)
            {
                printf("Slog %d:\t", i);
                StampajSlog(blok.slog[i]);
            }
        }
        cnt++;
        printf("\n");
    }
    fclose(f);
}

void StampajSlog(SLOG s)
{
    printf("%d %s %s %d %d %d %f\n",    s.sifraKluba,
                                        s.nazivKluba,
                                        s.nazivGrada,
                                        s.brojPostignutih,
                                        s.brojPrimljenih,
                                        s.brojBodova,
                                        s.godisnjiBudzet);
}

void StampajSekvencijalnu(char *imeFajla)
{
    BLOK_SEKVENCIJALNA blok;
    int i, blokCnt = 1;
    FILE *f = fopen(imeFajla, "rb+");

    while(fread(&blok, sizeof(BLOK_SEKVENCIJALNA), 1, f))
    {
        printf("BLOCK%d\n", blokCnt);
        for(i = 0; i < 3; i++)
        {
            if(blok.slog[i].deleted != 1)
            {
                printf("Slog %d:\t", i);
                StampajSekvencijalniSlog(blok.slog[i]);
            }
        }
        blokCnt++;
        printf("\n");
    }
    fclose(f);
}

void StampajSekvencijalniSlog(SLOG_SEKVENCIJALNA s)
{
    printf("%s - %f\n", s.nazivGrada, s.prosecniGodisnjiBudzet);
}

void GolRazlikaZaBudzet(char *imeFajla, int donjaGranica, int gornjaGranica)
{
    BLOCK blok;
    int i;
    int suma = 0;
    FILE *f = fopen(imeFajla, "rb+");
    while(fread(&blok, sizeof(BLOCK), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].deleted != 1)
            {
                if(blok.slog[i].godisnjiBudzet < gornjaGranica && blok.slog[i].godisnjiBudzet > donjaGranica)
                {
                    suma += blok.slog[i].brojPostignutih - blok.slog[i].brojPrimljenih;
                }
            }
        }
    }

    printf("Gol razlika za sve klubove sa budzetom u opsegu (%d, %d) je %d\n", donjaGranica, gornjaGranica, suma);
    fclose(f);
}

float IzracunajProsekDrzave(char* imeFajla, char* grad)
{
    BLOCK blok;
    FILE *f = fopen(imeFajla, "rb+");
    int i;
    fseek(f, 0, SEEK_SET);
    float suma = 0, n = 0;
    while(fread(&blok, sizeof(BLOCK), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(strcmp(blok.slog[i].nazivGrada, grad) == 0)
            {
                suma += blok.slog[i].godisnjiBudzet;
                n++;
            }
        }
    }
    fclose(f);
    return suma / n;
}

void FormirajIzvestaj(char *imeFajla)
{
    BLOCK blok;

    int i;
    FILE *in = fopen("test_ser_sek.dat", "rb+");
    FILE *out = fopen("test_ser_sek.dat", "rb+");
    char gradovi[100][100];
    int brojGradova = 0;

    BLOK_SEKVENCIJALNA noviBlok;
    FormirajPrazanBlok(&noviBlok);
    fwrite(&noviBlok, sizeof(BLOK_SEKVENCIJALNA), 1, out);

    while(fread(&blok, sizeof(BLOCK), 1, in))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].deleted == 1)
                continue;

            int isAlredyWritten = 0;
            for(int j = 0; j < brojGradova; j++)
            {
                if(strcmp(gradovi[j], blok.slog[i].nazivGrada) == 0)
                {
                    isAlredyWritten = 1;
                }
            }

            if(!isAlredyWritten)
            {
                float prosek = IzracunajProsekDrzave("test_ser_sek.dat", blok.slog[i].nazivGrada);
                DodajNoviSlog(out, blok.slog[i].nazivGrada, prosek);
                strcpy(gradovi[brojGradova++], blok.slog[i].nazivGrada);
            }
        }
    }

    fclose(out);
    fclose(in);
}

void FormirajPrazanBlok(BLOK_SEKVENCIJALNA* blok)
{
    int i = 0;
    for(i = 0; i < 3; i++)
    {
        SLOG_SEKVENCIJALNA slog;
        strcpy(slog.nazivGrada, "");
        slog.prosecniGodisnjiBudzet = -1;
        slog.deleted = 0;
        memcpy(&(blok->slog[i]), &slog, sizeof(SLOG_SEKVENCIJALNA));
    }
}

void DodajNoviSlog(FILE *fp, char *grad, float prosek)
{
    BLOK_SEKVENCIJALNA blok, blok2;
    SLOG_SEKVENCIJALNA slog, temp;
    int i;
    FILE* f = fopen("izlaz.dat", "rb+");
    strcpy(slog.nazivGrada, grad);
    slog.prosecniGodisnjiBudzet = prosek;
    slog.deleted = 0;
    int faktorBlokiranja = 3;

    while(fread(&blok, sizeof(BLOK_SEKVENCIJALNA), 1, f))
    {
        for(i = 0; i < faktorBlokiranja; i++)
        {
            if(strcmp(slog.nazivGrada, blok.slog[i].nazivGrada) < 0)
            {
               memcpy(&temp, &blok.slog[i], sizeof(SLOG_SEKVENCIJALNA));
               memcpy(&blok.slog[i], &slog, sizeof(SLOG_SEKVENCIJALNA));
               memcpy(&slog, &temp, sizeof(SLOG_SEKVENCIJALNA));
            }

            if(strcmp(slog.nazivGrada, "") == 0)
            {
                fseek(f, -sizeof(BLOK_SEKVENCIJALNA), SEEK_CUR);
                fwrite(&blok, sizeof(BLOK_SEKVENCIJALNA), 1, f);

                if(i == faktorBlokiranja - 1)
                {
                    FormirajPrazanBlok(&blok2);
                    fwrite(&blok2, sizeof(BLOK_SEKVENCIJALNA), 1, f);
                }
                fclose(f);
                return;
            }
        }

        fseek(f, -sizeof(BLOCK), SEEK_CUR);
        fwrite(&blok, sizeof(BLOCK), 1, f);
    }
    fclose(f);
}


#endif // SLOG_H
