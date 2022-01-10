void fun()
{
  if(digitalRead(2))
    digitalWrite(26, HIGH);
  else
    digitalWrite(26, LOW);

  if(digitalRead(7))
    digitalWrite(27, HIGH);
  else
    digitalWrite(27, LOW);

  if(digitalRead(8))
    digitalWrite(28, HIGH);
  else
    digitalWrite(28, LOW);

  if(digitalRead(35))
    digitalWrite(29, HIGH);
  else
    digitalWrite(29, LOW);
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
  fun();
}
