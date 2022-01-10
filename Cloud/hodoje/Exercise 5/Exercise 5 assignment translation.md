**Distributed transaction in cloud**

**Note:**
- "*count*" parameter from all of the methods have to be *int* since Azure tables don't support *uint*.

An assignment is to implement a distributed transaction as shown in picture 1.

Recommendation is that an assignment consists of the following projects:
1. *DistributedTransaction* (azure cloud project)
   - Bookstore (worker role)
   - Bank (worker role)
   - TransactionCoordinator (worker role)
2. Client (console application)
3. Common (class library) - contains all interfaces shown in picture 2.

*Bookstore* is a *worker role* type of process in *cloud* project that has implemented *IBookstore* interface and in it there will be all the methods from *IBookstore* and *ITransaction* interfaces. It is necessary to host a WCF service whos contract will be *IBookstore*.
- *ListAvailableItems()* - method that writes all books in the emulator
- *EnlistPurchase(string bookID, uint count)* - preparation for starting 2PC protocol, it is necessary to store *bookID* and *count* parameters locally.
- *GetItemPrice(string bookID)* - returns the price of a book whos ID is the passed *bookID*.
- *Prepare()* - finds a book in a table that has the *bookID* that is passed to *Enlist* method. If it is possible to decrease the book count of the book for *count* value, write to table a new entity whos *RowKey* will be *bookID* + "prep". That new entity will have a decreased state for *count* and the method returns *true*. Otherwise, method returns *false*.
- *Commit()* - finds an entity *bookID* + "prep" and its state writes down to entity *bookID* and the new entity is deleted.
- *Rollback()* - if in there is an entity *bookID* + "prep" in the table, it is deleted.

Same logic is used for *Bank* service, only difference is that methods from *IBank* and *ITransaction* interfaces are implemented. It is necessary to host a WCF service whos contract will be *IBank*.
- *ListClients()* - method that writes all clients to the emulator
- *EnlistMoneyTransfer(string userID, double amount)* - preparation for starting 2PC protocol, it is necessary to store *userID* and *amount* parameters locally.
- *Prepare()* - finds a client in the table with *userID* that is passed to *Enlist* method. If it is possible to decrease the state on the account for *amount* value, a new entity whos *RowKey* is *userID* + "prep" is written to the table. That new entity has a decreased state on the account and the method returns *true*. Otherwise it returns *false*.
- *Commit()* - finds an entity *userID* + "prep" and its state writes down to entity *userID* and the new entity is deleted.
- *Rollback()* - if there is an entity *userID* + "prep" in the table, it is deleted.

When running the *Bank* and *Bookstore* worker roles the following data should be stored in an Azure table (a same table for two different types of entities can be used):

*Bookstore:*
- *BookID*
- *Book state on the stock*
- *Book price*

*Bank service:*
- *UserID*
- *Account state*

*TransactionCoordinator* implements a method from *IPurchase* interface and makes a connection with *Bank* and *Bookstore* services. Coordinator should implement a two-phase commit protocol in the implementation of *OrderItem* method. Idea of the coordinator is to start a transaction (*Enlist*), check if both services can change from one consistent state to the other (*Prepare*) and based on that finish the transaction (*Commit* or *Rollback*). First, *Enlist* and  *Prepare* methods are called from both of the services. If conditions for changing the states are valid, coordinator calls *Commit*, otherwise *Rollback*.

*Client* initiates the communication with *TransactionCoordinator* service via *IPurchase* interface.
