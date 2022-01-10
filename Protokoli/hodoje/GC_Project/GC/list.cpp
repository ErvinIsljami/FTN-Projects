#include "structs.h"
#include "list.h"

// List API for MEMORY BLOCKS
//void ListAdd(NODE** root, HEAP_BLOCK** data)
//{
//	NODE* newElemPtr = NULL;
//	if((newElemPtr = (NODE*)malloc(sizeof(NODE))) == NULL)
//	{
//		return;
//	}
//	newElemPtr->blockInfo = *data;
//	newElemPtr->next = NULL;
//
//	// Empty list
//	if (*root == NULL)
//	{
//		*root = newElemPtr;
//		return;
//	}
//
//	// Non-empty list
//	// You wonder why this assignment is done here?
//	// If this was done before the check above currentElemPtr and *root would point to NULL
//	// And if we, after that, used currentElemPtr in the rest of code, only the memory that it points to would be changed
//	// and not the memory where *root points to because NULL is not a valid location to be pointed at
//	NODE* currentElemPtr = *root;
//
//	while (currentElemPtr->next != NULL)
//	{
//		currentElemPtr = currentElemPtr->next;
//	}
//	currentElemPtr->next = newElemPtr;
//}

//void ListInsert(NODE** root, int index, HEAP_BLOCK** data)
//{
//	NODE* newElem = NULL;
//	if((newElem = (NODE*)malloc(sizeof(NODE))) == NULL)
//	{
//		return;
//	}
//	newElem->blockInfo = *data;
//	newElem->next = NULL;
//
//	if (ListCount(&(*root)) <= index)
//	{
//		return;
//	}
//
//	NODE* currentElemPtr = *root;
//	NODE* previousElemPtr = NULL;
//	int idx = 0;
//	while (currentElemPtr != NULL)
//	{
//		if (idx == index)
//		{
//			newElem->next = currentElemPtr;
//			previousElemPtr->next = newElem;
//			return;
//		}
//		previousElemPtr = currentElemPtr;
//		currentElemPtr = currentElemPtr->next;
//		idx++;
//	}
//}

//HEAP_BLOCK* ListElementAt(NODE** root, int index)
//{
//	// Empty list
//	if (*root == NULL)
//	{
//		// Was not defined what to return in case of empty list
//		return NULL;
//	}
//
//	NODE* currentElemPtr = *root;
//
//	int elemCnt = 0;
//	while (currentElemPtr != NULL)
//	{
//		if (elemCnt == index)
//		{
//			return currentElemPtr->blockInfo;
//		}
//		currentElemPtr = currentElemPtr->next;
//		elemCnt++;
//	}
//
//	// If the index is not valid
//	return NULL;
//}

//NODE* ListRemove(NODE** root, void* ptr)
//{
//	if (*root == NULL)
//	{
//		return *root;
//	}
//
//	NODE* nodeToRemove = *root;
//
//	if (ListCount(root) == 1)
//	{
//		*root = NULL;
//		return nodeToRemove;
//	}
//
//	// If it's the first node and has no previous nodes
//	if (nodeToRemove->blockInfo->dataPtr == ptr)
//	{
//		NODE* next = nodeToRemove->next;
//		*root = next;
//		return nodeToRemove;
//	}
//
//	NODE* previousElemPtr = NULL;
//	while (nodeToRemove->blockInfo->dataPtr != ptr)
//	{
//		previousElemPtr = nodeToRemove;
//		nodeToRemove = nodeToRemove->next;
//	}
//	NODE* next = nodeToRemove->next;
//	previousElemPtr->next = next;
//	return nodeToRemove;
//}

//NODE* ListRemoveAt(NODE** root, int index)
//{
//	if (*root == NULL)
//	{
//		return *root;
//	}
//
//	if (ListCount(root) <= index)
//	{
//		return NULL;
//	}
//
//	NODE* currentElemPtr = *root;
//	NODE* previousElemPtr = NULL;
//	int idx = 0;
//	while (currentElemPtr != NULL)
//	{
//		if (idx == index)
//		{
//			NODE* next = currentElemPtr->next;
//			//free(currentElemPtr);
//			previousElemPtr->next = next;
//			//return;
//			return currentElemPtr;
//		}
//		previousElemPtr = currentElemPtr;
//		currentElemPtr = currentElemPtr->next;
//		idx++;
//	}
//	return NULL;
//}

//void ListClear(NODE** root)
//{
//	if (*root == NULL)
//	{
//		return;
//	}
//
//	NODE* restOfList = *root;
//	while (*root != NULL)
//	{
//		restOfList = restOfList->next;
//		free(*root);
//		*root = restOfList;
//	}
//}

int ListCount(NODE** root)
{
	if (*root == NULL)
	{
		return 0;
	}

	NODE* currentElemPtr = *root;
	int elemCnt = 0;

	while (currentElemPtr != NULL)
	{
		currentElemPtr = currentElemPtr->next;
		elemCnt++;
	}

	return elemCnt;
}