#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct KORISNIK_ST
{
	int rbr;
    char ime[30];
    char prezime[30];
    int godina;
}KORISNIK;

typedef struct CVOR_ST
{
    KORISNIK inf;
    struct CVOR_ST* left;
	struct CVOR_ST* right;
}CVOR;

void addNew(CVOR ** root, KORISNIK novi);
void printTree(CVOR *root);
void clear(CVOR **root);
void load(FILE*in, CVOR **root);
void obrni(CVOR *oldRoot, CVOR **newRoot);
void addNewReverse(CVOR ** root, KORISNIK new);

int main()
{
	CVOR *root = NULL;	
    FILE *in;
    in = fopen("stablo.txt","r");
    load(in, &root);
    fclose(in);
    printTree(root);
  
 	printf("*************************\n");
  	CVOR *newRoot = NULL;
  	obrni(root, &newRoot);
  	printTree(newRoot);

  	printf("%s == %s \n", root->inf.ime, newRoot->inf.ime);
  	printf("%s == %s \n", root->left->inf.ime, newRoot->left->inf.ime);

	return 0;
}

void load(FILE*in, CVOR **root)
{
	int n,i;
	fscanf(in,"%d",&n);
	for(i = 0; i < n; i++)
	{
		KORISNIK new;
		fscanf(in, "%d %s %s %d",&new.rbr, new.ime, new.prezime, &new.godina);
		addNew(&(*root), new);
	}
}

void addNew(CVOR ** root, KORISNIK new)
{
	if (*root == NULL)
	{
		CVOR* temp = (CVOR*)malloc(sizeof(CVOR));
		temp->inf = new;
		temp->left = NULL;
		temp->right = NULL;
		*root = temp;
	}
	else
	{
		if ( (*root)->inf.rbr < new.rbr )
		{
			addNew(&(*root)->right, new);
		}
		else
		{
			addNew(&(*root)->left, new);
		}
	}
}

void printTree(CVOR * root)
{
	if (root != NULL)
	{
		printTree(root->left);

            printf("%s %s %d \n",root->inf.ime,  root->inf.prezime, root->inf.godina);

		printTree(root->right);
	}

}

void clear(CVOR **root)
{
    if(*root != NULL)
    {
        
    	clear(&(*root)->left);
    	clear(&(*root)->right);
        if( (*root)->left == NULL && (*root)->right == NULL)
        {
            CVOR* pom = *root;
            free(pom);
            *root = NULL;
        }
        
    }
}

void obrni(CVOR *oldRoot, CVOR **newRoot)
{
	if(oldRoot != NULL)
	{

		KORISNIK newEL = oldRoot->inf;

		addNewReverse(&(*newRoot), newEL);
		
		obrni(oldRoot->left, &(*newRoot));

		obrni(oldRoot->right, &(*newRoot));
		
	}
	
}
void addNewReverse(CVOR ** root, KORISNIK new)
{
	if (*root == NULL)
	{
		CVOR* temp = (CVOR*)malloc(sizeof(CVOR));
		temp->inf = new;
		temp->left = NULL;
		temp->right = NULL;
		*root = temp;
	}
	else
	{
		if ( (*root)->inf.rbr > new.rbr )
		{
			addNew(&(*root)->right, new);
		}
		else
		{
			addNew(&(*root)->left, new);
		}
	}
}