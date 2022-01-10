void zad1()
{
  if(digitalRead(2) && digitalRead(35))
  {
    for(int i = 0; i < 8; i++)
      digitalWrite(26 + i, HIGH);
  }
  else if(digitalRead(2))
  {
    digitalWrite(29, HIGH);
    delay(100);
    digitalWrite(28, HIGH);
    delay(100);
    digitalWrite(27, HIGH);
    delay(100);
    digitalWrite(26, HIGH);
    delay(100);
    digitalWrite(29, LOW);
    digitalWrite(28, LOW);
    digitalWrite(27, LOW);
    digitalWrite(26, LOW);
    delay(100);
  }
  else if(digitalRead(35))
  {
    digitalWrite(30, HIGH);
    delay(100);
    digitalWrite(31, HIGH);
    delay(100);
    digitalWrite(32, HIGH);
    delay(100);
    digitalWrite(33, HIGH);
    delay(100);
    digitalWrite(30, LOW);
    digitalWrite(31, LOW);
    digitalWrite(32, LOW);
    digitalWrite(33, LOW);
    delay(100);
  }
  else
  {
    for(int i = 0; i < 8; i++)
      digitalWrite(26 + i, LOW);
  }
   for(int i = 0; i < 8; i++)
      digitalWrite(26 + i, LOW);
}

bool prethodno1 = false;
int poceo = 0;
int zavrsio = 0;
void zad2()
{
  if(digitalRead(4) && prethodno1 == false) //promena kad ukljucujem
  {
    poceo = millis(); //vraca vreme u millisekundama
    prethodno1 = true;
  }
  else if(!digitalRead(4) && prethodno1 == true) //pustam dugme, promena kad iskljucujem
  {
    zavrsio = millis();
    if(zavrsio - poceo > 2000) //ako je vreme dok je bilo ukljuceno vece od 2s
    {
      digitalWrite(30, !digitalRead(30));
    }
    prethodno1 = false;
  }
}


void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
  
  Serial.begin(9600);
}

void loop() {
  //zad1();
  //zad2();
}
