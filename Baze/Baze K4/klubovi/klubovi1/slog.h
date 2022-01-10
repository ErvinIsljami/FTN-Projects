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
}Blok;

void SmanjiBudzet(FILE *f)
{
    Blok blok;
    int flag = 0, i;

    fseek(f, 0, SEEK_SET);

    while(fread(&blok, sizeof(Blok), 1, f))
    {
        flag = 0;
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].godisnjiBudzet > 600)
            {
                blok.slog[i].godisnjiBudzet *= 0.9;
                flag = 1;
            }
        }

        if(flag == 1)
        {
            fseek(f, -sizeof(Blok), SEEK_CUR);
            fwrite(&blok, sizeof(Blok), 1, f);
            fseek(f, 0L, SEEK_CUR);
            fflush(f);
        }
    }
}

void IstampajSve(FILE *f)
{
    Blok blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("Klubovi: \n");
    while(fread(&blok, sizeof(Blok), 1, f))
    {
        printf("Blok%d\n", brojac);
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            printf("Slog %d:\t", i);
            IstampajSlog(blok.slog[i]);
        }
        brojac++;
        printf("\n\n");
    }
}

void IstampajSlog(SLOG s)
{
    printf("%d %s %s %d %d %d %f\n",    s.sifraKluba,
                                        s.nazivKluba,
                                        s.nazivGrada,
                                        s.brojPostignutih,
                                        s.brojPrimljenih,
                                        s.brojBodova,
                                        s.godisnjiBudzet);
}

void KluboviUOpsegu(FILE *f, float donjaGranica, float gornjaGranica)
{
    Blok blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("Klubovi ciji se budzet nalazi u opsegu %f - %f: \n", donjaGranica, gornjaGranica);
    while(fread(&blok, sizeof(Blok), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].godisnjiBudzet > donjaGranica && blok.slog[i].godisnjiBudzet < gornjaGranica)
                IstampajSlog(blok.slog[i]);
        }
    }
}



#endif // SLOG_H
