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
  // put your main code here, to run repeatedly:
  if(digitalRead(7))
  {
    digitalWrite(26, HIGH);
  }
  else
  {
    digitalWrite(26, LOW);
  }

  delay(50);
}
