#ifndef HASHFILE_H
#define HASHFILE_H

#define LOGICAL

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "bucket.h"

int createHashFile(FILE *pFile);
int insertToFile(FILE *pFile, Record r);

FILE *openFile(char *filename);
void createFile(char *filename);

FindRecordResult findRecord(FILE *pFile, int key);
int insertRecord(FILE *pFile, Record record);
int modifyRecord(FILE *pFile, Record record);
int removeRecord(FILE *pFile, int key);
void printContent(FILE *pFile);
void importRecordsFromCsv(char* csvFilename, FILE *pFile);
int updateTime(FILE *pFile, int key);
int compare(const void* a, const void* b);
void filterRecordsByDate(FILE *pFile, char *fromDate, char *toDate);

#endif
