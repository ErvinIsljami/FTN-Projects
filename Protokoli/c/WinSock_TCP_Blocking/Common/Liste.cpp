#include "Liste.hpp"

void InitList(CVOR** glava)
{
	*glava = NULL;
}


void Push(CVOR** glava, Knjiga k)	//stavlja element na kraj liste
{
	CVOR* pom = *glava;
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->sledeci = NULL;
	novi->podatak = k;
	if (*glava == NULL) //prvi element
	{
		*glava = novi;
		return;
	}
	while (pom->sledeci != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		pom = pom->sledeci;
	}
	//nakon while-a pom je pok na poslednji element
	pom->sledeci = novi;
}

void Pop(CVOR** glava)				//brise element sa kraja
{
	CVOR* prethodni = *glava;
	if (prethodni == NULL)	//prazna lista
	{
		return;
	}
	CVOR* pom = prethodni->sledeci;
	if (pom == NULL)
	{
		free(prethodni);
		*glava = NULL;
	}
	while (pom->sledeci != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		prethodni = pom;
		pom = pom->sledeci;
	}
	free(pom);
	prethodni->sledeci = NULL;
}


void ClearList(CVOR** glava)
{
	CVOR* pom = *glava;
	while (pom != NULL)
	{
		*glava = pom->sledeci;
		free(pom);
		pom = *glava;
	}
}

void PrintList(CVOR* glava)
{
	CVOR* pom = glava;
	printf("Lista: \n");
	while (pom != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		printf("Knjiga: %s %s %d %.2f\n", pom->podatak.naslov, pom->podatak.autor, pom->podatak.brStrana, pom->podatak.cena);

		pom = pom->sledeci;
	}
}

Knjiga NapraviNovuKnjigu(char* naslov, char* autor, int brStr, float cena)
{
	Knjiga ret;
	ret.cena = cena;
	ret.brStrana = brStr;
	strcpy(ret.autor, autor);
	strcpy(ret.naslov, naslov);

	return ret;
}

int CalculateLen(CVOR* glava)
{
	int len = 0;

	CVOR* pom = glava;
	while (pom != NULL)
	{
		len += sizeof(pom->podatak);

		pom = pom->sledeci;
	}

	return len;
}
char* Serialize(CVOR* glava)
{
	int len = CalculateLen(glava);
	CVOR* pom = glava;
	char* buffer = (char*)malloc(len);

	int i = 0;
	while (pom != NULL)
	{
		memcpy(buffer + i, &pom->podatak, sizeof(pom->podatak));
		i += sizeof(Knjiga);
		pom = pom->sledeci;
	}

	return buffer;
}
void Deserialize(CVOR** glava, char* data, int len)
{
	Knjiga* nizKnjiga = (Knjiga*)data;
	len /= sizeof(Knjiga);
	for (int i = 0; i < len; i++)
	{
		Push(&(*glava), nizKnjiga[i]);
	}
}



