int brojac = 0;

void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+ i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
  Serial.begin(9600);
}

void fun1()
{
  // put your main code here, to run repeatedly:
  int analogValue = analogRead(A0);
  int brojLampica = map(analogValue, 0, 1023, 0, 8);
  for(int i = 0; i < brojLampica; i++)
    digitalWrite(26 + i, HIGH);

  for(int i = 0; i < 8 - brojLampica; i++)
    digitalWrite(33 - i, LOW);
}



void loop() {
  
  int analogValue = analogRead(A0);
  int brzina = map(analogValue, 0, 1023, 50, 400);
  digitalWrite(26 + brojac, HIGH);
  delay(brzina);
  digitalWrite(26 + brojac, LOW);
  brojac++;
  brojac = brojac % 9;
}
