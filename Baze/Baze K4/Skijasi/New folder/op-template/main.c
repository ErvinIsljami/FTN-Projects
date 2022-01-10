#include <stdio.h>
#include <stdlib.h>
#include "slog.h"

int main() {
    // TODO : vase resenje
    FILE *f = fopen("test_ser_sek.dat", "rb+");
    FILE *f_out = fopen("test_sek.dat", "wb+");
    int brojac = 0;
    int izbor;

    do
    {

        puts("Izaberite opciju: ");
        puts("0. IZLAZ.");
        puts("1. Logicki obrisati skokove takmicara LJOKELS0Y1.");
        puts("2. Formiranje i popunjavanje sekvencijalne datoteke.");
        puts("3. Stampaj serijsku.");
        puts("4. Stampaj Sekvencijalnu.");
        puts("5. Broj takmicara sa vecim brojem poena od zadatog.");
        scanf("%d", &izbor);
        float granica;

        switch(izbor)
        {
        case 0:
            return 0;
        case 1:
            LogickoBrisanje(f);
            break;
        case 2:
            NapraviIzvestaj(f, f_out);
            break;
        case 3:
            IstampajSerijsku(f);
            break;
        case 4:
            IstampajSekvencijalnu(f_out);
            break;
        case 5:
            printf("Unesite gracu za poene: ");
            scanf("%f", &granica);
            int brojac = PrikazTakmSaVecimBodovima(f, 50.0);
            printf("Broj takmicara koji su u nekom skoku ostvarili vise od 50 poena je: %d\n", brojac);
        default:
            printf("Niste uneli dobru opciju. Molimo vas probaajte opet.");
        }

    }while(izbor != 0);




    fclose(f);
    fclose(f_out);
    return 0;
}
