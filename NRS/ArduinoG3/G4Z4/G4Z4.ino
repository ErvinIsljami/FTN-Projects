int task_id;

void funkcija(int id, void *param)
{
  digitalWrite(26, !digitalRead(26));  
}



void setup() {
  // put your setup code here, to run once:
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }

  Serial.begin(9600);

  task_id = createTask(funkcija, 50, TASK_ENABLE, NULL);
  
}

void loop() {
  // put your main code here, to run repeatedly:

}
