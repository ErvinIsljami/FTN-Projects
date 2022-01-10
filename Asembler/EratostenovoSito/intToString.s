.section .data
strlen = 10
string:   .fill strlen, 1, 0
.section .text
.global intToString

intToString:

pushl %ebp
movl %esp, %ebp
pushl %edi
pushl %esi

movl 8(%ebp), %eax

movl $string, %ecx
movl $10, %esi

petlja:
movl $0, %edx

divl %esi

addl $'0', %edx
movb %dl, (%ecx)
incl %ecx

cmpl $0, %eax
je obrniString
jmp petlja

obrniString:
movl $10, %eax
movl %eax, (%ecx)
decl %ecx           #pokazivac na poslednji
movl $string, %ebx  #pokazivac na prvi

obrni:
movb (%ecx), %dl
movb (%ebx), %dh
movb %dh, (%ecx)
movb %dl, (%ebx)
incl %ebx
decl %ecx
cmpl %ebx, %ecx
jbe ispis
jmp obrni

ispis:
movl $4, %eax
movl $1, %ebx
movl $string, %ecx
movl $strlen, %edx
int $0x80

kraj:
popl %esi
popl %edi
movl %ebp, %esp
popl %ebp
ret
