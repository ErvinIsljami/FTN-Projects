int nizLampica[] = {26, 27, 28, 29};
int nizSwitcheva[] = {2, 7, 8, 35};

void setup() {
  // put your setup code here, to run once:
   for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }

  Serial.begin(9600);  
}

void fja(int x)
{
  if(digitalRead(nizSwitcheva[x]))
    digitalWrite(nizLampica[x], HIGH);
  else
    digitalWrite(nizLampica[x], LOW);
}


void loop() {
  // put your main code here, to run repeatedly:
  fja(2);
}
