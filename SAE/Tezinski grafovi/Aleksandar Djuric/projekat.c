#include<stdio.h>
#include<stdlib.h>

typedef struct racunar
{
	char ime[30];
	char ip_adresa[16];
	int protok;
}RACUNAR;

void ucitajIzFajla(RACUNAR racunari[], FILE* in, int n, int matrica[n][n]);
int najmanji(int n, int udaljenost[], int poseceni[]);
void dijkstra(int n, int matricaSusedstva[n][n], int pocetniCvor, int krajnjiCvor, RACUNAR racunari[], int d);

int main(int brArg, char *args[])
{
	FILE *fp = fopen("graf.txt", "r");
	int d= 15;

	int n;
	fscanf(fp, "%d", &n);

	RACUNAR racunari[n];
	int matricaSusedstva[n][n];
	ucitajIzFajla(racunari, fp, n, matricaSusedstva);
	
	dijkstra(n, matricaSusedstva, 1, 5, racunari, d);

	return 0;
}


void ucitajIzFajla(RACUNAR racunari[], FILE* in, int n, int matrica[n][n])
{
	int i, j;
	int a, b, c;
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%s %s %d", racunari[i].ime, racunari[i].ip_adresa, &racunari[i].protok);
		
	}
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			matrica[i][j] = 0;

	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		matrica[a-1][b-1] = c;
		matrica[b-1][a-1] = c;
	}
}


int najmanji(int n, int udaljenost[], int poseceni[])
{
	int min = 1000000000;
	int min_index = -1;
	int i;
	for (i = 0; i < n; i++)
		if (poseceni[i] == 0 && udaljenost[i] <= min)
		{
			min = udaljenost[i];
			min_index = i;
		}

	return min_index;
}
void dijkstra(int n, int matricaSusedstva[n][n], int pocetniCvor, int krajnjiCvor, RACUNAR racunari[], int d)
{
	int udaljenost[n];     
	int i, j;
	int poseceni[n];

	for(i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			if(racunari[i].protok > d)
				matricaSusedstva[i][j] = 0;

	for (int i = 0; i < n; i++)
	{
		udaljenost[i] = 1000000000;
		poseceni[i] = 0;	
	}

	udaljenost[pocetniCvor-1] = 0;

	for (i = 0; i < n - 1; i++)
	{
		int min = najmanji(n, udaljenost, poseceni);

		poseceni[min] = 1;

		//relax
		for (int j = 0; j < n; j++)
			if (!poseceni[j] && matricaSusedstva[min][j] != 0 && udaljenost[min] != 1000000000)
				if( udaljenost[min] + matricaSusedstva[min][j] < udaljenost[j] )
					udaljenost[j] = udaljenost[min] + matricaSusedstva[min][j];
	
	}
	//printf("od %d do %d ima %d rutera\n", pocetniCvor, krajnjiCvor, udaljenost[krajnjiCvor-1]);

	printf("Udaljenost svih cvorova od izvora:\n");
	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == 1000000000)	
			udaljenost[i] = -1;
		printf("cvor: %d \tudaljnost: %d\n",i+1, udaljenost[i]);
	}

}
