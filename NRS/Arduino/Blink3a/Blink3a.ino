// definisemo pokazivac na funkciju tipa void bez parametara
typedef void (*interruptFunc)();

// definisemo promenljive za rad sa interaptom za dugme BTN1
int BTN1 = 4;
int BTN1_old_state;
int BTN1_new_state;
interruptFunc BTN1_f_rising;
interruptFunc BTN1_f_falling;

// postavljanje interapta za dugme1
int attachInterupt1(interruptFunc f, int mode){
  if (mode == RISING)
     BTN1_f_rising = f;
  else
     BTN1_f_falling = f;
}

// uklanjanje interapta za dugme1
int deattachInterupt1(int mode){
  if (mode == RISING)
     BTN1_f_rising = NULL;
  else
     BTN1_f_falling = NULL;
}

// funkcija koja pali lampicu 27
void myInterruptOn(){
   digitalWrite(27, HIGH);
   delay(100);
   digitalWrite(27, LOW);
   delay(100);
   digitalWrite(27, HIGH);
   delay(100);
   digitalWrite(27, LOW);
   delay(100);
   digitalWrite(27, HIGH);
   delay(100);
   digitalWrite(27, LOW);
   delay(100);
}

// funkcija koja gasi lampicu 33
void myInterruptOff(){
   digitalWrite(33, HIGH);
   delay(100);
   digitalWrite(33, LOW);
   delay(100);
   digitalWrite(33, HIGH);
   delay(100);
   digitalWrite(33, LOW);
   delay(100);
   digitalWrite(33, HIGH);
   delay(100);
   digitalWrite(33, LOW);
   delay(100);
}

void setup() {
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);
  }
 BTN1_old_state = digitalRead(BTN1);
 BTN1_f_rising = NULL;
 BTN1_f_falling = NULL; 
 attachInterupt1(myInterruptOn, RISING);
 attachInterupt1(myInterruptOff, FALLING);
}

void loop() {
  BTN1_new_state = digitalRead(BTN1);
  if (BTN1_new_state == 1 && BTN1_old_state == 0){
      if (BTN1_f_rising!=NULL)
         (*BTN1_f_rising)();
  } else if (BTN1_new_state == 0 && BTN1_old_state == 1){
      if (BTN1_f_falling!=NULL)
         (*BTN1_f_falling)();
  }
  BTN1_old_state = BTN1_new_state;
  delay(20);
}
