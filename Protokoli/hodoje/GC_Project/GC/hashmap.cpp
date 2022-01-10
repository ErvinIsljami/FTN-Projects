#include "hashmap.h"

HASHMAP* HashmapInit(unsigned int size)
{
	HASHMAP* hashmap = NULL;

	if (size < 1)
	{
		return NULL;
	}

	if ((hashmap = (HASHMAP*)malloc(sizeof(HASHMAP))) == NULL)
	{
		return NULL;
	}
		
	if ((hashmap->buckets = (HASHMAP_ENTRY**)malloc(size * sizeof(HASHMAP_ENTRY*))) == NULL)
	{
		return NULL;
	}
		
	for (int i = 0; i < size; i++)
	{
		hashmap->buckets[i] = NULL;
	}

	hashmap->size = size;

	return hashmap;
}

int HashmapGetIndex(HASHMAP** set, void* key)
{
	int hash = HashmapGetHash(key);

	return (hash % (*set)->size);
}

int HashmapGetHash(void* key)
{
	return (int)key / 4;;
}

HASHMAP_ENTRY* HashmapCreateEntry(HEAP_BLOCK* newBlock)
{
	HASHMAP_ENTRY* newentry = NULL;
	void *key = newBlock->dataPtr;

	if (key == NULL)
	{
		return NULL;
	}

	if ((newentry = (HASHMAP_ENTRY*)malloc(sizeof(HASHMAP_ENTRY))) == NULL)
	{
		return NULL;
	}

	newentry->key = newBlock->dataPtr;
	newentry->blockInfo = newBlock;
	newentry->next = NULL;

	return newentry;
}

void HashmapAdd(HASHMAP** set, HEAP_BLOCK** entry)
{
	HASHMAP_ENTRY* newentry = NULL;
	HASHMAP_ENTRY* next = NULL;
	HASHMAP_ENTRY* last = NULL;		// Last is the last previous element of an entry to be inserted
	void* key = (*entry)->dataPtr;
	int index = HashmapGetIndex(set, key);

	// Next is the first entry on particular index
	next = (*set)->buckets[index];

	// Checks if there is already an entry on particular index
	// Iterate to the end of the list until next is NULL
	while (next != NULL && next->key != NULL && (key != next->key))
	{
		last = next;
		next = next->next;
	}
	// After this loop, next is NULL
	// New entry has to be assigned to next

	// Creates new entry based on new memory block
	newentry = HashmapCreateEntry(*entry);

	// Case if bucket is empty
	if (next == (*set)->buckets[index])
	{
		(*set)->buckets[index] = newentry;
		newentry->next = NULL;
	}
	// Case if bucket is not empty
	else if (next == NULL)
	{
		last->next = newentry;
	}
	// Place in the middle of the list
	else
	{
		newentry->next = next;
		last->next = newentry;
	}
}

NODE* HashmapRemove(HASHMAP** set, void* key)
{
	HASHMAP_ENTRY* elementToRemove = NULL;
	HASHMAP_ENTRY* previous = NULL;
	int index = HashmapGetIndex(set, key);

	// Getting the first element of the list
	elementToRemove = (*set)->buckets[index];

	while (elementToRemove != NULL && elementToRemove->key != NULL && (key != elementToRemove->key))
	{
		previous = elementToRemove;
		elementToRemove = elementToRemove->next;
	}
	// After this loop we have the elementToRemove

	if (previous == NULL)
	{
		// Either elementToRemove's next or NULL will be at this index
		(*set)->buckets[index] = elementToRemove->next;
	}
	else
	{
		// Tie up previous and elementToRemove's next
		previous->next = elementToRemove->next;
	}

	// Reference to a node to be added to the H_MANAGER free list
	NODE* nodeForFreeList = NULL;
	if((nodeForFreeList = (NODE*)malloc(sizeof(NODE))) == NULL)
	{
		return NULL;
	}
	nodeForFreeList->blockInfo = elementToRemove->blockInfo;
	nodeForFreeList->next = NULL;

	free(elementToRemove);

	return nodeForFreeList;
}

HASHMAP_ENTRY* HashmapGetElement(HASHMAP** set, void* key)
{
	HASHMAP_ENTRY* entryToReturn = NULL;
	int index = HashmapGetIndex(set, key);

	entryToReturn = (*set)->buckets[index];
	if (entryToReturn == NULL)
	{
		return NULL;
	}
	else
	{
		while (entryToReturn != NULL && entryToReturn != NULL && entryToReturn->key != key)
		{
			entryToReturn = entryToReturn->next;
		}
		
		if (entryToReturn == NULL || entryToReturn->key == NULL || entryToReturn->key != key)
		{
			return NULL;
		}
		else
		{
			return entryToReturn;
		}
	}
}