#include <stdio.h>
#include <stdlib.h>

typedef struct CVOR_ST
{
    char vr;
    struct CVOR_ST* sledeci;
}CVOR;

void enqueue(CVOR** glava, char v);  //dodavanje na kraj
char dequeue(CVOR** glava);  //brisanje sa pocetka
void stampajListu(CVOR* glava);
void clear(CVOR** glava);
void ucitajIzFajla(CVOR** glava, FILE *in);
void meni(CVOR **glava);

int main()
{
    CVOR* glava = NULL;
    FILE *in = fopen("karakteri.txt", "r");

    ucitajIzFajla(&glava, in);
    stampajListu(glava);
    meni(&glava);

    return 0;
}
void ucitajIzFajla(CVOR** glava, FILE *in)
{
	char c;
	int n;
	fscanf(in,"%d",&n);
	fscanf(in,"%c",&c);
	while( fscanf(in,"%c",&c) != EOF)
	{
		enqueue(&(*glava), c);
	}
}
void enqueue(CVOR** glava, char c)
{
    CVOR* novi;
    novi = (CVOR*)malloc(sizeof(CVOR));
    novi->vr= c;  
    novi->sledeci = NULL;

    if(*glava == NULL)
    {
        *glava = novi;
    }
    else
    {
        CVOR *tekuci;
        tekuci = *glava;

        while(tekuci->sledeci != NULL)
        {
            tekuci = tekuci->sledeci;
        }

        tekuci->sledeci = novi;
    }
}
char dequeue(CVOR **glava)
{
	char ret;
    if(*glava != NULL)
    {
        CVOR* pom = *glava;
     	ret = pom->vr;
        *glava = pom->sledeci;
        free(pom);
    }
    else
    {
        ret = 0;
    }
    return ret;
}
void stampajListu(CVOR* glava)
{
    CVOR* tekuci = glava;
    if(tekuci == NULL)
    {
        printf("Lista je prazna\n");
    }
    else
    {
        while(tekuci != NULL)
        {
            printf("%c ",tekuci->vr);
            tekuci = tekuci->sledeci;
        }
    }
}

void clear(CVOR** glava)
{
    if(*glava == NULL)
    {
        printf("Red je vec prazan\n");
    }
    else
    {
        while(*glava != NULL)
        {
            dequeue(&(*glava));
        }
    }
}
void meni(CVOR **glava)
{
	int i;
	do
	{
		printf("Unesite izbor: \n");
		printf("1. Uzimanje prvog karaktera.\n");
		printf("2. Brisanje svih karaktera.\n");
		printf("3. Dodavanje novog karaktera.\n");
		printf("4. Prekid.\n");	
		scanf("%d",&i);

		if(i == 1)
		{
			system("clear");
			char c = dequeue(&(*glava));
			printf("Karakter uzet iz reda: %c\n",c);
		}else 
		if (i == 2)
		{
			clear(&(*glava));

		}else
		if(i == 3)
		{
			//moramo da ocistimo baffer tastature jer ostane jedan enter ucitan
			//_fpurge(stdin);
			char c;
			char pom;
			printf("Unesite novi karakter: ");
			scanf("%c %c",&pom,&c);
			enqueue(&(*glava), c);
			system("clear");
		}
		else
		if(i==4)
		{
			int malaSlova = 0;
			int velikaSlova = 0;
			int cifre = 0;
			int ostali = 0;
			char pom;
			while( (pom = dequeue(&(*glava)) ) )
			{
				if(pom >= 'a' && pom <='z')
					malaSlova++;
				else
				if(pom >= 'A' && pom <= 'Z')
					velikaSlova++;
				else
				if(pom >= '0' && pom<= '9')
					cifre++;
				else
					ostali++;
			}
			system("clear");
			printf("Cifara: %d\n",cifre);
			printf("Malih slova: %d\n", malaSlova);
			printf("Velikih slova: %d\n", velikaSlova);
			printf("Ostali znaci: %d\n", ostali);
			return;
		}
		else
		{
			system("clear");
			printf("Nedozvoljen unos!\n");
		}
	}while(1);
}



