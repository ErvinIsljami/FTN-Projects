#include <stdio.h>
#include <stdlib.h>
#include "slog.h"

int main() {
    // TODO : vase resenje
    int brojac = 0;
    int izbor;

    do
    {
        puts("Izaberite opciju: ");
        puts("1. Logicki obrisati skokove takmicara LJOKELS0Y1.");
        puts("2. Formiranje i popunjavanje sekvencijalne datoteke.");
        puts("3. Stampaj serijsku.");
        puts("4. Stampaj Sekvencijalnu.");
        puts("5. Broj takmicara sa vecim brojem poena od zadatog.");
        puts("0. IZLAZ.");

        scanf("%d", &izbor);
        float g;
        switch(izbor)
        {
        case 0:
            return 0;
        case 1:
            LogickoBrisanje(SERIJSKA_DAT_IME);
            break;
        case 2:
            NapraviIzvestaj(SERIJSKA_DAT_IME, SEKVENCIJALNA_DAT_IME);
            break;
        case 3:
            IstampajSerijsku(SERIJSKA_DAT_IME);
            break;
        case 4:
            IstampajSekvencijalnu(SEKVENCIJALNA_DAT_IME);
            break;
        case 5:
            printf("Unesite gracu za poene: ");
            scanf("%f", &g);
            int cnt = PrikazTakmSaVecimBodovima(SERIJSKA_DAT_IME, g);
            printf("Broj takmicara sa vecim broj poena od '%f' je: %d\n", g, cnt);
        default:
            printf("Greska kod unosa. Molimo vas probajte opet.");
        }
    }while(izbor != 0);


    return 0;
}
