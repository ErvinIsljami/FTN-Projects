#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<limits.h>

typedef struct podaci_st
{
	char ime[20];
	char prezime[20];
	int godina;
}PODACI;

void ucitajGraph(PODACI korisnici[], FILE* in, int n, int AdjMatrix[n][n]);
void printMatrix(int n, int AdjMatrix[n][n]);

void dijkstra(int n, int AdjMatrix[n][n], int pocetni);
int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[]);
void initSingleSource(int n, int udaljenost[], int poseceni[], int pocetni);
void relax(int n, int AdjMatrix[n][n], int udaljenost[], int poseceni[], int min);


int main()
{
	int graph[9][9] = {{0, 4, 0, 0, 0, 0, 0, 8, 0},
                      {4, 0, 8, 0, 0, 0, 0, 11, 0},
                      {0, 8, 0, 7, 0, 4, 0, 0, 2},
                      {0, 0, 7, 0, 9, 14, 0, 0, 0},
                      {0, 0, 0, 9, 0, 10, 0, 0, 0},
                      {0, 0, 4, 14, 10, 0, 2, 0, 0},
                      {0, 0, 0, 0, 0, 2, 0, 1, 6},
                      {8, 11, 0, 0, 0, 0, 1, 0, 7},
                      {0, 0, 2, 0, 0, 0, 6, 7, 0}
                     };

	dijkstra(9, graph, 0);


	return 0;
}
void ucitajGraph(PODACI korisnici[], FILE* in, int n, int AdjMatrix[n][n])
{
	int a,b,c,i,j;
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			AdjMatrix[i][j] = 0;
	
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%s %s %d", korisnici[i].ime, korisnici[i].prezime, &korisnici[i].godina);
		printf("%s %s %d\n",korisnici[i].ime, korisnici[i].prezime, korisnici[i].godina);		
	}

	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		AdjMatrix[a-1][b-1] = c;
		AdjMatrix[b-1][a-1] = c;
	}
}

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

int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[])
{
	int min = INT_MAX;
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
void initSingleSource(int n, int udaljenost[], int poseceni[], int pocetni)
{
	for (int i = 0; i < n; i++)
	{
		udaljenost[i] = INT_MAX;
		poseceni[i] = 0;	
	}

	udaljenost[pocetni] = 0;
}

void relax(int n, int AdjMatrix[n][n], int udaljenost[], int poseceni[], int min)
{
	for (int j = 0; j < n; j++)

			if (!poseceni[j] && AdjMatrix[min][j] != 0 && udaljenost[min] != INT_MAX)
				if( udaljenost[min] + AdjMatrix[min][j] < udaljenost[j] )
					udaljenost[j] = udaljenost[min] + AdjMatrix[min][j];
}
void dijkstra(int n, int AdjMatrix[n][n], int pocetni)
{
	int udaljenost[n];     
	int i, j;
	int poseceni[n];

	initSingleSource(n, udaljenost, poseceni, pocetni);

	for (i = 0; i < n - 1; i++)
	{
		int u = najmanjaUdaljenost(n, udaljenost, poseceni);

		poseceni[u] = 1;

		relax(n, AdjMatrix, udaljenost, poseceni, u);
	
	}

	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == INT_MAX)
			udaljenost[i] = -1;
		printf("%d -> %d = %d\n",pocetni, i, udaljenost[i]);
	}

}
