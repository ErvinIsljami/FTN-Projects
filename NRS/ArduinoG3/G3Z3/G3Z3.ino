
void fun1()
{
  // put your main code here, to run repeatedly:
  int val = analogRead(A0);
  int brLampica = map(val, 0, 1023, 0, 8);
  for(int i = 0; i < brLampica; i++)
    digitalWrite(26 + i, HIGH);
    
  for(int i = 0; i < 8 - brLampica; i++)
    digitalWrite(33 - i, LOW);
}

void pwm()
{
  int val = analogRead(A0);
  int t = map(val, 0, 1023, 0, 10);
  digitalWrite(33, HIGH);
  delay(t * 1000);
  digitalWrite(33, LOW);
  delay((10 - t) * 1000);
}



void setup() {
  // put your setup code here, to run once:
   for(int i = 0; i < 8; i++)
   {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
   }

   
}

void loop() {
  
digitalWrite(30, HIGH);
delay(100);
digitalWrite(30, LOW);
  
  
}
