#include <stdio.h>
#include <stdlib.h>

#include "hash_file.h"
#include "bucket.h"

#define LEN 30
#define DATA_DIR "data"                    // poseban direktorijum za datoteke
#define LS_CMD "dir "DATA_DIR""            // komanda za prikaz svih datoteka
#define DEFAULT_FILENAME "test.dat"
#define DEFAULT_INFILENAME "in.dat"

int menu();
FILE *safeFopen(char filename[]);
void handleResult(int returnCode);
void handleFindResult(FindRecordResult findResult);
void fromTxtToSerialFile(FILE *pFtxt, FILE *pFbin);

int main()
{
    int running = 1;
    int option = -1;
    FILE *pFile = NULL;
    char filename[20];
    Record record;
    int key;
    int i = 0;
    char csvFilename[100];

    while(running)
    {
        option = menu();

        switch (option) 
        {
            case 1:
                system("cls");
                printf("\nUnesite naziv datoteke: ");
                gets(filename);
                pFile = safeFopen(filename);
                break;

            case 2:
                system("cls");
                printf("\nUnesite naziv datoteke: ");
                gets(filename);
                pFile = openFile(filename);
                break;

            case 3:
                system("cls");
                printf("Trenutno otvorena(aktivna) datoteka: %s.\n", filename);
                break;

            case 4:
                system("cls");
                record = scanRecord(WITH_KEY);
                handleResult(insertToFile(pFile, record));
                break;
            
            case 5:
                system("cls");
                printf("Unesite kljuc rekorda koji zelite da pronadjete.\n");
                scanf("%d", &key);
                handleFindResult(findRecord(pFile, key));
                break;

            case 6:
                system("cls");
                printContent(pFile);
                break;
            
            case 7:
                system("cls");
                printf("Unesite kljuc rekorda kojeg zelite da obrisete.\n");
                scanf("%d", &key);
                handleResult(removeRecord(pFile, key));
                break;
            
            case 8:
                system("cls");
                printf("Unesite kljuc rekorda kojem zelite da promenite duzinu boravka.\n");
                scanf("%d", &key);
                handleResult(updateTime(pFile, key));
                break;

            case 9:
                system("cls");
                printf("Unesite ime csv fajla (sa ekstenzijom) iz kog zelite da importujete rekorde.\n");
                gets(csvFilename);
                importRecordsFromCsv(csvFilename, pFile);
                break;

            case 10:
                system("cls");
                char dateFrom[DATE_LEN];
                char dateTo[DATE_LEN];
                printf("Unesite pocetni datum: ");
                gets(dateFrom);
                printf("Unesite krajnji datum: ");
                gets(dateTo);
                filterRecordsByDate(pFile, dateFrom, dateTo);
                break;
            
            case 0:
                system("cls");
                running = 0;
                puts("Program se zavrsava. Pritisnite enter za izlazak. \n");
                getchar();
                break;
            default:
                break;
        }
    }

    fclose(pFile);
    return 0; 
}

int menu() 
{
    int option;

    puts("\n\nIzaberite opciju:");
    puts("\t1.  Formiranje prazne datoteke.");      //DONE
    puts("\t2.  Izbor aktivne datoteke.");          //DONE
    puts("\t3.  Prikaz naziva aktivne datoteke.");  //DONE
    puts("\t4.  Unos novog sloga.");                //DONE
    puts("\t5.  Trazenje sloga");                   //done
    puts("\t6.  Prikaz sadrzaja datoteke");         //DONE
    puts("\t7.  Logicko brisanje sloga.");          //DONE
    puts("\t8.  Azuriranje duzine boravka.");       //DONE
    puts("\t9.  Importovanje iz .csv fajla.");      //DONE
    puts("\t10. Filtriranje po datumu parkiranja.");//DONE
    puts("\t0. Kraj programa");

    printf(">>");

    scanf("%d", &option);
    getc(stdin);
    return option;
}

FILE *safeFopen(char filename[]) 
{
    FILE *pFile;

    pFile = fopen(filename, "rb+");

    if (pFile == NULL) 
    {                        // da li datoteka sa tim imenom vec postoji
        pFile = fopen(filename, "wb+");     // ako ne, otvara se za pisanje
        createHashFile(pFile);                  // i kreira prazna rasuta organizacija
        puts("Kreirana prazna datoteka.");
    } 
    else 
    {
        puts("Otvorena postojeca datoteka.");   // ako da, koristi se postojeca datoteka
    }

    if (pFile == NULL) 
    {
        printf("Nije moguce otvoriti/kreirati datoteku: %s.\n", filename);
    }

    return pFile;
}

void handleResult(int returnCode) 
{
    if (returnCode < 0) {
        puts("Operacija neuspesna.");
    } else {
        // u zavisnosti od operacija ovde se moze ispisati i
        // detaljnija poruka o razlogu neuspesnosti
        puts("Operacija uspesna.");
    }
}

void handleFindResult(FindRecordResult findResult) {
    if (findResult.ind1 != RECORD_FOUND) {
        puts("Neuspesno trazenje.");
    } else {
        printRecord(findResult.record);
    }
}

void fromTxtToSerialFile(FILE *pInputTxtFile, FILE *pOutputSerialFile) {
    Record r;
    // while(fscanf(pInputTxtFile, "%d%s%s", &r.key, r.code, r.date) != EOF) 
    {
        fwrite(&r, sizeof(Record), 1, pOutputSerialFile);
    }
}
