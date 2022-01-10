#include "slog.h"
#include <string.h>

void LogickoBrisanje(FILE *f)
{
    BLOK blok;
    int flag = 0, i;

    fseek(f, 0, SEEK_SET);

    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        flag = 0;
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(strcmp(blok.slog[i].idTakmicar, "LJOKELS0Y1") == 0)
            {
                blok.slog[i].deleted = 1;
                flag = 1;
            }
        }

        if(flag == 1)
        {
            fseek(f, -sizeof(BLOK), SEEK_CUR);
            fwrite(&blok, sizeof(BLOK), 1, f);
            fseek(f, 0L, SEEK_CUR);
            fflush(f);
        }
    }
}

void IstampajSkok(SLOG s)
{
    printf("%d %d %s %s %.2f %.2f %d\n", s.idSkok, s.runda, s.idTakmicar, s.drzTakmicar, s.duzina, s.stil);
}

void IstampajSerijsku(FILE *f)
{
    BLOK blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("*************Sekvencijalna DATOTEKA**********************************\n");
    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        printf("BLOK%d\n", brojac);

        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(!blok.slog[i].deleted)
            {
                printf("Slog %d:\t", i);
                IstampajSkok(blok.slog[i]);
            }
        }
        brojac++;
        printf("\n\n");
    }
    printf("**************************************************************\n");
}

int PrikazTakmSaVecimBodovima(FILE *f, float duzina)
{
    BLOK blok;
    int i, brojac = 0;

    fseek(f, 0, SEEK_SET);

    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].idSkok == -1)
            {
                continue;
            }
            if(!blok.slog[i].deleted && blok.slog[i].duzina > duzina)
            {
                brojac++;
            }
        }
    }

    return brojac;
}

void NapraviIzvestaj(FILE *f_in, FILE *f_out)
{
    BLOK blok;
    int i;
    fseek(f_in, 0, SEEK_SET);

    BLOK_SEKV noviBlok;
    FormirajPrazanBlok(&noviBlok);
    fseek(f_out, 0, SEEK_SET);
    fwrite(&noviBlok, sizeof(BLOK_SEKV), 1, f_out);
    fclose(f_out);
    f_out = fopen("test_sek.dat", "rb+");
    fseek(f_out, 0, SEEK_SET);

    IstampajSekvencijalnu(f_out);

    while(fread(&blok, sizeof(BLOK), 1, f_in))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(blok.slog[i].deleted || blok.slog[i].idSkok == -1)
                continue;

            if(!DaLiJeDrzavaUpisana(f_out, blok.slog[i].drzTakmicar))
            {
                printf("Drzava '%s' nije upisana.\n", blok.slog[i].drzTakmicar);
                float prosek = IzracunajProsekDrzave("test_ser_sek.dat", blok.slog[i].drzTakmicar);
                DodajNoviSlogSekv(f_out, blok.slog[i].drzTakmicar, prosek);
            }
            else
            {
                printf("Drzava '%s' je vec upisana.\n", blok.slog[i].drzTakmicar);
            }
        }
    }
}

int DaLiJeDrzavaUpisana(FILE *f, char* drzava)
{
    BLOK_SEKV blok;
    int i;
    fseek(f, 0, SEEK_SET);

    while(fread(&blok, sizeof(BLOK_SEKV), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
        {
            if(strcmp(drzava, blok.slog[i].drzTakmicar) == 0)
            {
                return 1;
            }
        }
    }

    return 0;
}

void DodajNoviSlogSekv(FILE *fajl, char* drzava, double prosek)
{
    SLOG_SEKV slogKojiUpisujemo;
	slogKojiUpisujemo.deleted = 0;
	strcpy(slogKojiUpisujemo.drzTakmicar, drzava);
	slogKojiUpisujemo.prosek = prosek;

	BLOK_SEKV blok;
	fseek(fajl, 0, SEEK_SET);
	while (fread(&blok, sizeof(BLOK_SEKV), 1, fajl)) {

		for (int i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++) {

			if (strcmp(blok.slog[i].drzTakmicar, "***") == 0)
                {
				memcpy(&blok.slog[i], &slogKojiUpisujemo, sizeof(SLOG_SEKV));
				if (i != FAKTOR_BLOKIRANJA_SEKV-1)
                {
					strcpy(blok.slog[i+1].drzTakmicar, "***");

					fseek(fajl, -sizeof(BLOK_SEKV), SEEK_CUR);
					fwrite(&blok, sizeof(BLOK_SEKV), 1, fajl);

					printf("Novi slog evidentiran u datoteci.\n");
					return;

				}
				else
                {

					fseek(fajl, -sizeof(BLOK_SEKV), SEEK_CUR);
					fwrite(&blok, sizeof(BLOK_SEKV), 1, fajl);

					BLOK_SEKV noviBlok;
					strcpy(noviBlok.slog[0].drzTakmicar, "***");
					fwrite(&noviBlok, sizeof(BLOK_SEKV), 1, fajl);

					printf("Novi slog evidentiran u datoteci.\n");
					printf("(dodat novi blok)\n");
					return;
				}
			}
			else if (strcmp(blok.slog[i].drzTakmicar, slogKojiUpisujemo.drzTakmicar) == 0)
			{
                if (!blok.slog[i].deleted)
                {
                    printf("Slog sa tom vrednoscu kljuca vec postoji!\n");
                    return;
                }
                else
                {

                    memcpy(&blok.slog[i], &slogKojiUpisujemo, sizeof(SLOG_SEKV));

                    fseek(fajl, -sizeof(BLOK_SEKV), SEEK_CUR);
					fwrite(&blok, sizeof(BLOK_SEKV), 1, fajl);

					printf("Novi slog evidentiran u datoteci.\n");
					printf("(prepisan preko logicki izbrisanog)\n");
					return;
                }

            } else if (atoi(blok.slog[i].drzTakmicar) > atoi(slogKojiUpisujemo.drzTakmicar)) {

				SLOG_SEKV tmp;
				memcpy(&tmp, &blok.slog[i], sizeof(SLOG_SEKV));
				memcpy(&blok.slog[i], &slogKojiUpisujemo, sizeof(SLOG_SEKV));
				memcpy(&slogKojiUpisujemo, &tmp, sizeof(SLOG_SEKV));

				if (i == FAKTOR_BLOKIRANJA_SEKV-1) {
					fseek(fajl, -sizeof(BLOK_SEKV), SEEK_CUR);
					fwrite(&blok, sizeof(BLOK_SEKV), 1, fajl);
					fseek(fajl, 0, SEEK_CUR);   //??????????????????????
				}
			}
		}
	}
}

void FormirajPrazanBlok(BLOK_SEKV* blok)
{
    int i = 0;
    for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
    {
        SLOG_SEKV slog;
        strcpy(slog.drzTakmicar, "***");
        slog.prosek = 0;
        slog.deleted = 0;
        blok->slog[i] = slog;
    }
}

double IzracunajProsekDrzave(char* imeFajla, char* drzava)
{
    BLOK blok;
    FILE *f = fopen(imeFajla, "rb+");
    int i;
    fseek(f, 0, SEEK_SET);
    float suma = 0, n = 0;
    while(fread(&blok, sizeof(BLOK), 1, f))
    {
        for(i = 0; i < FAKTOR_BLOKIRANJA; i++)
        {
            if(strcmp(blok.slog[i].drzTakmicar, drzava) == 0)
            {
                suma += blok.slog[i].duzina + blok.slog[i].stil;
                n++;
            }
        }
    }

    return suma / n;
}

void IstampajSekvencijalnu(FILE *f)
{
    BLOK_SEKV blok;
    int i, brojac = 1;

    fseek(f, 0, SEEK_SET);

    printf("*************SERIJSKA DATOTEKA**********************************\n");
    while(fread(&blok, sizeof(BLOK_SEKV), 1, f))
    {
        printf("BLOK%d\n", brojac);

        for(i = 0; i < FAKTOR_BLOKIRANJA_SEKV; i++)
        {
            if(!blok.slog[i].deleted)
            {
                printf("Slog %d:\t", i);
                printf("%s - %.2f\n", blok.slog[i].drzTakmicar, blok.slog[i].prosek);
            }
        }
        brojac++;
        printf("\n\n");
    }
    printf("**************************************************************\n");
}

