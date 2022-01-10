#pragma once
#include <stdio.h>
#include <string.h>
#include <string.h>
#include <ws2tcpip.h>

typedef struct node {
	struct node* next;
	int value;
}node_t;

typedef struct queue {
	CRITICAL_SECTION cs;
	node_t* head;
	node_t* tail;
}queue_t;

node_t* create_node(int value) {
	node_t* node = (node_t*)malloc(sizeof(node_t));
	node->next = NULL;
	node->value = value;
	return node;
}

queue_t* create_queue() {
	queue_t* queue = (queue_t*)malloc(sizeof(queue_t));
	InitializeCriticalSection(&queue->cs);
	queue->head = NULL;
	queue->tail = NULL;
	return queue;
}

void enqueue(queue_t* queue, int value) {
	node_t* node = create_node(value);
	EnterCriticalSection(&queue->cs);
	if (queue->head == NULL) {
		queue->head = node;
		queue->tail = node;
		LeaveCriticalSection(&queue->cs);
		return;
	}

	queue->head->next = node;
	queue->head = node;
	LeaveCriticalSection(&queue->cs);
}

int Dequeue(queue_t* q) {
	if (q->tail == NULL && q->head == NULL) {
		//printf("Queue is empty\n");
		return INT_MIN;
	}

	EnterCriticalSection(&q->cs);
	if (q->tail == q->head) {
		q->head = NULL;
	}

	node_t* temp = q->tail;
	q->tail = temp->next;
	int num = temp->value;
	LeaveCriticalSection(&q->cs);
	free(temp);
	return num;
}

void Clear(queue_t* queue)
{
	if (queue == NULL)
		return;

	while (queue->head != NULL)
	{
		Dequeue(queue);
	}

	DeleteCriticalSection(&queue->cs);
}