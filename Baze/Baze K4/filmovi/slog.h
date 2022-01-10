#ifndef SLOG_H
#define SLOG_H

#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#define FAKTOR_BLOKIRANJA 4
#define OCENA_GRANICA 7.7
#define IME_DATOTEKE "test_ser_sek.dat"

#define IME_DATOTEKE_SEKV "test_sekvencijalna.dat"

#define FAKTOR_BLOKIRANJA_SEKV 3

typedef struct
{   int sifraFilma;
    char nazivFilma[25];
    char nazivDrzave[20];
    int godina;
    int trajanje;
    float ocena;
    float budzet;
    int deleted;
} SLOG;

typedef struct blok
{
    SLOG slog[FAKTOR_BLOKIRANJA];
}Blok;

typedef struct slog_st_sekv
{
    char nazivDrzave[20];
    float prosecanBudzet;
}SLOG_SEKV;

typedef struct blok_sekv_st
{
    SLOG_SEKV slog[FAKTOR_BLOKIRANJA_SEKV];
}Blok_sekv;

void LogickoBrisanje(FILE* f);
void IstampajSerijsku(FILE *f);

void NapraviIzvestaj(FILE *f_in, FILE *f_out);
int DaLiJeDrzavaUpisana(FILE *f, char* drzava);
void DodajNoviSlogSekv(FILE *f, char* drzava, double prosek);
void FormirajPrazanBlok(Blok_sekv* blok);
double IzracunajProsekDrzave(char* imeFajla, char* drzava);

void IstampajSekvencijalnu(FILE *f);

void NajboljeOcenjeni(FILE *f, int godina);


#endif // SLOG_H
