/*
 * 1)  Procitati promene na button-u BTN1, kada se button pritisne upaliti lapmicu 26 i lampica gori sve dok je button pritisnut
    Kada button promeni stanje upaliti lampicu 27 koja gori dok god je button iskljucen.
    Inicijalno su sve lampice iskljucene. Implementirati funkcionalnost za sve button-e.
    BTN2 - lampice 28, 29
    BTN3 - lampice 30, 31
    BTN4 - lampice 32, 33
    Sistem radi samo ako je odgovarajuci switch podignut(BTN1 pali i gasi lampice 26 i 27 ako je sw1 podignut itd)
    7 bodova

 */
bool upaljena1 = false;
int pocetak;
int vreme;
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
  if(digitalRead(2))  //sw2
  {
    if(digitalRead(4) && !upaljena1) //promena stanja
    {
      Serial.println("Pritisnut button1");
      digitalWrite(26, HIGH);
      digitalWrite(27, LOW);
      upaljena1 = true;
      pocetak = millis();
    }
    else if(!digitalRead(4) && upaljena1) //promena stanja
    {
      Serial.println("Pusten button1 ");
      digitalWrite(26, LOW);
      digitalWrite(27, HIGH);
      upaljena1 = false;
      vreme = millis() - pocetak;
      Serial.println(vreme);
    }
  }

}
