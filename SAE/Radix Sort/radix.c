#include <stdio.h>
#include <string.h>
 
typedef struct korisnik
{
  char ime[20];
  int slika;
  int pratilaca;
  int pracenih;
}KORISNIK;

int ucitajIzFajla(FILE* fp, KORISNIK korisnici[]);
void countSort(KORISNIK korisnici[], int n, int exp);
int getMax(KORISNIK korisnici[], int n);
void radixSort(KORISNIK korisnici[], int n);
void printUsers(KORISNIK korisnici[], int n);
int main()
{
    int i;
    int n;
    FILE *in = fopen("instagram.txt", "r");
    KORISNIK korisnici[30];

    n = ucitajIzFajla(in, korisnici);
    printf("n = %d\n",n);
    /*
    printUsers(korisnici, n);
    printf("************\n");
    */
    radixSort(korisnici, n);
    printUsers(korisnici, n);

    
    return 0;
}
int ucitajIzFajla(FILE* fp, KORISNIK korisnici[])
{
  int i = 0;
  while( (fscanf(fp,"%s %d %d %d",korisnici[i].ime, &korisnici[i].slika, &korisnici[i].pratilaca, &korisnici[i].pracenih)) != EOF)
  {
    i++;
  }

  return i;
}
int getMax(KORISNIK korisnici[], int n)
{
    int mx = korisnici[0].pratilaca;
    for (int i = 1; i < n; i++)
        if (korisnici[i].pratilaca > mx)
            mx = korisnici[i].pratilaca;

    return mx;
}

void countSort(KORISNIK korisnici[], int n, int exp, int pocetni)
{
    KORISNIK output[n];
    int i, count[10] = {0};
 
    for (i = 0; i < n; i++)
        count[ (korisnici[i].pratilaca/exp)%10 ]++;
 
    for (i = 1; i < 10; i++)
        count[i] += count[i - 1];

    for (i = n - 1; i >= 0; i--)
    {
        output[count[ (korisnici[i].pratilaca/exp)%10 ] - 1] = korisnici[i];
        count[ (korisnici[i].pratilaca/exp)%10 ]--;
    }
 
    for (i = 0; i < n; i++)
        korisnici[i] = output[i];
}

void radixSort(KORISNIK korisnici[], int n)
{
    int m = getMax(korisnici, n);

    for (int exp = 1; m/exp > 0; exp *= 10)
        countSort(korisnici, n, exp);
}

void printUsers(KORISNIK korisnici[], int n)
{
  int i;
  for(i = 0; i < n; i++)
    printf("%s %d %d %d\n",korisnici[i].ime, korisnici[i].slika, korisnici[i].pratilaca, korisnici[i].pracenih);
}