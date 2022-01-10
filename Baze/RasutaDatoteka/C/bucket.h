#include <stdlib.h>
#ifndef BUCKET_H
#define BUCKET_H

#define B 11             // broj baketa
#define STEP 1          // fiksan korak
#define BUCKET_SIZE 2   // faktor baketiranja

#define WITH_HEADER 1
#define WITHOUT_HEADER 0
#define WITH_KEY 1
#define WITHOUT_KEY 0

#define RECORD_FOUND 0

#define CODE_LEN 8
#define DATE_LEN 21
#define EV_BR_LEN 10
#define REG_LEN 11
#define PARKING_LEN 7
typedef enum { EMPTY = 0, ACTIVE, DELETED } RECORD_STATUS;

typedef struct 
{
    int key;
    char evidencioni_broj[EV_BR_LEN];
    char registarska_oznaka[REG_LEN];
    char datum_vreme[DATE_LEN];
    char parking_mesto[PARKING_LEN];
    int boravak_minuta;
    RECORD_STATUS status; 
} Record; //SLOG

typedef struct 
{
    Record records[BUCKET_SIZE];
} Bucket;

typedef struct 
{
    int ind1;           // indikator uspesnosti trazenja, 0 - uspesno, 1 - neuspesno
    int ind2;           // indikator postojanja slobodnih lokacija, 0 - nema, 1 - ima
    Bucket bucket;      
    Record record;
    int bucketIndex;    // indeks baketa sa nadjenim slogom
    int recordIndex;    // indeks sloga u baketu
} FindRecordResult;

int transformKey(int key);
int nextBucketIndex(int currentIndex);
void printRecord(Record record);
void printBucket(Bucket bucket);
Record scanRecord();
int scanKey();
void printHeader();
int calculateNumOfDigitsFromInt(int number);
void printBucketAscending(Bucket bucket);

#endif