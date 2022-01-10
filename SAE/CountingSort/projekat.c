#include <stdio.h>
#include <stdlib.h>
typedef struct OSOBA_ST
{
	char ime[20];
	char prezime[20];
	char pol;
	int godina;
}OSOBA;
void CoutingSort(OSOBA A[], int n);
int ucitajStanovnike(FILE* fp, OSOBA niz[]);
void printStanovnike(OSOBA niz[], int n);
int main()
{
    int n;
    OSOBA niz[50];
    FILE *fp = fopen("stanovnici.txt","r");
   	if(fp == NULL)
   	{
   		printf("Greska prilikom otvaranja fajla!\n");
   		exit(1);
   	}
    n = ucitajStanovnike(fp, niz);

    CoutingSort(niz, n);
  
    printStanovnike(niz, n);



    return 0;
}
void CoutingSort(OSOBA niz[], int n)
{
    int i, j;
    int k = 2018;
    OSOBA resenje[n];
    int pomocni[2018+1];

    //inicijalizuje na nulus
    for (i = 0; i < k; i++)
        pomocni[i] = 0;

    //povecava brojace
    for (i = 0; i < n; i++)
        pomocni[ niz[i].godina]++;
    
    //sabira susedne
    for (i = 1; i <= k; i++)
        pomocni[i] = pomocni[i] + pomocni[i-1];
    
    //sortira niz
    for (j = 0; j < n; j++)
    {
        resenje[ pomocni[niz[j].godina-1] ] = niz[j];
        pomocni[niz[j].godina]--;
    }

    //kopira resenje u originalni niz
	for( i =0; i < n; i++)
		niz[i] = resenje[i];

}
int ucitajStanovnike(FILE* fp, OSOBA niz[])
{
	int n=0;
	while( fscanf(fp,"%s %s %c %d",	niz[n].ime, 
									niz[n].prezime, 
									&niz[n].pol, 
									&niz[n].godina) != EOF)
	{
		n++;
	}

	return n;
}
void printStanovnike(OSOBA niz[], int n)
{
	int i = 0;
	int cnt = 0;
	for(i = n-1; i >= 0; i--)
	{
		if(niz[i].godina < 2000)
		{
			printf("%s %s %c %d\n",	niz[i].ime, 
									niz[i].prezime, 
									niz[i].pol, 
									niz[i].godina);
			cnt++;
		}
	}
	printf("Ukupno glasaca: %d\n",cnt);
}