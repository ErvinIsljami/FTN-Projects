//svaki prekidac predstavlja poziciju binarne cifre u binarnom sistemu
//ukljuciti onoliko lampica koji je broj ispisan prekidacima

void setup() {
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }

  Serial.begin(9600);

}

void loop() {
  int broj = 0;
  if(digitalRead(2)) //sw1
    broj += 1;
  if(digitalRead(7)) //sw2
    broj += 2;
  if(digitalRead(8)) //sw3
    broj += 4;
  if(digitalRead(35)) //sw4
    broj += 8;
    
  if(broj >= 8)
    broj = 8;

  for(int i = 0; i < broj; i++)
    digitalWrite(26 + i, HIGH);

  for(int i = 0; i < 8 - broj; i++)
    digitalWrite(33 - i, LOW);

    

}
