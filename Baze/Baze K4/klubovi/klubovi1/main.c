#include <stdio.h>
#include "slog.h"

int main() {
    // TODO : vase resenje
    FILE *f = fopen("test_ser_sek.dat", "rb+");

    IstampajSve(f);
    SmanjiBudzet(f);
    IstampajSve(f);
    KluboviUOpsegu(f, 300, 600);


    return 0;
}
