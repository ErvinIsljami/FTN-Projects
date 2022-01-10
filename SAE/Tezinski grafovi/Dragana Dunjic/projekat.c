#include<stdio.h>
#include<stdlib.h>

typedef struct racunar
{
	char ime[30];
	char ip_adresa[16];
	int protok;
}RACUNAR;




void ucitajFajl(RACUNAR racunari[], FILE* in, int n, int matrica[n][n])
{
	int i, j;
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%s %s %d", racunari[i].ime, racunari[i].ip_adresa, &racunari[i].protok);
		
	}
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			matrica[i][j] = 0;

	int a;
	int b;
	int c;
	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		matrica[a-1][b-1] = c;
		matrica[b-1][a-1] = c;	//ako je usmeren grafo zakomentarisati ovu liniju
	}
}


int extractMin(int n, int dist[], int visited[])
{
	int min = 10000;
	int min_index = -1;
	int i;
	for (i = 0; i < n; i++)
		if (visited[i] == 0 && dist[i] <= min)
		{
			min = dist[i];
			min_index = i;
		}

	return min_index;
}
void relax(int n, int AdjMatrix[n][n], int dist[], int visited[], int min)
{
	for (int i = 0; i < n; i++)

			if (!visited[i] && AdjMatrix[min][i] != 0 && dist[min] != 10000)
				if( dist[min] + AdjMatrix[min][i] < dist[i] )
					dist[i] = dist[min] + AdjMatrix[min][i];
}
void dijkstra(int n, int AdjMatrix[n][n], int source, RACUNAR racunari[], int pr)
{
	int dist[n];     
	int i, j;
	int visited[n];

	for(i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			if(racunari[i].protok < pr)
				AdjMatrix[i][j] = 0;

	//initilize single source
	for (int i = 0; i < n; i++)
	{
		dist[i] = 10000;	//postavlja prvobitne udaljenosti na beskonacno
		visited[i] = 0;	
	}

	dist[source] = 0;	//udaljenostod pocetnog je 0

	for (i = 0; i < n - 1; i++)
	{
		int min = extractMin(n, dist, visited);	

		visited[min] = 1;

		relax(n, AdjMatrix, dist, visited, min);
	
	}

	printf("Putanje: \n");
	for(i = 0; i < n;i++)
	{
		if(dist[i] == 10000)
			dist[i] = -1;
		printf("od %d do %d = %d\n",source+1, i+1, dist[i]);
	}
}
int main(int brArg, char *args[])
{
	if(brArg != 2)
	{
		printf("greska, uneli ste pogresan broj argumenata");
		exit(1);
	}
	FILE *fp = fopen("graf.txt", "r");
	int n;
	fscanf(fp, "%d", &n);

	RACUNAR racunari[n];
	int AdjMatrix[n][n];
	ucitajFajl(racunari, fp, n, AdjMatrix);
	
	dijkstra(n, AdjMatrix, 0, racunari, atoi(args[1]));

	return 0;
}