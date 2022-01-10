int period = 10;

void pwm(int pin)
{
  int val = analogRead(A0); //[0,1023]
  int vreme = map(val, 0, 1023, 0, period);
  digitalWrite(pin, HIGH);
  delay(vreme * 1000);  //upaljena je vreme sekundi
  digitalWrite(pin, LOW);
  delay((period - vreme) * 1000);
}
//paliti i gasiti lampice kao na radiu za zvuk u zavisnosti od pozicije potenciometra
void zad1()
{
  int val = analogRead(A0); //[0,1023]
  int brojLampica = map(val, 0, 1023, 0, 8);

  for(int i = 0; i < brojLampica; i++)
    digitalWrite(26 + i, digitalRead(7));

  for(int i = 0; i < 8 - brojLampica; i++)
    digitalWrite(33 - i, !digitalRead(7));
}
//lampice blinkaju, a sa potenciometrom podesiti period blinkanja
void zad2()
{
  int val = analogRead(A0); //[0,1023]
  int period = map(val, 0, 1023, 50, 400);

  for(int i = 0; i < 8; i++)
    digitalWrite(26 + i, HIGH);

  delay(period);
  
  for(int i = 0; i < 8; i++)
    digitalWrite(26 + i, LOW);
    
  delay(period);
}

void zad3()
{
  int val = analogRead(A0); //[0,1023]
  int a = map(val, 0, 1023, 0, 255);

  analogWrite(27, a);
  
}
void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+ i, OUTPUT);
    digitalWrite(26+i, LOW);
  }
  Serial.begin(9600);
}

void loop() {
  //pwm(30);
  //zad1();
  //zad2();
  zad3();
  
}
