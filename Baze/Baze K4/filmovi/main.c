#include <stdio.h>
#include "slog.h"

int main() {
    // TODO : vase resenje

    FILE *f = fopen(IME_DATOTEKE, "rb+");
    if(f == NULL)
    {
        printf("Datoteka '%s' nije uspesno otvorena", IME_DATOTEKE);
        return -1;
    }

    FILE *f_out = fopen(IME_DATOTEKE_SEKV, "wb+");
    if(f_out == NULL)
    {
        printf("Datoteka '%s' nije uspesno otvorena", IME_DATOTEKE);
        return -1;
    }


    LogickoBrisanje(f);
    puts("--------------------------------------------------------------------");
    IstampajSerijsku(f);
    NapraviIzvestaj(f, f_out);
    IstampajSekvencijalnu(f_out);

    NajboljeOcenjeni(f, 2019);


    return 0;
}
