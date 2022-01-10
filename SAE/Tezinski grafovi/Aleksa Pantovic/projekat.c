#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<limits.h>

typedef struct grad_st
{
	char ime[20];
	int brojStanovnika;
	int nadmorskaVisina;
}GRAD;

void ucitajGraph(GRAD gradovi[], FILE* in, int n, int AdjMatrix[n][n]);
void printMatrix(int n, int AdjMatrix[n][n]);
void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[]);
int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[], GRAD gradovi[]);
void initSingleSource(int n, int udaljenost[], int poseceni[], int pocetni);
void relax(int n, int AdjMatrix[n][n], int udaljenost[], int poseceni[], int min, GRAD gradovi[]);


int main()
{
	FILE *in = fopen("graf.txt","r");	//datoteka iz koje ucitavamo graf
	int n;	
	fscanf(in,"%d",&n);	//ucitavamo n iz fajla, velicina grafa
	int AdjMatrix[n][n];	//pravimo matricu susedstva n x n
	GRAD gradovi[n];		//pravimo niz gradova

	ucitajGraph(gradovi, in, n, AdjMatrix);	//funkcija za ucitavanje grafova
	
	dijkstra(n, AdjMatrix, 0, gradovi);	//dijkstrin algoritam

	return 0;
}
void ucitajGraph(GRAD gradovi[], FILE* in, int n, int AdjMatrix[n][n])
{
	int a,b,c,i,j;
	//inicijalizacija matrice na 0
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			AdjMatrix[i][j] = 0;
	
	for(i = 0; i < n; i++)
	{
		//citamo iz datoteke jedan po jedan grad i smestamo u niz struktura
		fscanf(in,"%s %d %d", gradovi[i].ime, &gradovi[i].brojStanovnika, &gradovi[i].nadmorskaVisina);
		//mozes da istampas cisto da vidis da li je dobro uneo, ne moras
		//printf("%s %d %d\n", gradovi[i].ime, gradovi[i].brojStanovnika, gradovi[i].nadmorskaVisina);
	}

	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		AdjMatrix[a-1][b-1] = c;	//postavimo vezu
		AdjMatrix[b-1][a-1] = c;	//ova linija ne treba ako je usmeren graf, ali zadatak to ne nalaze
	}
}

//test funkcija za adj matricu, mozes obrisati
void printMatrix(int n, int AdjMatrix[][n])
{
	int i,j;

	for( i = 0; i < n; i++)
		{
			
			for(j = 0; j < n; j++)
				printf("%d ",AdjMatrix[i][j]);

			printf("\n");
		}

}
//funkcija koja u grafu trazi najmanji
int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[], GRAD gradovi[])
{
	int min = INT_MAX;
	int min_index;
	int i;
	for (i = 0; i < n; i++)
		if (poseceni[i] == 0 && udaljenost[i] <= min )
		{
			min = udaljenost[i];
			min_index = i;
		}

	return min_index;
}
//inicijalizuje pocetni cvor u grafu a sve ostalo stavlja na inf(INT_MAX)
void initSingleSource(int n, int udaljenost[], int poseceni[], int pocetni)
{
	for (int i = 0; i < n; i++)
	{
		udaljenost[i] = INT_MAX;	
		poseceni[i] = 0;	
	}

	udaljenost[pocetni] = 0;
}

void relax(int n, int AdjMatrix[n][n], int udaljenost[], int poseceni[], int min, GRAD gradovi[])
{
	for (int j = 0; j < n; j++)

			if (!poseceni[j] && AdjMatrix[min][j] != 0 && udaljenost[min] != INT_MAX)
				if( udaljenost[min] + AdjMatrix[min][j] < udaljenost[j] )
					if(gradovi[min].nadmorskaVisina > gradovi[j].nadmorskaVisina)
						udaljenost[j] = udaljenost[min] + AdjMatrix[min][j];
}
void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[])
{
	int udaljenost[n];     
	int i, j;
	int poseceni[n];

	
	initSingleSource(n, udaljenost, poseceni, pocetni);

	for (i = 0; i < n - 1; i++)
	{
		//trazi najmanji
		int u = najmanjaUdaljenost(n, udaljenost, poseceni, gradovi);
		//postavlja u nizu vrednost da je posecen
		poseceni[u] = 1;
		//relaksira tj updateuje novu putanju, smanjuje tezinu ako je moguce
		relax(n, AdjMatrix, udaljenost, poseceni, u, gradovi);
	
	}

	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == INT_MAX)
			udaljenost[i] = -1;
		printf("%d -> %d = %d\n",pocetni+1, i+1, udaljenost[i]);
	}

}
