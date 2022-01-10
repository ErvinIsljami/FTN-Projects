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
int najmanjaUdaljenost(int n, int udaljenost[], int obradjen[]);

void udaljenostPrekoMladjeg(int n, int AdjMatrix[n][n], PODACI korisnici[]);

int main()
{
	FILE *in = fopen("graf.txt","r");
	int n;
	fscanf(in,"%d",&n);
	int AdjMatrix[n][n];
	PODACI korisnici[n];

	ucitajGraph(korisnici, in, n, AdjMatrix);
//	dijkstra(n, AdjMatrix, 0);
	
	udaljenostPrekoMladjeg(n, AdjMatrix, korisnici);

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
	//	printf("%s %s %d\n",korisnici[i].ime, korisnici[i].prezime, korisnici[i].godina);		
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

int najmanjaUdaljenost(int n, int udaljenost[], int obradjen[])
{
	int min = INT_MAX;
	int min_index = -1;
	int i;
	for (i = 0; i < n; i++)
		if (obradjen[i] == 0 && udaljenost[i] <= min)
		{
			min = udaljenost[i];
			min_index = i;
		}

	return min_index;
}
void dijkstra(int n, int AdjMatrix[n][n], int pocetni)
{
	int udaljenost[n];     
	int i, j;
	int obradjen[n];
					
	for (int i = 0; i < n; i++)
	{
		udaljenost[i] = INT_MAX;
		obradjen[i] = 0;	
	}

	//init single source
	udaljenost[pocetni] = 0;	

	for (i = 0; i < n - 1; i++)
	{
		//extract min
		int u = najmanjaUdaljenost(n, udaljenost, obradjen);	//find min

		obradjen[u] = 1;


		for (int j = 0; j < n; j++)
			//relax
			if (!obradjen[j] && AdjMatrix[u][j] != 0 && udaljenost[u] != INT_MAX)
				if( udaljenost[u] + AdjMatrix[u][j] < udaljenost[j] )
					udaljenost[j] = udaljenost[u] + AdjMatrix[u][j];
	
	}

	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == INT_MAX)
			udaljenost[i] = -1;
		printf("%d -> %d = %d\n",pocetni+1, i+1, udaljenost[i]);
	}


}
void udaljenostPrekoMladjeg(int n, int AdjMatrix[n][n], PODACI korisnici[])
{
	int i,j;
	int AdjMatrix2[n][n];
	int p;
	printf("Unesite pocetni cvor\n");
	scanf("%d",&p);

	for(i = 0; i < n; i++)
		for(j = 0; j < n; j++)
	{	
		{
			if(korisnici[i].godina < korisnici[j].godina && AdjMatrix[i][j] > 0)
			{
				AdjMatrix2[i][j] = AdjMatrix[i][j];
			}
			else
				AdjMatrix2[i][j] = 0;
		}
	}
	
	dijkstra(n, AdjMatrix2, p-1);
}