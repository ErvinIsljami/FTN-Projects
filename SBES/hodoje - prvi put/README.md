# sses
College subject - Security and safety in electroenergetic systems

**NOTE:**
  - Project not fully done. Timeline was due.
  - Problems that stayed:
    - After revoking the certificate, user can still login to the service and can call methods, but he shouldn't be able to even get
    authenticated after the revocation.
    - Backup (replication) does not work on two different computers.
  - This repository is used as a replication repository since we are using TFS repository as a main repository.
  - Each commit will have the collaborator name sticked to it.
  - List of collaborators:
    - https://github.com/TheLastHop3
    - https://github.com/srki1925
    - https://github.com/fgbjoki

**Assignment text:**

**PROJECT 15**

Implement a banking system which is composed of the following components:
  - **BANK**, has two roles:
    - Issuing MasterCard cards to its clients during the creation of a bank account. For needs of issuing a card, clients contact the bank 
    via Windows communication protocol. MasterCard is a certificate generated so the *SubjectName* matches the username, in addition, a
    PIN code is defined as a second factor of authentication (which the bank is not allowed to store in a readable format). Generated 
    cards are stored in a local folder for needs of later distribution. Distribution of MasterCard certificates is done manually.
    - Managing user transactions, i.e. payments and payouts, when the clients represent themselves to the bank by certificates. Each
    transaction has to be digitally signed by client.
  - **CLIENTS**, which:
    - Send requests for creating an account and requests for withdrawal of certificates and issuing of a new one in case of compromisation. In this case, clients establish a connection with bank via Windows authentication protocol.
    - Send requests for transaction execution and PIN code reset, when clients and bank authenticate each other using certificates. During sending of each transaction, PIN code validation check is done, as well as validation of digital signatures.
  - Replication of data about all user accounts on a backup server. Primary and backup components for certificate management authenticate each other using Windows authentication protocol.
  
Additionally:
  - **BANK** keeps records of all client activities within a specifically created Windows event log, primarily the following activities:
    - Requests for issuing, withdrawal and renewal of MasterCard certificates
    - Requests for MasterCard's certificate PIN code reset
    - Requests for transaction execution
    - If the **BANK** detects that in a period of N (configurable) time, a payouts transaction is done to the same account more than M times, an event is logged on the central BankingAudit server with which the **BANK** communicates via Windows authentication protocol. During logging, in the *Source* it is necessary to specify the name of the bank, account name, time of detection etc.
