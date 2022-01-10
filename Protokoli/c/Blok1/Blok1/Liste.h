#include <stdio.h>
#include <stdlib.h>
#pragma warning(disable:4996)
typedef struct Knjiga_st
{
	char naslov[30];
	char autor[30];
	int brStrana;
	float cena;
}Knjiga;

typedef struct Cvor_st
{
	Knjiga podatak;
	struct Cvor_st* sledeci;
}CVOR;

//unos elemenata i sklanjanje(pop, dequeue, pristupi kod reda i steka)
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
void PushFront(CVOR** glava, Knjiga k) //stavlja element na pocetak liste
{
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->sledeci = NULL;
	novi->podatak = k;

	CVOR* pom = (*glava);	//zapamtim prvi element

	novi->sledeci = pom;
	*glava = novi;
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
void PopFront(CVOR** glava)		//brise element sa pocetka
{
	if (*glava == NULL)
	{
		return;
	}
	CVOR* pom = *glava;
	*glava = pom->sledeci;
	free(pom);
}
Knjiga Peak(CVOR* glava)			//vraca poslednji element
{
	CVOR* pom = glava;
	if (pom == NULL)
	{
		return;
	}
	while (pom->sledeci != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		pom = pom->sledeci;
	}
	//nakon while-a pom je pok na poslednji element
	return pom->podatak;
}
Knjiga Pop2(CVOR** glava)			//vraca poslednji element i brise ga iz liste
{
	Knjiga ret;
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
	ret.cena = pom->podatak.cena;
	ret.brStrana = pom->podatak.brStrana;
	strcpy(ret.autor, pom->podatak.autor);
	strcpy(ret.naslov, pom->podatak.naslov);

	ret = pom->podatak; //moze i ovako, ne preporucujem

	free(pom);
	prethodni->sledeci = NULL;

	return ret;
}
void Enqueue(CVOR** glava, Knjiga k)			//stavlja element na pocetak reda
{
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->sledeci = NULL;
	novi->podatak = k;

	CVOR* pom = (*glava);	//zapamtim prvi element

	novi->sledeci = pom;
	*glava = novi;
}
void Dequeue(CVOR** glava)			//brise element sa kraja liste
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
Knjiga Dequeue2(CVOR** glava)		//vraca poslednji element i brise ga sa reda
{
	Knjiga ret;
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
	ret.cena = pom->podatak.cena;
	ret.brStrana = pom->podatak.brStrana;
	strcpy(ret.autor, pom->podatak.autor);
	strcpy(ret.naslov, pom->podatak.naslov);

	ret = pom->podatak; //moze i ovako, ne preporucujem

	free(pom);
	prethodni->sledeci = NULL;

	return ret;
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
void InsertEl(CVOR** glava,Knjiga k, int index)
{
	CVOR* prethodni = *glava;
	CVOR* pom = prethodni->sledeci;
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->podatak = k;
	novi->sledeci = NULL;
	if (pom == NULL)
	{
		return;
	}
	int i = 0;
	if (index == 0)
	{
		novi->sledeci = (*glava);
		*glava = novi;
		return;
	}
	i++;
	while (pom != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		if (i == index)
		{
			novi->sledeci = pom;
			prethodni->sledeci = novi;
			return;
		}

		pom = pom->sledeci;
		prethodni = prethodni->sledeci;
		i++;
	}
}
//Stampanje liste
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
Knjiga UnesiNovuKnjigu()
{
	Knjiga ret;
	printf("Unesite knjigu: \n");
	printf("Unesite naslov knjige: ");
	scanf("%s", ret.naslov);
	printf("Unesite autora knjige: ");
	scanf("%s", ret.autor);
	printf("Unesite broj strana: ");
	scanf("%d", &ret.brStrana);
	printf("Unesite cenu knjige: ");
	scanf("%f", &ret.cena);

	return ret;
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
//pretraga i brisanje
Knjiga SearchByIndex(CVOR* glava, int index)
{
	CVOR* pom = glava;
	if (pom == NULL)
	{
		return;
	}
	int i = 0;
	while (pom != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		i++;
		if (i == index)
			break;

		pom = pom->sledeci;
	}
	if (i < index)
	{
		printf("Prekoracili ste index.\n");
		return;
	}
	return pom->podatak;
}
Knjiga SearchByNaslov(CVOR* glava, char* naslov)
{
	CVOR* pom = glava;
	if (pom == NULL)
	{
		return;
	}
	while (pom != NULL) //prolazim kroz listu dok ne dodjem do kraja
	{
		if (strcmp(pom->podatak.naslov, naslov) == 0)
		{
			return pom->podatak;
		}
		pom = pom->sledeci;
	}
	
	return;
}
void RemoveByNaslov(CVOR** glava, char* naslov)
{
	CVOR* prethodni = *glava;
	if (prethodni == NULL) //ako je prazna lista
	{
		return;
	}
	CVOR* pom = prethodni->sledeci;

	if (strcmp(prethodni->podatak.naslov, naslov) == 0)
	{
		*glava = pom;	//glava je drugi element
		free(prethodni);	//brisem prvi element
		return;
	}

	while (pom != NULL)
	{
		if (strcmp(pom->podatak.naslov, naslov) == 0)
		{
			prethodni->sledeci = pom->sledeci;	//glava je drugi element
			free(pom);	//brisem prvi element
			return;
		}
	}

	return;
}

//Serijalizacija i deserijalizacija
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
	char* buffer = (char*)malloc(len);
	CVOR* pom = glava;

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