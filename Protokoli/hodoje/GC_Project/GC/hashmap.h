#pragma once
#include "structs.h"

#ifndef HASHMAP_H
#define HASHMAP_H

/**
@brief Initializes the hashmap
@param unsigned int size - number of indexes in the hashmap
@return returns a pointer to a newly initialized hashmap
 */
HASHMAP* HashmapInit(unsigned int size);

/**
@brief Gets and returns the index of an element with a given key
@param HASHMAP** hashmap - adress of the pointer to hashmap
@param void* key - key that identifies an element
@return returns the index of an element
 */
int HashmapGetIndex(HASHMAP** hashmap, void* key);

/**
@brief Function that returns a hash for a given key
@param void* key - key for which the function will return the hash
@return returns the hash
 */
int HashmapGetHash(void* key);

/**
@brief Return an element of the hashmap
@param HASHMAP** hashmap - address of a pointer to hashmap
@param void* key - key that identifies an element
@return returns the element of the hashmap
 */
HASHMAP_ENTRY* HashmapGetElement(HASHMAP** hashmap, void* key);

/**
@brief Returns a newly created element for a hashmap
@param HEAP_BLOCK* entry - heap block structure that will be encapsulated in a hashmap element
@return returns a newle created hashmap entry for storing in the hashmap
 */
HASHMAP_ENTRY* HashmapCreateEntry(HEAP_BLOCK* entry);

/**
@brief Adds a new hashmap element to the hashmap
@param HASHMAP** hashmap - hashmap to which an element will be added
@param HEAP_BLOCK** entry - heap block structure that will be encapsulated with HashmapCreateEntry and added to the hashmap
@returns returns void
 */
void HashmapAdd(HASHMAP** hashmap, HEAP_BLOCK** entry);

/**
@brief Removes an element from the hashmap for a given key and returns a structure to be added to the list of free elements in Heap manager
@param HASHMAP** hashmap - hashmap from which an element will be removed from
@param void* key = key that identifies an element
@return returns a structure to be added to the list of free elements in Heap manager
 */
NODE* HashmapRemove(HASHMAP** hashmap, void* key);

#endif // !HASHMAP_H