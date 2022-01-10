bool pritisnut = false;
int poceo;
int zavrsio;
int task_id;

void zadatak(int id, void * tptr) 
{
  if(digitalRead(4) == HIGH && pritisnut == false)
  {
    poceo = millis(); //vraca vreme u ms od kad je plocica ukljucena
    pritisnut = true;
  }
  else if(digitalRead(4) == LOW && pritisnut == true)
  {
    pritisnut = false;
    zavrsio = millis();
    if(zavrsio - poceo > 2000)  //vise od 2s
    {
      executeSoftReset(RUN_SKETCH_ON_BOOT);
    }
  }
}




void setup() {
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
  Serial.begin(9600);

  task_id = createTask(zadatak, 10, TASK_ENABLE, NULL);
}

void loop() {

  

}
