void zad1()
{
  int val = analogRead(A0);
  int brojLampica = map(val, 0, 1023, 0 , 8);
  for(int i = 0; i < brojLampica; i++)
    digitalWrite(26 + i, HIGH);

  for(int i = 0; i < 8 - brojLampica; i++)
    digitalWrite(33 - i, LOW);
}

void zad2()
{
  int val = analogRead(A0);
  int period = map(val, 0, 1023, 50 , 250);
  
  for(int i = 0; i < 8; i++)
    digitalWrite(26 + i, HIGH);

  delay(period);
  
  for(int i = 0; i < 8 ; i++)
    digitalWrite(33 - i, LOW);

  delay(period);
}
void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
}

void loop() {
 
  //zad1();
  zad2();
  
  
}
