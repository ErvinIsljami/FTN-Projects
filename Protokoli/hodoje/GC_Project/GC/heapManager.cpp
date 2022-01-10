#include "heapManager.h"

H_MANAGER* HeapManagerInit()
{
	H_MANAGER* hM = NULL;

	if((hM = (H_MANAGER*)malloc(sizeof(H_MANAGER))) == NULL)
	{
		return NULL;
	}

	HeapInit(&hM);

	if((hM->listOfFree = (NODE*)malloc(sizeof(Node))) == NULL)
	{
		return NULL;
	}

	hM->mallocSemaphore = CreateSemaphore(NULL, 1, 1, NULL);

	HEAP_BLOCK* initialMemoryBlock;
	if((initialMemoryBlock = (HEAP_BLOCK*)malloc(sizeof(HEAP_BLOCK))) == NULL)
	{
		return NULL;
	}
	initialMemoryBlock->dataPtr = hM->heapStart;
	initialMemoryBlock->dataSize = hM->heapSize;
	initialMemoryBlock->mark = false;

	hM->listOfFree->blockInfo = initialMemoryBlock;
	hM->listOfFree->next = NULL;
	hM->hashMapOfOccupied = HashmapInit(INITIAL_HASHTABLE_SIZE);
	hM->malloc = Malloc;
	hM->free = Free;

	return hM;
}

void HeapInit(struct Heap_manager** hManager)
{
	H_MANAGER* hM = *hManager;

	if((hM->heapStart = (char*)malloc(INITIAL_HEAP_SIZE)) == NULL)
	{
		return;
	}
	hM->heapSize = INITIAL_HEAP_SIZE;
}

void* Malloc(struct Heap_manager** hManager, int nbytes)
{
	H_MANAGER* hM = *hManager;
	// Define new memory block data
	HEAP_BLOCK* mB = NULL;
	if((mB = (HEAP_BLOCK*)malloc(sizeof(HEAP_BLOCK))) == NULL)
	{
		return NULL;
	}
	mB->dataPtr = NULL;
	mB->dataSize = nbytes;
	mB->mark = false;

	// Lock while searching a block position and addition to hashmap
	WaitForSingleObject(hM->mallocSemaphore, INFINITE);

	// Recalculate new position for free block and its size 
	NODE* firstFittingNode = hM->listOfFree;
	if (ListCount(&hM->listOfFree) == 1)
	{
		if (nbytes > firstFittingNode->blockInfo->dataSize)
		{
			// Not enough memory
			return NULL;
		}
		mB->dataPtr = firstFittingNode->blockInfo->dataPtr;
		firstFittingNode->blockInfo->dataPtr = (char*)(firstFittingNode->blockInfo->dataPtr) + nbytes;
		firstFittingNode->blockInfo->dataSize = firstFittingNode->blockInfo->dataSize - nbytes;
	}
	else
	{
		NODE* previous = NULL;
		// Find the first fit
		while (nbytes > firstFittingNode->blockInfo->dataSize)
		{
			if (firstFittingNode->next == NULL)
			{
				//collect
				// after collect, start again
				// if it fails again, heap is full
				return NULL;
			}
			previous = firstFittingNode;
			firstFittingNode = firstFittingNode->next;
		}

		// Assign pointer and update the fitting node
		mB->dataPtr = firstFittingNode->blockInfo->dataPtr;
		firstFittingNode->blockInfo->dataPtr = (char*)(firstFittingNode->blockInfo->dataPtr) + nbytes;
		firstFittingNode->blockInfo->dataSize = firstFittingNode->blockInfo->dataSize - nbytes;

		// Handle if updated available size is 0 depending if there is a previous node to first fitting
		if (firstFittingNode->blockInfo->dataSize == 0)
		{
			if (previous == NULL)
			{
				hM->listOfFree = firstFittingNode->next;
				free(firstFittingNode);
			}
			else
			{
				previous->next = firstFittingNode->next;
				free(firstFittingNode);
			}
		}
	}

	// Add new node in list of occupied nodes
	HashmapAdd(&hM->hashMapOfOccupied, &mB);		

	// Free the semaphore so other threads can malloc
	ReleaseSemaphore(hM->mallocSemaphore, 1, NULL);

	return mB->dataPtr;
}

void Free(struct Heap_manager** hManager, void** ptr)
{
	H_MANAGER* hM = *hManager;
	NODE* nodeToFree = HashmapRemove(&hM->hashMapOfOccupied, *ptr);

	// Search where to put the block back
	// Try to find the between
	NODE* firstFreeNodeIterator = hM->listOfFree;
	NODE* previousNode = NULL;
	while (firstFreeNodeIterator->blockInfo->dataPtr < nodeToFree->blockInfo->dataPtr)
	{
		previousNode = firstFreeNodeIterator;
		firstFreeNodeIterator = firstFreeNodeIterator->next;
	}

	// If there is only one block currently free and it's not at the beginning of heap
	// just connect the blocks
	if (previousNode == NULL)
	{
		previousNode = nodeToFree;
		previousNode->next = firstFreeNodeIterator;
		hM->listOfFree = previousNode;

		// Check if nodeToFree and its next node are next to each other
		if ((char*)nodeToFree->blockInfo->dataPtr + nodeToFree->blockInfo->dataSize == firstFreeNodeIterator->blockInfo->dataPtr)
		{
			firstFreeNodeIterator->blockInfo->dataPtr = nodeToFree->blockInfo->dataPtr;
			firstFreeNodeIterator->blockInfo->dataSize = firstFreeNodeIterator->blockInfo->dataSize + nodeToFree->blockInfo->dataSize;
			hM->listOfFree = firstFreeNodeIterator;
			free(nodeToFree->blockInfo);
			free(nodeToFree);
		}
	}
	else
	{
		previousNode->next = nodeToFree;
		nodeToFree->next = firstFreeNodeIterator;

		// Check if previous and nodeToFree are next to each other
		if ((char*)previousNode->blockInfo->dataPtr + previousNode->blockInfo->dataSize == nodeToFree->blockInfo->dataPtr)
		{
			// If they are increase the size of previous and free nodeToFree
			previousNode->blockInfo->dataSize = previousNode->blockInfo->dataSize + nodeToFree->blockInfo->dataSize;
			previousNode->next = nodeToFree->next;
			free(nodeToFree->blockInfo);
			free(nodeToFree);
		}
		// if not, check if nodeToFree and current node are next to each other
		else if ((char*)nodeToFree->blockInfo->dataPtr + nodeToFree->blockInfo->dataSize == firstFreeNodeIterator->blockInfo->dataPtr)
		{
			firstFreeNodeIterator->blockInfo->dataPtr = nodeToFree->blockInfo->dataPtr;
			firstFreeNodeIterator->blockInfo->dataSize = firstFreeNodeIterator->blockInfo->dataSize + nodeToFree->blockInfo->dataSize;
			free(nodeToFree->blockInfo);
			free(nodeToFree);
		}
	}
}