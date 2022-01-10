bool upaljen = false;
int count = 0;
int pocetak;

void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }

  if(digitalRead(4))
    upaljen = true;
  else
    upaljen = false;

  Serial.begin(9600);
}

void loop() {
  if(digitalRead(4) && !upaljen)
  {
    upaljen = true;
    pocetak = millis(); //millis mi pamti trenutno vreme
    count++;
    Serial.print("Button je pritisnut ");
    Serial.print(count);
    Serial.println(" puta");
    digitalWrite(26, HIGH);
  }
  else if(!digitalRead(4) && upaljen)
  {
    upaljen = false;
    Serial.println("Button je pusten");
    digitalWrite(26, LOW);
    if(millis() - pocetak > 2000)
      Serial.println("Vise od dve sekunde");

   
  }
  
}
