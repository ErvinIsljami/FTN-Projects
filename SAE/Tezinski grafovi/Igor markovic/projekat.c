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

void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[], int v);
int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[], GRAD gradovi[], int v);
void initSingleSource(int n, int udaljenost[], int poseceni[], int pocetni);
void relax(int n, int AdjMatrix[n][n], int udaljenost[], int poseceni[], int min);



int main()
{
	FILE *in = fopen("graf.txt","r");
	int n, v;
	fscanf(in,"%d",&n);
	int AdjMatrix[n][n];
	GRAD gradovi[n];

	ucitajGraph(gradovi, in, n, AdjMatrix);

	//printMatrix(n, AdjMatrix);
	printf("Unesite nadmorsku visinu");
	scanf("%d",&v);
	dijkstra(n, AdjMatrix, 0, gradovi, v);

	return 0;
}
void ucitajGraph(GRAD gradovi[], FILE* in, int n, int AdjMatrix[n][n])
{
	int a,b,c,i,j;
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			AdjMatrix[i][j] = 0;
	
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%s %d %d", gradovi[i].ime, &gradovi[i].brojStanovnika, &gradovi[i].nadmorskaVisina);
	//	printf("%s %d %d\n",gradovi[i].ime, gradovi[i].brojStanovnika, gradovi[i].nadmorskaVisina);		
	}

	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		AdjMatrix[a-1][b-1] = c;
		//zakomentarisati ako je usmereni, usmereni - jedan pravac
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
int najmanjaUdaljenost(int n, int udaljenost[], int poseceni[], GRAD gradovi[], int v)
{
	int min = INT_MAX;
	int min_index;
	int i;
	for (i = 0; i < n; i++)
		if (poseceni[i] == 0 && udaljenost[i] <= min && gradovi[i].nadmorskaVisina < v)
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
		udaljenost[i] = INT_MAX;		//najveci broj koji se moze zapisati u intu
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
void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[], int v)
{
	int udaljenost[n];     
	int i, j;
	int poseceni[n];

	initSingleSource(n, udaljenost, poseceni, pocetni);

	for (i = 0; i < n - 1; i++)
	{
		int u = najmanjaUdaljenost(n, udaljenost, poseceni, gradovi, v);

		poseceni[u] = 1;

		relax(n, AdjMatrix, udaljenost, poseceni, u);
	
	}

	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == INT_MAX)
			udaljenost[i] = -1;
		printf("%d -> %d = %d\n",pocetni+1, i+1, udaljenost[i]);
	}

}
