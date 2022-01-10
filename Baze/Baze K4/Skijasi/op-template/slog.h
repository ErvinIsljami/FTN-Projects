#ifndef SLOG_H
#define SLOG_H

#include <stdio.h>
#include <stdlib.h>

#define FAKTOR_BLOKIRANJA 3
#define FAKTOR_BLOKIRANJA_SEKV 2

#define SERIJSKA_DAT_IME "test_ser_sek.dat"
#define SEKVENCIJALNA_DAT_IME "sekvencijalna.dat"

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

void LogickoBrisanje(char* nazivDatoteke);

void IstampajSerijsku(char* nazivDatoteke);
void IstampajSkok(SLOG s);

int PrikazTakmSaVecimBodovima(char* nazivDatoteke, float duzina);
void NapraviIzvestaj(char* nazivDatotekeIn, char* nazivDatotekeOut);
int DaLiJeDrzavaUpisana(char* nazivDatoteke, char* drzava);
void DodajNoviSlogSekv(char* nazivDatoteke, char* drzava, double prosek);
void FormirajPrazanBlok(BLOK_SEKV* blok);
double IzracunajProsekDrzave(char* nazivDatoteke, char* drzava);
void IstampajSekvencijalnu(char* nazivDatoteke);

#endif // SLOG_H
