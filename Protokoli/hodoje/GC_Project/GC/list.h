#pragma once
#include "structs.h"

#ifndef LIST_H
#define LIST_H

//void ListAdd(NODE** root, HEAP_BLOCK** element);
//void ListInsert(NODE** root, int index, HEAP_BLOCK** element);
//HEAP_BLOCK* ListElementAt(NODE** root, int index);
//NODE* ListRemove(NODE** root, void* ptr);
//NODE* ListRemoveAt(NODE** root, int index);
//void ListClear(NODE** root);
/**
@brief Returns the number of elements in a specified list
@param NODE** root - root of the list
@return returns the number of elements in a list
 */
int ListCount(NODE** root);

#endif