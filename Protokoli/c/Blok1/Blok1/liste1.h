typedef struct Knjiga_st {

    char naslov[30];
    char autor[30];
    int brStrana;
    float cena;
}Knjiga;

typedef struct Cvor_st {
    Knjiga podatak;
    struct Cvor_st sledeci;
}CVOR;


void InitList(CVOR** glava) {

    *glava = NULL;
}

void Push(CVOR** glava, Knjiga k) {

    CVOR* pom = *glava;
    CVOR* novi = (CVOR*)malloc(sizeof(CVOR));
    novi->sledeci = NULL;
    novi->podatak = k;

    if(pom == NULL) {
        return;
    }

    while(pom->sledeci != NULL) {

        pom = pom->sledeci;
    }

    pom->sledeci = novi;
}

void PushFront(CVOR** glava, Knjiga k) {
    CVOR *pom = *glava;
    CVOR *novi = (CVOR *)malloc(sizeof(CVOR));
    novi->sledeci = NULL;
    novi->podatak = k;

    if (pom == NULL)
    {
        return;
    }

    novi->sledeci = *glava;
    *glava = novi;
}

void Pop(CVOR** glava) {

    CVOR* prethodni = *glava;

    if(prethodni == NULL) {
        return;
    }

    CVOR* pom = prethodni->sledeci;
    if(pom == NULL) {

        free(prethodni);
        *glava = NULL;
        return;
    }

    while(pom->sledeci != NULL) {

        prethodni = pom;
        pom = pom->sledeci;
    }

    free(pom);
    prethodni->sledeci = NULL;
}

void PopFront(CVOR** glava) {

    CVOR* pom = *glava;

    if(pom == NULL) {
        return;
    }

    *glava = pom->sledeci;
    free(pom);
}

Knjiga Peak(CVOR* glava) {

    CVOR* pom = glava;

    if(pom == NULL) {
        return;
    }

    while(pom->sledeci != NULL) {
        pom = pom->sledeci;
    }

    return pom->podatak;
}

Knjiga Pop2(CVOR** glava) {

    Knjiga ret;
    CVOR* prethodni = *glava;
    if(prethodni == NULL) {
        return;
    }

    CVOR* pom = prethodni->sledeci;
    if(pom == NULL) {
        free(prethodni);
        *glava = NULL;
    }

    while(pom->sledeci != NULL) {

        prethodni = pom;
        pom = pom->sledeci;
    }

    ret.cena = pom->podatak.cena;
    ret.brStrana = pom->podatak.brStrana;
    strcpy(ret.autor, pom->podatak.autor);
    strcpy(ret.naslov, pom->podatak.naslov);

    free(pom);
    prethodni->sledeci = NULL;
}

void ClearList(CVOR** glava) {

    CVOR* pom = *glava;

    while(pom != NULL) {

        *glava = pom->sledeci;
        free(pom);
        pom = *glava;
    }
}

void InsertEl(CVOR** glava, Knjiga k, int index) {

    CVOR* prethodni = *glava;
    CVOR* pom = prethodni->sledeci;
    CVOR* novi = (CVOR *)malloc(sizeof(CVOR));
    novi->sledeci = NULL;
    novi->podatak = k;
    int i = 0;

    if(prethodni == NULL){
        return;
    }

    if(index == 0) {

        novi->sledeci = prethodni;
        *glava = novi;
        return;
    }
    i++;

    while(pom != NULL) {

        if(i == index) {

            novi->sledeci = pom;
            prethodni->sledeci = novi;
            return;
        }

        pom = pom->sledeci;
        prethodni = prethodni->sledeci;
        i++;
    }
}

Knjiga SearchByIndex(CVOR* glava, int index){

    CVOR* pom = glava;
    int i = 0;

    if(pom == NULL) {
        return;
    }

    while(pom != NULL) {
        i++;
        if(i == index) {
            break;
        }

        pom = pom->sledeci;
    }

    if(i < index) {
        printf("prekoracili ste index");
        return;
    }

    return pom->podatak;
}

Knjiga SearchByNaslov(CVOR* glava, char* naslov) {

    CVOR* pom = glava;

    if(pom == NULL){
        return;
    }

    while(pom != NULL) {

        if(strcmp(pom->podatak.naslov,naslov) == 0) {
            return pom->podatak;
        }

        pom = pom->sledeci;
    }

    return;
}

void RemoveByNaslov(CVOR** glava,char* naslov) {

    CVOR* prethodni = *glava;
    CVOR* pom = prethodni->sledeci;

    if(prethodni == 0) {
        return;
    }

    if(strcmp(prethodni->podatak.naslov,naslov) == 0) {
        *glava = pom;
        free(prethodni);
        return;
    }

    while(pom != NULL) {
        if(strcmp(pom->podatak.naslov,naslov) == 0) {
            prethodni->sledeci = pom->sledeci;
            free(pom);
            return;
        }

    }
    return;
}

int CalculateLen(CVOR* glava) {

    CVOR* pom = glava;
    int len = 0;

    if(pom == NULL) {
        return;
    }

    while(pom != NULL) {
        len += sizeof(pom->podatak);
        pom = pom->sledeci;
    }

    return len;
}

char* Serialize(CVOR* glava) {

    CVOR* pom = glava;
    int len = CalculateLen(glava);
    int i = 0;
    char* buffer = (char*)malloc(len);

    while(pom != NULL) {

        memcpy(buffer+i, &pom->podatak, sizeof(pom->podatak));
        i += sizeof(Knjiga);
        pom = pom->sledeci;
    }

    return buffer;
}

void Deserialize(CVOR** glava, char* data, int len) {

    len /= sizeof(Knjiga);
    Knjiga* nizKnjiga = (Knjiga*)data;

    for(int i = 0; i < len; i++) {
        Push(&(*glava), nizKnjiga[i]);
    }
}