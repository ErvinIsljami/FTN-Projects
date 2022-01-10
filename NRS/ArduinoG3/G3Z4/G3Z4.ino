//podesiti period blinkanja lampica u zavisnosti od analognog ulaza

void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
 {
  pinMode(26 + i, OUTPUT);
  digitalWrite(26 + i, LOW);
 }
}

void loop() {
  // put your main code here, to run repeatedly:
  int val = analogRead(A0);
  int period = map(val, 0, 1023, 50, 200);

  delay(period);
  for(int i = 0; i < 8; i++)
    digitalWrite(26 + i, !digitalRead(26 + i));
  delay(period);
}
