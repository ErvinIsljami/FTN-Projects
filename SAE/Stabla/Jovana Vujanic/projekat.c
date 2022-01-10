#include<stdio.h>
#include<stdlib.h>

typedef struct cvor
{
	int inf;
	struct cvor* levi;
	struct cvor* desni;
}CVOR;

int ucitajStablo(CVOR **koren);
void stampajStablo(CVOR *koren);
void brojUnutrasnjihCvorova(CVOR *koren, int *listova);

int main()
{
	CVOR *koren = NULL;
	int n = ucitajStablo(&koren);
	int listova = 0;

	brojUnutrasnjihCvorova(koren, &listova);
	printf("Broj listova = %d\n",listova);
	printf("Broj unutrasnjih cvorova = %d\n", n - listova);	//ako se koren ne racuna -1
	


	return 0;
}

int ucitajStablo(CVOR** koren)
{
	FILE *in = fopen("stablo.txt", "r");
	int n, m, i, j;
	fscanf(in, "%d", &n);
	fscanf(in, "%d", &m);
	CVOR* pomNiz[n];
	int a[m];
	int b[m];
	int c[m];
	for(i = 0; i < n; i++)
	{
		pomNiz[i] = (CVOR*)malloc(sizeof(CVOR));
		pomNiz[i]->levi = NULL;
		pomNiz[i]->desni = NULL;
		pomNiz[i]->inf = i+1;
	}
	for(i = 0; i < m; i++)
	{
		fscanf(in,"%d %d %d", &a[i], &b[i], &c[i]);
//ako je c nula onda je sa leve strane
//ako je c jedinica onda je sa desne strane
		if(c[i] == 0)
			pomNiz[a[i]-1]->levi = pomNiz[b[i]-1];
		else
			pomNiz[a[i]-1]->desni = pomNiz[b[i]-1];
	}
	for(i = 0; i < n; i++)
	{
		int isKoren = 1;
		for(j = 0; j < m; j++)
		{
			if(pomNiz[i]->inf == b[j])
			{
			
				isKoren = 0;
			}
		}
		if(isKoren)
		{
			*koren = pomNiz[i];
			break;
		}
	}
	fclose(in);
	return n;
}

void stampajStablo(CVOR *koren)
{
	if(koren != NULL)
	{
		stampajStablo(koren->levi);
		printf("%d\n",koren->inf);
		stampajStablo(koren->desni);
	}
}

void brojUnutrasnjihCvorova(CVOR *koren, int *listova)
{
	if(koren != NULL)
	{
		if(koren->levi == NULL && koren->desni == NULL)
		{
			printf("List: %d\n", koren->inf);
			(*listova)++;
		}

		brojUnutrasnjihCvorova(koren->levi, listova);
		brojUnutrasnjihCvorova(koren->desni, listova);
	}
}