// lampice se pale i gase tako sto idu sa leva na desno i kada dodje do kraja obrne smer
int brojac = 0;
bool isLevo = true;


void setup() {
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+i, OUTPUT);
    digitalWrite(26+i, LOW);
  }

  Serial.begin(9600); //uspostavlja uart komunikaciju


}

void loop() {
  if(isLevo)
  {
    digitalWrite(26 + brojac, HIGH);
    delay(200);
    digitalWrite(26 + brojac, LOW);
  }
  else
  {
    digitalWrite(33 - brojac, HIGH);
    delay(200);
    digitalWrite(33 - brojac, LOW);
  }
  
  brojac++;
  if(brojac == 8)
  {
    brojac = 0;
    isLevo = !isLevo;
  }
/*
  for(int i = 0; i < 8; i++)
  {
    digitalWrite(26 + i, HIGH);
    delay(200);
    digitalWrite(26 + i, LOW);
  }
  for(int i = 0; i < 8; i++)
  {
    digitalWrite(33 - i, HIGH);
    delay(200);
    digitalWrite(33 - i, LOW);
  }
  */
}
