#include "slog.h"

void LogickoBrisanje(FILE *f)
{
    Blok blok;
    int flag = 0, i;

    fseek(f, 0, SEEK_SET);


    while(fread(&blok, sizeof(Blok), 1, f))
    {
        flag = 0;
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            //puts("uso");
            if(blok.slog[i].ocena < 7.7 && blok.slog[i].sifraFilma != -1)
            {
                blok.slog[i].deleted = 1;
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

void IstampajFilm(SLOG s)
{
    printf("%d %s %s %d %d %.2f %.2f %d %d\n",  s.sifraFilma,
                                                            s.nazivFilma,
                                                            s.nazivDrzave,
                                                            s.godina,
                                                            s.trajanje,
                                                            s.ocena,
                                                            s.budzet,
                                                            s.deleted);

}

void IstampajSerijsku(FILE *f)
{
    Blok blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("*************SERIJSKA DATOTEKA**********************************\n");
    while(fread(&blok, sizeof(Blok), 1, f))
    {
        printf("Blok%d\n", brojac);

        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            printf("Slog %d:\t", i);
            IstampajFilm(blok.slog[i]);
        }
        brojac++;
        printf("\n\n");
    }
    printf("**************************************************************\n");
}

void NapraviIzvestaj(FILE *f_in, FILE *f_out)
{
    Blok blok;
    int i;
    fseek(f_in, 0, SEEK_SET);

    Blok_sekv noviBlok;
    FormirajPrazanBlok(&noviBlok);
    fseek(f_out, 0, SEEK_SET);
    fwrite(&noviBlok, sizeof(Blok_sekv), 1, f_out);
    fseek(f_out, 0, SEEK_SET);

    while(fread(&blok, sizeof(Blok), 1, f_in))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(!DaLiJeDrzavaUpisana(f_out, blok.slog[i].nazivDrzave))
            {
                float prosek = IzracunajProsekDrzave(IME_DATOTEKE, blok.slog[i].nazivDrzave);
                DodajNoviSlogSekv(f_out, blok.slog[i].nazivDrzave, prosek);
            }
        }
    }
}

int DaLiJeDrzavaUpisana(FILE *f, char* drzava)
{
    Blok_sekv blok;
    int i;
    fseek(f, 0, SEEK_SET);

    while(fread(&blok, sizeof(Blok_sekv), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
        {
            if(strcmp(drzava, blok.slog[i].nazivDrzave) == 0)
            {
                return 1;
            }
        }
    }

    return 0;
}

void DodajNoviSlogSekv(FILE *f, char* drzava, double prosek)
{
    Blok_sekv blok, blok2;
    SLOG_SEKV slog, temp;
    int i;

    strcpy(slog.nazivDrzave, drzava);
    slog.prosecanBudzet = prosek;

    printf("Upisujem u sekvencijalnu: %s - %f\n", drzava, prosek);

    while(fread(&blok, sizeof(Blok_sekv), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
        {
            if(strcmp(slog.nazivDrzave, blok.slog[i].nazivDrzave) > 0)
            {
               memcpy(&temp, &blok.slog[i], sizeof(SLOG_SEKV));
               memcpy(&blok.slog[i], &slog, sizeof(SLOG_SEKV));
               memcpy(&slog, &temp, sizeof(SLOG_SEKV));
            }

            if(strcmp(slog.nazivDrzave, "*****") == 0)
            {
                fseek(f, -sizeof(Blok_sekv), SEEK_CUR);
                fwrite(&blok, sizeof(Blok_sekv), 1, f);

                if(i == FAKTOR_BLOKIRANJA_SEKV - 1)
                {
                    FormirajPrazanBlok(&blok2);
                    fwrite(&blok2, sizeof(Blok_sekv), 1, f);
                }
                fclose(f);

                return;
            }
        }

        fseek(f, -sizeof(Blok), SEEK_CUR);
        fwrite(&blok, sizeof(Blok), 1, f);
    }
}

void FormirajPrazanBlok(Blok_sekv* blok)
{
    int i = 0;
    for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
    {
        SLOG_SEKV slog;
        strcpy(slog.nazivDrzave, "*****");
        slog.prosecanBudzet = -1;
        blok->slog[i] = slog;
    }
}

double IzracunajProsekDrzave(char* imeFajla, char* drzava)
{
    Blok blok;
    FILE *f = fopen(IME_DATOTEKE, "rb+");
    int i;
    fseek(f, 0, SEEK_SET);
    float suma = 0, n = 0;
    while(fread(&blok, sizeof(Blok), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(strcmp(blok.slog[i].nazivDrzave, drzava) == 0)
            {
                suma += blok.slog[i].budzet;
                n++;
            }
        }
    }

    return suma / n;
}

void IstampajSekvencijalnu(FILE *f)
{
    Blok_sekv blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("\n\n*************SEKVENCIJALNA DATOTEKA**********************************\n\n");
    while(fread(&blok, sizeof(Blok_sekv), 1, f))
    {
        printf("Blok%d\n", brojac);

        for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
        {
            printf("Slog %d:\t", i);
            printf("%s  -  %.2f\n", blok.slog[i].nazivDrzave, blok.slog[i].prosecanBudzet);
        }
        brojac++;
        printf("\n\n");
    }
    printf("\n********************************************************************\n\n");
}


void NajboljeOcenjeni(FILE *f, int godina)
{
    Blok blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);
    float max = -1;
    SLOG maxSlog;

    while(fread(&blok, sizeof(Blok), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(max < blok.slog[i].ocena && blok.slog[i].godina == godina)
            {
                max = blok.slog[i].ocena;
                maxSlog = blok.slog[i];
            }
        }
    }

    fseek(f, 0, SEEK_SET);
    while(fread(&blok, sizeof(Blok), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(max == blok.slog[i].ocena && blok.slog[i].godina == godina)
            {
                printf("Najbolje ocenjeni film %d godine je: ", godina);
                IstampajFilm(maxSlog);
            }
        }
    }

}





