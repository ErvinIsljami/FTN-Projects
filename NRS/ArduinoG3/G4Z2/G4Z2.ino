void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+i, OUTPUT);
    digitalWrite(26+i, LOW);
  }

  Serial.begin(9600);
  
  
 
}

void loop() {
  // put your main code here, to run repeatedly:
  digitalWrite(26, HIGH);
  delay(2000);
  digitalWrite(27, HIGH);
  delay(500);
  digitalWrite(26, LOW);
  digitalWrite(27, LOW);
  digitalWrite(28, HIGH);
  delay(2000);
  digitalWrite(28, LOW);
  digitalWrite(27, HIGH);
  delay(500);
  digitalWrite(27, LOW);

  
}
