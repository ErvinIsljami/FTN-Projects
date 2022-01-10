//FIFO struktura
#include <stdio.h>
#include <stdlib.h>
typedef struct osoba_st
{
	char ime[20];
	char prezime[20];
	int godinaRodjenja;
}OSOBA;

typedef struct red_st
{
    OSOBA osoba;
    struct red_st* sledeci;
}RED;

void enqueue(RED** glava, OSOBA novi);  //dodavanje na kraj
int dequeue(RED **glava, OSOBA *ret);  //brisanje sa pocetka
void stampajRed(RED* glava);
void clear(RED** glava);
void ucitajOsobe(RED** glava, FILE *in);
void opcije(RED **glava);

int main()
{
    RED* glava = NULL;	//red(glava)
    FILE *in = fopen("osobe.txt", "r");	//otvaramo fajl za citanje

    ucitajOsobe(&glava, in);	//ucitavamo osobe iz fajla
   
    opcije(&glava);		//stampa 5 izbora za korisnika
    clear(&glava);		//brise red kad pre zavrsetka programa, cisti memoriju
    fclose(in);
    return 0;
}
void ucitajOsobe(RED** glava, FILE *in)
{
	int n;
	int i;
	OSOBA pom;
	fscanf(in,"%d",&n);	//broj osoba
	for(i = 0; i < n; i++) 
	{
		//cita n osoba iz fajla i dodaje ih u red sa enqueue
		fscanf(in,"%s %s %d", pom.ime, pom.prezime, &pom.godinaRodjenja);
		enqueue(&(*glava), pom);
	}
}
//dodavanje elemnta u red na kraj
void enqueue(RED** glava, OSOBA nova)
{
    RED* novi;
    novi = (RED*)malloc(sizeof(RED));
    novi->osoba = nova;
    novi->sledeci = NULL;

    if(*glava == NULL)
    {
        *glava = novi;	
    }
    else
    {
        RED *temp;
        temp = *glava;

        while(temp->sledeci != NULL)
        {
            temp = temp->sledeci;
        }

        temp->sledeci = novi;
    }
}
//brise prvog
int dequeue(RED **glava, OSOBA *ret)
{	
	//povratna vrednost nam govori da li je uspesno odradio uzimanje prvog elementa iz reda
	//a u ret smesti tu osobu kao povratnu 
    if(*glava != NULL)
    {
        RED* pom = *glava;
     	*ret = pom->osoba;
        *glava = pom->sledeci;
        free(pom);
        return 1;
    }
    else
    return 0;
}

void stampajRed(RED* glava)
{
    RED* temp = glava;
    if(temp == NULL)
    {
        printf("Red je prazan\n");
    }
    else
    {
        while(temp != NULL)
        {
            printf("%s %s %d\n", temp->osoba.ime, temp->osoba.prezime, temp->osoba.godinaRodjenja);
            temp = temp->sledeci;
        }
    }
}

void clear(RED** glava)
{
    if(*glava == NULL)
    {
        printf("Red je vec prazan\n");
    }
    else
    {
        while(*glava != NULL)
        {
        	OSOBA o;
            dequeue(&(*glava), &o);
            //OSOBA o nam nigde ne treba, tj necemo je koristiti
            //ali je moramo zapisati jer nam funkcija trazi u parametrima
        }
    }
}

void opcije(RED **glava)
{
	int i;
	do
	{
		printf("Unesite izbor: \n");
		printf("1. Ipis prvog iz niza.\n");
		printf("2. Brisanje celog reda.\n");
		printf("3. Ispis svih osoba iz reda cekanja.\n");
		printf("4. Dodavanje nove osobe.\n");
		printf("5. Izlaz.\n");
		scanf("%d",&i);

		if(i == 1)
		{
			OSOBA pom;
			int uspesno = dequeue(&(*glava), &pom);
			if(uspesno == 1)
				printf("%s %s %d\n", pom.ime, pom.prezime, pom.godinaRodjenja);
		
		}else 
		if (i == 2)
		{
			clear(&(*glava));

		}else
		if(i == 3)
		{
			stampajRed(*glava);
		}
		else
		if(i==4)
		{
			OSOBA nova;
			printf("Unesite ime:\n");
			scanf("%s", nova.ime);
			printf("Unesite prezime:\n");
			scanf("%s", nova.prezime);
			printf("Unesite godiste\n");
			scanf("%d", &nova.godinaRodjenja);

			enqueue(&(*glava), nova);
		}
	}while(i != 5);
}