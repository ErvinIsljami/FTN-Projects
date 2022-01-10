#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct cvor_st
{
	char brIndeksa[12];
	int poeni;
	struct cvor_st* sledeci;
}CVOR;

void zadatak1(FILE *in, CVOR **recnik);
int getHashCode(char brIndeksa[]);
void dodajUListu(CVOR **glava, char brIndeksa[], int poeni);
void stampajRecnik(CVOR** recnik);
void zadatak2(FILE* in, CVOR **recnik);
void pretragaPoKljucu(CVOR** recnik, char brIndeksa[]);
void zadatak3(FILE *in, CVOR **recnik);
void brisiIzRecnika(CVOR **recnik, char brIndeksa[]);
void clearMemory(CVOR** recnik);
int main()
{
	FILE *in1, *in2, *in3;
	//otvaramo datoteke
	in1 = fopen("bodovi1.txt","r");
	in2 = fopen("bodovi2.txt","r");
	in3 = fopen("bodovi3.txt","r");
	int i;
	//zauzimamo memoriju za recnik, posto je niz listi moramo dinamicki zauzeti
	CVOR** recnik = (CVOR**)malloc(5*sizeof(CVOR*));
	for(i = 0; i < 5; i++)
		recnik[i] = NULL;

	//pokrecemo zadatak 1
	zadatak1(in1,recnik);
	//stampajRecnik(recnik);
	zadatak2(in2, recnik);


	zadatak3(in3, recnik);

	stampajRecnik(recnik);
	//cistimo memoriju za nama
	clearMemory(recnik);
	return 0;
}
void stampajRecnik(CVOR** recnik)
{
	int i;
	for(i = 0; i < 5; i++)
	{
		CVOR* lista = recnik[i];
		printf("hash = %d\n", i);
		//stampamo hesh vrednost i sve koji su u listi za tu hesh vrednost
		while(lista != NULL)
		{	
			printf("[brIndeksa: %s poeni: %d]\n",lista->brIndeksa, lista->poeni);
			lista = lista->sledeci;
		}
	}
}
void zadatak1(FILE* in, CVOR** recnik)
{
	char brIndeksa[12];
	int poeni;

	//ucitava iz fajla sve dok ne dodje do EOF(End Of File)
	//ucitava do kraja kolko god studenata da ima
	while( fscanf(in, "%s %d", brIndeksa, &poeni) != EOF)
	{
		int hash = getHashCode(brIndeksa);
		//za svakog studenta izracuna hesh funkciju i doda ga u listu koja mu je predodredjena
		//za svaku hesh vrednost postoji posebna lista
		dodajUListu( &recnik[hash], brIndeksa, poeni);
	}
}

void dodajUListu(CVOR **glava, char brIndeksa[], int poeni)
{
	CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
	novi->poeni = poeni;
	strcpy(novi->brIndeksa, brIndeksa);
	novi->sledeci = NULL;
	//proverava da li je glava prazna, ako jeste onda doda na glavu
	if(*glava == NULL)
	{
		*glava = novi;
	}
	else
	{	//ako glava nije prazna onda ide na kraj 
		CVOR* tekuci = *glava;
		while(tekuci->sledeci != NULL)
		{
			tekuci = tekuci->sledeci;
		}
		tekuci->sledeci = novi;
	}
}

int getHashCode(char brIndeksa[])
{	//funkcija koja racuna hesh vrednost za broj indeksa i vraca je
	int ret;
	if(brIndeksa[0] == 'E' && brIndeksa[1] == 'E')
		ret = 1;
	else
	if(brIndeksa[0] == 'R' && brIndeksa[1] == 'A')
		ret = 2;
	else
	if(brIndeksa[0] == 'P' && brIndeksa[1] == 'R')
		ret = 3;
	else
	if(brIndeksa[0] == 'S' && brIndeksa[1] == 'F')
		ret = 4;
	else
		ret = 0;

	return ret;
}
void zadatak2(FILE* in, CVOR **recnik)
{
	char brIndeksa[12];
	int poeni;

	while( fscanf(in, "%s", brIndeksa) != EOF)
	{
		//za svaki ucitani broj indeksa iz fajla pretrazuje ga u recniku
		pretragaPoKljucu(recnik, brIndeksa);
	}
}
void pretragaPoKljucu(CVOR** recnik, char brIndeksa[])
{
	//pretrazuje ga u recniku i proverava da li je tu
	int hash = getHashCode(brIndeksa);
	CVOR* lista = recnik[hash];
	int nasao = 0;
	while(lista != NULL)
	{	//ako se nalazi u recniku stampamo ga
		if(strcmp(lista->brIndeksa, brIndeksa) == 0)
		{
			printf("[%s, %d]\n",brIndeksa, lista->poeni);
			nasao = 1;
			break;
		}
		lista = lista->sledeci;
	}
	//ako se ne nalazi u recniku ispisemo gresku
	if(!nasao)
		printf("Student sa brojem indeksa %s nije u recniku\n", brIndeksa);
}
void zadatak3(FILE *in, CVOR **recnik)
{
	char brIndeksa[12];
	
	while( fscanf(in, "%s", brIndeksa) != EOF)
	{
		//iz fajla 3 cita jedan po jedan indeks i brise ga ako se nalazi u recniku
		brisiIzRecnika(&(*recnik), brIndeksa);
	}
}
void brisiIzRecnika(CVOR **recnik, char brIndeksa[])
{	
	//nalazi hesh vrednost za kljuc koji zelimo da brisemo
	//pristupamo bas onoj listi koju nam vrati hesh funkcija
	int hash = getHashCode(brIndeksa);
	CVOR* glava = recnik[hash];

	if(glava != NULL)
	{
		CVOR *temp = glava;
		//kada ga nadjemo obrisemo ga
		//prvo proveravamo da li je gava ona koju treba da brisemo
		if(strcmp(glava->brIndeksa, brIndeksa) == 0)
		{
			recnik[hash] = temp->sledeci;
			free(temp);
		}
		else
		{
			temp = glava->sledeci;
			//ako nije glava prolazimo kroz listu i nalazimo element koji treba da brisemo
			while(temp != NULL)
			{
				if(strcmp(temp->brIndeksa, brIndeksa) == 0)
				{
					glava->sledeci = temp->sledeci;
					free(temp);
					break;
				}
				glava = glava->sledeci;
				temp = temp->sledeci;	
			}
		}

	}

}
void clearMemory(CVOR** recnik)
{
	int i;
	for(i = 0; i < 5; i++)
	{
		CVOR* glava = recnik[i];
		while(glava != NULL)
		{
			CVOR* pom = glava;
			glava = pom->sledeci;
			free(pom);
		}
	}
}