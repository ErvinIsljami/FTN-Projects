void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 +i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }

  Serial.begin(9600);
  
}

void loop() {
  int val = analogRead(A0); 
  int vrednost = map(val, 0, 1023, 0, 8);

  for(int i = 0; i < vrednost; i++)
    digitalWrite(26 + i, digitalRead(7));

  for(int i = 0; i < 8 - vrednost; i++)
    digitalWrite(33 - i, !digitalRead(7));
  


}
