#include "bucket.h"
#include <stdio.h>
#include <string.h>
#include <math.h>

int transformKey(int key) 
{
    // get the string representation of the passed key
    char stringKey[10];
    itoa(key, stringKey, 10);

    // get the number of digits of number of buckets (e.g. 11 buckets yields 2 digit places)
    int bucketNumDigitCnt = calculateNumOfDigitsFromInt(B);

    // extract parts of passed key
    char* firstPart = (char*)calloc(bucketNumDigitCnt, sizeof(char));
    char* secondPart = (char*)calloc(bucketNumDigitCnt, sizeof(char));
    char* thirdPart = (char*)calloc(bucketNumDigitCnt, sizeof(char));
    char* fourthPart = (char*)calloc(bucketNumDigitCnt, sizeof(char));
    char* fifthPart = (char*)calloc(bucketNumDigitCnt, sizeof(char));
    memcpy(firstPart, (const void*)(stringKey), bucketNumDigitCnt);
    memcpy(secondPart, (const void*)(stringKey + 2), bucketNumDigitCnt);
    memcpy(thirdPart, (const void*)(stringKey + 4), bucketNumDigitCnt);
    memcpy(fourthPart, (const void*)(stringKey + 6), bucketNumDigitCnt);
    memcpy(fifthPart, (const void*)(stringKey + 8), bucketNumDigitCnt);

    // convert each part to int
    int firstPartInt = atoi(firstPart);
    int secondPartInt = atoi(secondPart);
    int thirdPartInt = atoi(thirdPart);
    int fourthPartInt = atoi(fourthPart);
    int fifthPartInt = atoi(fifthPart);

    // calculate the sum of parts
    int partsSumInt = firstPartInt + secondPartInt + thirdPartInt + fourthPartInt + fifthPartInt;

    // get the number of digits of the sum
    int partsSumIntDigitCnt = calculateNumOfDigitsFromInt(partsSumInt);

    // create string representation of the sum
    char* partsSum = (char*)malloc(partsSumIntDigitCnt);
    itoa(partsSumInt, partsSum, 10);

    // get the final sum used in determining the bucket
    int finalSum = 0;
    if(strlen(partsSum) > 2){
        finalSum = atoi((const char*)(partsSum + 1));
    }
    else{
        finalSum = atoi((const char*)partsSum);
    }

    // free all resources
    free(partsSum);
    free(firstPart);
    free(secondPart);
    free(thirdPart);
    free(fourthPart);
    free(fifthPart);

    // determine the bucket based on the formula here -> http://www.acs.uns.ac.rs/sr/filebrowser/download/4591209 page 25. calculation of T1.
    int bNum = B;
    int T = finalSum % (int)pow(bNum, bucketNumDigitCnt);
    return 1 + floor((B * T) / pow(10, bucketNumDigitCnt));
}

int calculateNumOfDigitsFromInt(int number)
{
    int cnt = 0;
    while(number != 0)
    {
        number /= 10;
        cnt++;
    }
    return cnt;
}

int nextBucketIndex(int currentIndex) 
{
    return (currentIndex + STEP) % B;
}

void printHeader() 
{
    printf("%7s|%10s|%16s|%18s|%17s|%13s|%14s \n","status","key","evidencioni broj","registarska oznaka","datum i vreme","parking mesto","duzina boravka");
}

char* getEnumStringFromEnumValue(RECORD_STATUS status)
{
    switch(status)
    {
        case EMPTY:
            return "EMPTY";
        case ACTIVE:
            return "ACTIVE";
        case DELETED:
            return "DELETED";
        default:
            return "EMPTY";
    }
}

void printRecord(Record record) 
{
    char skey[10];
    char sboravak_minuta[10];
    itoa(record.key, skey, 10);
    itoa(record.boravak_minuta, sboravak_minuta, 10);
    printf("%7s|%10s|%16s|%18s|%17s|%13s|%14s\n", getEnumStringFromEnumValue(record.status), skey, record.evidencioni_broj, record.registarska_oznaka, record.datum_vreme, record.parking_mesto, sboravak_minuta);
}

void printBucket(Bucket bucket) 
{
    int i;
    for (i = 0; i < BUCKET_SIZE; i++) 
    {
        if(bucket.records[i].status == ACTIVE)
        {
            printRecord(bucket.records[i]);
        }
    }
}

//TODO: izmeniti da se unosi sve
Record scanRecord() 
{
    Record record;
    printf("\nUnesite slog: \n");
    record.status = ACTIVE;
    
    printf("key (max 2,147,483,647):\n");
    scanf("%d", &record.key);
    
    printf("Evidencioni broj (9 cifara):\n");
    scanf("%s", record.evidencioni_broj);

    printf("Datum i vreme (format yyyy.mm.dd.#hh:mm):\n");
    scanf("%s", record.datum_vreme);

    printf("Boravak u minutima (max 1 000 000 minuta):\n");
    scanf("%d", &record.boravak_minuta);
    
    printf("Parking mesto (7 karaktera):\n");
    scanf("%s", record.parking_mesto);

    printf("Registarska oznaka auta (10 karaktera):\n");
    scanf("%s", record.registarska_oznaka);
    
    return record;
}

int scanKey() 
{
    int key;
    printf("key = ");
    scanf("%d", &key);
    return key;
}