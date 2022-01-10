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
  if(Serial.available() > 0)
  {
    char a = Serial.read();
    
    if(a == 'a')
    {
      digitalWrite(26, !digitalRead(26)); //menjanje stanja
    }
    
    /*
    int i = 0;
    a -= '0';
    digitalWrite(26, LOW);
    digitalWrite(27, LOW);
    digitalWrite(28, LOW);
    digitalWrite(29, LOW);
    while(a != 0)
    {
      digitalWrite(26 + i, a%2);
      
      a /= 2;
      i++;
    }*/
  }
  digitalWrite(30, digitalRead(4));
  digitalWrite(31, digitalRead(34));
  digitalWrite(32, digitalRead(36));
  digitalWrite(33, digitalRead(37));
}
