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
  digitalWrite(26, HIGH);
  digitalWrite(27, HIGH);
  delay(5000);
  digitalWrite(28, HIGH);
  digitalWrite(29, HIGH);
  delay(1000);
  digitalWrite(26, LOW);
  digitalWrite(27, LOW);
  digitalWrite(28, LOW);
  digitalWrite(29, LOW);
  digitalWrite(30, HIGH);
  digitalWrite(31, HIGH);
  delay(5000);
  digitalWrite(30, LOW);
  digitalWrite(31, LOW);
  digitalWrite(28, HIGH);
  digitalWrite(29, HIGH);
  delay(1000);
  digitalWrite(28, LOW);
  digitalWrite(29, LOW);
}
