#ifndef SLOG_H
#define SLOG_H

#include <stdio.h>
#include <stdlib.h>

#define FAKTOR_BLOKIRANJA 3
#define FAKTOR_BLOKIRANJA_SEKV 2

typedef struct {
	int idSkok;
	int runda;
	char idTakmicar[10+1];
	char drzTakmicar[3+1];
	float duzina;
	float stil;
	int deleted;
} SLOG;

typedef struct {
    SLOG slog[FAKTOR_BLOKIRANJA]
}BLOK;

typedef struct
{
    char drzTakmicar[3+1];
    float prosek;
    int deleted;
}SLOG_SEKV;

typedef struct
{
    SLOG_SEKV slog[FAKTOR_BLOKIRANJA]
}BLOK_SEKV;

void LogickoBrisanje(FILE* f);

void IstampajSerijsku(FILE *f);
void IstampajSkok(SLOG s);

int PrikazTakmSaVecimBodovima(FILE *f, float duzina);

void NapraviIzvestaj(FILE *f_in, FILE *f_out);
int DaLiJeDrzavaUpisana(FILE *f, char* drzava);
void DodajNoviSlogSekv(FILE *f, char* drzava, double prosek);
void FormirajPrazanBlok(BLOK_SEKV* blok);
double IzracunajProsekDrzave(char* imeFajla, char* drzava);

void IstampajSekvencijalnu(FILE *f);



#endif // SLOG_H
