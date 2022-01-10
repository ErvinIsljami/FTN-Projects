# Imenko Prezimic sw123-2017
#
# Ovaj program treba završiti...

.section .data
unos_poruka:
	.ascii "Unesite izraz: "
strlen1 = . - unos_poruka

zbir_poruka:
	.ascii "Rezultat: "
strlen2 = . - zbir_poruka

gr_prekoracenje:
	.ascii "Prekoracenje!"
strlen3 = .- gr_prekoracenje

gr_cifra:
	.ascii "Pogresna cifra!"
strlen4 = . - gr_cifra

gr_prazan:
	.ascii "Prazan izraz!"
strlen5 = . - gr_prazan

printfstring:
	.ascii "Rezultat: %hx"
#moze da se menja ovo %x u npr %d, x je za hex, d je dec, o je za oct
#h se dodaje ispred kao half(pola), jer je 16 bit = 32/2
sscanfstring:
	.ascii "%hx %c %hx\n"

ulazniString:
	.fill  100, 1, 0	#moze da se menja samo ovo 100, to je 							#maksimalan broj karaktera
A:						#prvi broj
	.word 0				#word stavljamo jer je 16bit-a
B:						#drugi broj
	.word 0
C:						#operacija
	.byte 0
REZ:
	.word 0
.section .text
.globl main
main:

#poruka za ispis unosa(write)
movl $4, %eax
movl $1, %ebx			#1 -> stdout(standard out)
movl $unos_poruka, %ecx	#koji string
movl $strlen1, %edx
int $0x80				#interupt

#unos ulaznog stringa
movl $3, %eax
movl $0, %ebx		#stdin -> standardni ulaz
movl $ulazniString, %ecx	#u koji string da se unese
movl $100, %edx				#max broj karaktera
int $0x80

#kad se unese string u edx mi se nalazi broj unetih karaktera
cmpl $1, %edx
je prazanString

pushl $B
pushl $C
pushl $A
pushl $sscanfstring
pushl $ulazniString
call sscanf
addl $20, %esp

cmpl $3, %eax		#povratna vrednost sscanf
jne pogresnaCifra

movb C, %dl
cmpb $'+', %dl	#menja se + u npr &(and), |(ili, or), ^(xor)
je saberi

cmpb $'-', %dl
je oduzmi

cmpb $'*', %dl
je pomnozi

cmpb $'/', %dl
je podeli

pogresnaCifra:
movl $4, %eax
movl $1, %ebx			#1 -> stdout(standard out)
movl $gr_cifra, %ecx	#koji string
movl $strlen4, %edx
int $0x80
movl $1, %ebx
jmp kraj


prekoracenje:
movl $4, %eax
movl $1, %ebx			#1 -> stdout(standard out)
movl $gr_prekoracenje, %ecx	#koji string
movl $strlen3, %edx
int $0x80
movl $1, %ebx
jmp kraj

prazanString:
movl $4, %eax
movl $1, %ebx			#1 -> stdout(standard out)
movl $gr_prazan, %ecx	#koji string
movl $strlen5, %edx
int $0x80
movl $1, %ebx
jmp kraj


saberi:
movl A, %eax
movl B, %ebx
addl %ebx, %eax			#umesto addl stavi andl, kod logickih ne postoji prekoracenje!!!
jo prekoracenje			#jc ako su neoznaceni
jmp rezultat

oduzmi:
movl A, %eax
movl B, %ebx
subl %ebx, %eax			#umesto subl orl
jo prekoracenje			#jc ako su neoznaceni
jmp rezultat

pomnozi:
movl A, %eax
movl B, %ebx
imull %ebx			#mnozi eax sa onim sto smo prosledili
cmpl $0, %edx
jne prekoracenje
jmp rezultat

podeli:
movl A, %eax
movl $0, %edx		#visi deo mora na nulu zbog deljenja
movl B, %ebx
idivl %ebx			#mnozi eax sa onim sto smo prosledili
jmp rezultat

rezultat:
movl %eax, REZ

pushl REZ
pushl $printfstring
call printf
addl $8, %esp
movl $0, %ebx





kraj:
movl $1, %eax
int $0x80




























