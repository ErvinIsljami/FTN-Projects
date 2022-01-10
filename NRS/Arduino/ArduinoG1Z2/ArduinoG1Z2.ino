// ako btn1 drzimo pritisnutim duze od 2s kad ga pustimo upalice sve parne lampice
// u loop-u ako pritisnemo btn4 upali neparne lampice
// task sa parnim lampicama radi samo ako je podignut sw1
bool isPritisnuto = false;
int pocetak;
int kraj;
int task_id;

void task(int id, void * tptr)
{
  Serial.println("ukljucen");
  if(digitalRead(4) == HIGH && isPritisnuto == false) //proveravam da li je pritisnuto dugme
  {
    pocetak = millis();
    isPritisnuto = true;
  }
  else if(digitalRead(4) == LOW && isPritisnuto == true)
  {
    kraj = millis();
    isPritisnuto = false;
    if(kraj - pocetak > 2000)
    {
      digitalWrite(26, HIGH);
      digitalWrite(28, HIGH);
      digitalWrite(30, HIGH);
      digitalWrite(32, HIGH);
      delay(1000);
      digitalWrite(26, LOW);
      digitalWrite(28, LOW);
      digitalWrite(30, LOW);
      digitalWrite(32, LOW);
    }
  }
}
void setup() {
  for(int i = 0; i < 8; i++)
  {
    pinMode(26+i, OUTPUT);
    digitalWrite(26+i, LOW);
  }
  
  Serial.begin(9600);
  
  if(digitalRead(4))  //btn1
    isPritisnuto = true;
  
  task_id = createTask(task, 10, TASK_ENABLE, NULL);
  if(digitalRead(2))  //sw1
  {
    if(getTaskState(task_id) == TASK_DISABLE)
      setTaskState(task_id, TASK_ENABLE);
  }
  else
  {
     if(getTaskState(task_id) == TASK_ENABLE)
      setTaskState(task_id, TASK_DISABLE);
  }
  
}

void loop() {
  if(digitalRead(2))  //sw1
  {
    setTaskState(task_id, TASK_ENABLE);
    Serial.println("ukljucio");
  }
  else
  {
    setTaskState(task_id, TASK_DISABLE);
  }
  
  if(digitalRead(37))
  {
      digitalWrite(27, HIGH);
      digitalWrite(29, HIGH);
      digitalWrite(31, HIGH);
      digitalWrite(33, HIGH);
  }
  else
  {
      digitalWrite(27, LOW);
      digitalWrite(29, LOW);
      digitalWrite(31, LOW);
      digitalWrite(33, LOW);
  }
}
