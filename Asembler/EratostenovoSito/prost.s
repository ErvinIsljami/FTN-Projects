#Ispisati sve proste brojeve do 1 000 000
#Zadatak koristi algoritam Eratostenovog sita
#Autor: Milos Prica, PR65/2016
.section .data
NELEM  = 999999
niz:    .fill NELEM, 4, 0
string:   .ascii "Prost brojevi do milion su: \0"
stringlen = . - string
.section .text

.global main
main:

movl $2, %eax
movl $niz, %ebx

upisi:
movl %eax,(%ebx)
incl %eax
addl $4, %ebx
cmpl $NELEM, %eax
ja dalje
jmp upisi

dalje:
movl $2, %edi       #prvi prost broj
movl $niz, %esi     #pokazivac na niz brojeva


movl $4, %eax
movl $1, %ebx
movl $string, %ecx
movl $stringlen, %edx
int $0x80

sito:
movl (%esi), %eax   #uzmemo tekuci
cmpl $0, %eax       #proverimo da li je nula ako jeste idi na sledeci jer nula nije prost broj
je sledeci

#stampanje prostog broja

pushl (%esi)
call intToString
addl $4, %esp

movl %esi, %ecx
addl $4, %ecx
movl %edi, %ebx

    izbaci:
    movl (%ecx), %eax
    movl $0, %edx
    divl %edi

    cmpl $0, %edx
    je slozen
    jmp preskoci

    slozen:
    movl $0, %eax
    movl %eax, (%ecx)

    preskoci: #kao labela sledeci
    addl $4, %ecx
    incl %ebx
    cmpl $NELEM, %ebx
    je sledeci
    jmp izbaci

sledeci:
addl $4, %esi
incl %edi
cmpl $1000, %edi    #algoritam ide do koren iz 1 000 000
je istampajostatak
jmp sito


istampajostatak:
movl (%esi), %eax
cmpl $0, %eax
je sledeci2

pushl (%esi)
call intToString
addl $4, %esp

sledeci2:
addl $4, %esi
incl %edi
cmpl $NELEM, %edi    #algoritam ide do koren iz 1 000 000
je kraj
jmp istampajostatak




kraj:
movl $1, %eax
movl $0, %ebx
int $0x80
