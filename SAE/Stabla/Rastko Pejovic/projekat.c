#include<stdio.h>
#include<stdlib.h>

typedef struct cvor
{
	int inf;
	struct cvor* levi;
	struct cvor* desni;
}CVOR;

void ucitajStablo(CVOR **koren, FILE *fp);
void stampajStablo(CVOR *koren);
int visinaStabla(CVOR* koren);
FILE *safe_fopen(char datoteka[], char mode[], int error);
void IzdvojNivoe(CVOR *koren, CVOR *tekuci, int n, int h);
int udaljenostOdKorena(CVOR *koren, int v);
int main()
{
	CVOR *koren = NULL;
	FILE *fp;
	int n;
	int h;
	fp = safe_fopen("stablo.txt","r",1);
	ucitajStablo(&koren, fp);

	h = visinaStabla(koren);
	printf("h = %d \n",h);
	printf("Unesite broj nivoa:\n");
	scanf("%d",&n);

	IzdvojNivoe(koren,koren, n, h);
	


	fclose(fp);
	return 0;
}
FILE *safe_fopen(char datoteka[], char mode[], int error)
{
	FILE *fp = fopen(datoteka, mode);
	if(fp == NULL)
	{
		exit(error);
	}

	return fp;
}

void ucitajStablo(CVOR** koren, FILE *fp)
{
	int n, m, i, j;
	fscanf(fp, "%d", &n);
	fscanf(fp, "%d", &m);
	int nizDece[m];
	CVOR* nizCvorova[n];
	for(i = 0; i < n; i++)
	{
		nizCvorova[i] = (CVOR*)malloc(sizeof(CVOR));
		nizCvorova[i]->inf = i+1;
		nizCvorova[i]->levi = NULL;
		nizCvorova[i]->desni = NULL;
	}
	for(i = 0; i < m; i++)
	{
		int a, b, c;
		fscanf(fp,"%d %d %d", &a, &b, &c);
		nizDece[i] = b;
		if(c == 0)
			nizCvorova[a-1]->levi = nizCvorova[b-1];
		else
			nizCvorova[a-1]->desni = nizCvorova[b-1];
	}
	for(i = 0; i < n; i++)
	{
		int dete = 0;
		for(j = 0; j < m; j++)
		{
			if(nizCvorova[i]->inf == nizDece[j])
			{
				dete = 1;
			}
		}
		if(!dete)
		{
			*koren = nizCvorova[i];
			return;
		}
	}
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
	if(koren == NULL)
		return 0;
	int levoPodstablo = visinaStabla(koren->levi);
	int desnoPodstablo = visinaStabla(koren->desni);

	if(levoPodstablo > desnoPodstablo)
		return levoPodstablo + 1;
	else
		return desnoPodstablo + 1;	
}
void IzdvojNivoe(CVOR *koren, CVOR *tekuci, int n, int h)
{
	if(tekuci != NULL)
	{
		IzdvojNivoe(koren,tekuci->levi, n, h);

		int temp = udaljenostOdKorena(koren, tekuci->inf);
		if( (h-temp) <= n)
			printf("Cvor %d se nalazi u %d poslednja nivoa\n",tekuci->inf,n);
		
		IzdvojNivoe(koren,tekuci->desni, n, h);
	}
}
int udaljenostOdKorena(CVOR *koren, int v)
{
	if(koren == NULL)
		return -1;
	
		int n = -1;
		if(koren->inf == v)
			return n + 1;
		int levi = udaljenostOdKorena(koren->levi, v);
		if(levi >= 0)
			return levi + 1;

		int desni = udaljenostOdKorena(koren->desni,v);
		if(desni>=0)
			return desni+1;
	return n;
}










