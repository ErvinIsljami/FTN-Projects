#include <stdio.h>
#include "slog.h"

int main()
{
    StampajSve("test_ser_sek.dat");
    ObrisiLogicki("test_ser_sek.dat");
    StampajSve("test_ser_sek.dat");
    GolRazlikaZaBudzet("test_ser_sek.dat", 250, 400);
    FormirajIzvestaj("test_ser_sek.dat");
    StampajSekvencijalnu("izlaz.dat");

    return 0;
}
