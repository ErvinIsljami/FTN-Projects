#include<stdio.h>
#include<stdlib.h>

typedef struct telefon_st
{
	int IMEI;
	char brojTelefona[12];
	char vrstaTelefona[15];
}TELEFON;


void ucitajGraph(TELEFON telefoni[], FILE* in, int n, int AdjMatrica[n][n]);
void printMatrix(int n, int AdjMatrica[n][n]);

void dijkstra(int n, int AdjMatrica[n][n], int pocetni, TELEFON telefoni[], char operater[]);
int najmanjeKostanje(int n, int kostanja[], int visited[], TELEFON telefoni[], char operater[]);
void initilizeSingleSource(int n, int kostanja[], int visited[], int pocetni);
void relax(int n, int AdjMatrix[n][n], int udaljenost[], int visited[], int min, TELEFON telefoni[], char operater[]);
int uporediOperatere(char operater[], char brojTelefona[]);

int main()
{
	int n, v;
	char operater[] = "060";
	FILE *in = fopen("graf.txt","r");
	fscanf(in,"%d",&n);
	int AdjMatrica[n][n];
	TELEFON telefoni[n];

	ucitajGraph(telefoni, in, n, AdjMatrica);
//	printMatrix(n, AdjMatrica);

	dijkstra(n, AdjMatrica, 0, telefoni, operater);



	return 0;
}
void ucitajGraph(TELEFON telefoni[], FILE* in, int n, int AdjMatrica[n][n])
{
	int i, j;
	for( i = 0; i < n; i++)
		for(j = 0; j < n; j++)
			AdjMatrica[i][j] = 0;
	
	for(i = 0; i < n; i++)
	{
		fscanf(in,"%d %s %s", &telefoni[i].IMEI, telefoni[i].brojTelefona, telefoni[i].vrstaTelefona);
	//	printf("%d %s %s\n", telefoni[i].IMEI, telefoni[i].brojTelefona, telefoni[i].vrstaTelefona);	
	}
	int a;
	int b;
	int c;
	while( (fscanf(in,"%d %d %d", &a, &b, &c)) != EOF)
	{
		AdjMatrica[a-1][b-1] = c;
		AdjMatrica[b-1][a-1] = c;
	}
}
//nije potrebna funkcija, mozes obrisati
void printMatrix(int n, int AdjMatrica[n][n])
{
	int i,j;
	for( i = 0; i < n; i++)
		{
			
			for(j = 0; j < n; j++)
				printf("%d ",AdjMatrica[i][j]);

			printf("\n");
		}
}

int uporediOperatere(char operater[], char brojTelefona[])
{
	for(int i = 0; i < 3; i++)
		if(operater[i] != brojTelefona[i])
			return 0;

	return 1;
}
int najmanjeKostanje(int n, int kostanja[], int visited[], TELEFON telefoni[], char operater[])
{
	int min = 1000;
	int min_index;
	int i;
	for (i = 0; i < n; i++)
	{
		//int jednaki = uporediOperatere(operater, telefoni[i].brojTelefona);
		if (visited[i] == 0 && kostanja[i] <= min )
		{
			min = kostanja[i];
			min_index = i;
		}
	}
	return min_index;
}
void initilizeSingleSource(int n, int udaljenost[], int visited[], int pocetni)
{
	for (int i = 0; i < n; i++)
	{
		udaljenost[i] = 1000;	//neki jako veliki broj da se inicijalizuju pocetne vrednosti
		visited[i] = 0;	
	}

	udaljenost[pocetni] = 0;
}

void relax(int n, int AdjMatrix[n][n], int udaljenost[], int visited[], int min, TELEFON telefoni[], char operater[])
{
	//puts("fasdf");
	for (int i = 0; i < n; i++)
	{
			if (!visited[i] && AdjMatrix[min][i] != 0 && udaljenost[min] != 1000)
				if( udaljenost[min] + AdjMatrix[min][i] < udaljenost[i] )
					if(uporediOperatere(operater, telefoni[i].brojTelefona))
						udaljenost[i] = udaljenost[min] + AdjMatrix[min][i];
	
	}
}
void dijkstra(int n, int AdjMatrica[n][n], int pocetni, TELEFON telefoni[], char operater[])
{
	int udaljenost[n];     
	int i, j;
	int visited[n];

	initilizeSingleSource(n, udaljenost, visited, pocetni);

	for (i = 0; i < n - 1; i++)
	{
		int u = najmanjeKostanje(n, udaljenost, visited, telefoni, operater);

		visited[u] = 1;

		relax(n, AdjMatrica, udaljenost, visited, u, telefoni, operater);
	
	}

	//stampanje svih rezulata
	for(i = 0; i < n;i++)
	{
		if(udaljenost[i] == 1000)
			udaljenost[i] = -1;
		printf("%d -> %d = %d\n",pocetni+1, i+1, udaljenost[i]);
	}

}