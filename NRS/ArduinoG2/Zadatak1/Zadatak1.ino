int brojac = 0;
bool kaCentru = false;
void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+ i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
}



void loop() {
  int sw1 = digitalRead(2);
 int sw2 = digitalRead(35);
  if(kaCentru)
  { 
    if(sw1 == HIGH)
      digitalWrite(26 + brojac, HIGH);
    if(sw2 == HIGH)
      digitalWrite(33 - brojac, HIGH);
    
    delay(200);
    if(sw1 == HIGH)
      digitalWrite(26 + brojac, LOW);
    if(sw2 == HIGH)
      digitalWrite(33 - brojac, LOW);
  }
  else
  {
    
    
    if(sw2 == HIGH)
      digitalWrite(30 + brojac, HIGH);
    if(sw1 == HIGH)
      digitalWrite(29 - brojac, HIGH);
    
    delay(200);
    if(sw2 == HIGH)
      digitalWrite(30 + brojac, LOW);
    if(sw1 == HIGH)
      digitalWrite(29 - brojac, LOW);
  }

  brojac++;
  if(brojac == 4)
  {
    brojac = 0;
    kaCentru = !kaCentru;
  }
}
