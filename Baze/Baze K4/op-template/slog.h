#ifndef SLOG_H
#define SLOG_H

typedef struct {
    int sifraPrimecivanja;
    char datum[13];
    int sifraMesta;
    char tipPokemona[13];
    char vrstaPokemona[13];
    int hp;
    int cp;
    int deleted;
} SLOG;

#endif // SLOG_H
