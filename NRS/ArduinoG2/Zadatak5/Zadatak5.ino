/*
 * Ako potenciometar predje pola onda blinka prvih 4 lampica, ako ne predje onda blinka drugih 4
 * Funkcionalnost omoguciti paljenjem switcha3
 */
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
  if(digitalRead(8))
  {
    int val = analogRead(A0); //uvek ovako citaomo potenciometar
    if(val > 512)
    {
      digitalWrite(26, !digitalRead(26));
      digitalWrite(27, !digitalRead(27)); 
      digitalWrite(28, !digitalRead(28)); 
      digitalWrite(29, !digitalRead(29)); 
    }
    else
    {
      digitalWrite(30, !digitalRead(30));
      digitalWrite(31, !digitalRead(31)); 
      digitalWrite(32, !digitalRead(32)); 
      digitalWrite(33, !digitalRead(33)); 
    }
  
    delay(100);
  }
}
