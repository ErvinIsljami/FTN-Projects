#pragma once
#include "structs.h"
#include "hashmap.h"
#include "list.h"

#ifndef HEAP_MANAGER_H
#define HEAP_MANAGER_H

#define INITIAL_HEAP_SIZE 100 * 1024 * 1024
#define INITIAL_HASHTABLE_SIZE INITIAL_HEAP_SIZE / 8

/**
@brief Allocates a memory block of defined size
@param Heap_manager** hManager - address of a pointer to Heap manager
@param int nbytes - size of the memory block in bytes
@return returns a pointer to newly allocated memory on Heap manager's heap
*/
void* Malloc(struct Heap_manager** hManager, int nbytes);
/**
@brief Deletes an entry in the hashmap of occupied memory blocks 
       and adds the information of deleted block to a list of free memory blocks
@param Heap_manager** hManager - address of a pointer to Heap manager
@param void** ptr - address of a pointer to an address on Heap manager (serves as a key for an allocated block)
@return returns void
*/
void Free(struct Heap_manager** hManager, void** ptr);
/**
@brief Initializes the heap for the Heap manager by allocating memory of defined size
@param Heap_manager** hManager - address of a pointer to Heap manager
@return returns void
*/
void HeapInit(struct Heap_manager** hManager);
/**
@brief Initilizes the Heap manager
@param no params
@return returns a pointer to newly allocated Heap manager
*/
struct Heap_manager* HeapManagerInit();

#endif
