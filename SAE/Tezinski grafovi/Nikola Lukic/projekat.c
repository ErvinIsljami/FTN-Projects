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
void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[], int k);
int leastCost(int n, int cost[], int visited[]);
void initSingleSource(int n, int cost[], int visited[], int pocetni);
void relax(int n, int AdjMatrix[n][n], int cost[], int visited[], int min);



int main()
{
	FILE *in = fopen("graf.txt","r");
	int n, v, k;
	fscanf(in,"%d",&n);
	int AdjMatrix[n][n];
	GRAD gradovi[n];

	ucitajGraph(gradovi, in, n, AdjMatrix);
	//printMatrix(n, AdjMatrix);
	printf("unesite broj stanovnika: \n");
	scanf("%d", &k);
	dijkstra(n, AdjMatrix, 0, gradovi, k);

	return 0;
}
void ucitajGraph(GRAD gradovi[], FILE* in, int n, int AdjMatrix[n][n])
{
	int a,b,c,i,j;
	int m;
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			AdjMatrix[i][j] = 0;
	
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%s %d %d", gradovi[i].ime, &gradovi[i].brojStanovnika, &gradovi[i].nadmorskaVisina);
		//printf("%d: %s %d %d\n",i+1, gradovi[i].ime, gradovi[i].brojStanovnika, gradovi[i].nadmorskaVisina);
	}
	fscanf(in,"%d",&m);
	
	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF || --m != 0)
	{
		AdjMatrix[a-1][b-1] = c;
		AdjMatrix[b-1][a-1] = c;
	}
}
//mozes je obrisati
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
int leastCost(int n, int cost[], int visited[])
{
	int min = INT_MAX;
	int min_index;
	int i;
	for (i = 0; i < n; i++)
		if (visited[i] == 0 && cost[i] <= min)
		{
			min = cost[i];
			min_index = i;
		}

	return min_index;
}
void initSingleSource(int n, int cost[], int visited[], int pocetni)
{
	for (int i = 0; i < n; i++)
	{
		cost[i] = INT_MAX;		//najveci broj koji se moze zapisati u intu
		visited[i] = 0;	
	}

	cost[pocetni] = 0;
}

void relax(int n, int AdjMatrix[n][n], int cost[], int visited[], int min)
{
	for (int j = 0; j < n; j++)

			if (!visited[j] && AdjMatrix[min][j] != 0 && cost[min] != INT_MAX)
				if( cost[min] + AdjMatrix[min][j] < cost[j] )
					cost[j] = cost[min] + AdjMatrix[min][j];
}
void dijkstra(int n, int AdjMatrix[n][n], int pocetni, GRAD gradovi[], int k)
{
	int cost[n];     
	int i, j;
	int visited[n];

	initSingleSource(n, cost, visited, pocetni);

	for(i = 0; i < n; i++)
	{
		if(gradovi[i].brojStanovnika < k)
		for(j = 0; j < n; j++)
		{
				AdjMatrix[i][j] = 0;
				AdjMatrix[j][i] = 0;
		}
	}




	for (i = 0; i < n - 1; i++)
	{
		int u = leastCost(n, cost, visited);

		visited[u] = 1;

		relax(n, AdjMatrix, cost, visited, u);
	
	}

	for(i = 0; i < n;i++)
	{
		if(cost[i] == INT_MAX)
			cost[i] = -1;
		printf("%d -> %d = %d\n",pocetni+1, i+1, cost[i]);
	}

}
