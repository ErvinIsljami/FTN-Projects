#include <stdio.h>
#include "slog.h"

int main()
{
    // TODO : vase resenje
    StampajSve("test_ser_sek.dat");
    ObrisiLogicki("test_ser_sek.dat");
    StampajSve("test_ser_sek.dat");
    GolRazlikaZaBudzet("test_ser_sek.dat", 300, 500);
    FormirajIzvestaj("test_ser_sek.dat");
    StampajSekvencijalnu("izlaz.dat");

    return 0;
}
