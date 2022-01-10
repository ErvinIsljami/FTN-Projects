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
int visinaStabla(CVOR *koren);
void stampajJedanNivo(CVOR* koren, int level);
void stampajNivoe(CVOR* koren);

int main()
{
	CVOR *koren = NULL;
	int n = ucitajStablo(&koren);
	int listova = 0;
	printf("h = %d\n",visinaStabla(koren));
	stampajNivoe(koren);
	


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
int visinaStabla(CVOR* koren)
{
    if (koren==NULL)
        return 0;
    else
    {
        
        int lheight = visinaStabla(koren->levi);
        int rheight = visinaStabla(koren->desni);
 
        if (lheight > rheight)
            return(lheight+1);
        else 
        	return(rheight+1);
    }
}
void stampajNivoe(CVOR* koren)
{
    int h = visinaStabla(koren);
    int i;
    for (i=1; i<=h; i++)
    {
        stampajJedanNivo(koren, i);
        printf("\n");
    }
}
 
void stampajJedanNivo(CVOR* koren, int nivo)
{
    if (koren == NULL)
        return;
    if (nivo == 1)
        printf("%d ", koren->inf);
    else if (nivo > 1)
    {
        stampajJedanNivo(koren->levi, nivo-1);
        stampajJedanNivo(koren->desni, nivo-1);
    }
}