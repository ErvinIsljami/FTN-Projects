
 /*
  * Blink 3
  Blink LED1 with a 200ms cycle (5Hz)
  Utilizing the Timer1 rather than delay functions.
 */

extern "C" void __attribute__((interrupt(),nomips16)) Timer1Handler(void);

int pin = 33;           // LED pin
int frequency = 5;      // Hz
int PRS = 5;
//************************************************************************
void EnableTimer(void)
{
  T1CON = TACON_PS_256;                             // izbor preskalera
  
  TMR1 = 0;                                         // Obrisi Timer1 counter
  PR1 = ((__PIC32_pbClk / 2) / 256 / frequency);    // 80 MHz je CPU takt
  // Serial.println(PR1);
  
  // inicijalizacija prekida
  setIntVector(_TIMER_1_VECTOR, Timer1Handler);
  clearIntFlag(_TIMER_1_IRQ);
  setIntPriority(_TIMER_1_VECTOR, _T1_IPL_IPC, _T1_SPL_IPC);
  setIntEnable(_TIMER_1_IRQ);  

  T1CONSET = TACON_ON;      // pusti ga da broji
}

void disableTimer(void)
{
  clearIntEnable(_TIMER_1_IRQ);
  T1CON = 0;
  T1CONCLR = TACON_ON;      // zaustavi brojac
  clearIntVector(_TIMER_1_VECTOR);
}

//*  we need the extern C so that the interrupt handler names dont get mangled by C++
extern "C"
{
void __attribute__((interrupt(),nomips16)) Timer1Handler(void)
{
  static int count200 = 0;
  static int count = 0;
  count++;
  if(count == 5)
  {
  digitalWrite(pin, !digitalRead(pin));
  count200++;
  if (count200 == 200)
     disableTimer();
  
  count = 0;
  }
  // clear the interrupt flag
  clearIntFlag(_TIMER_1_IRQ);
  Serial.println(TMR1);
}
};  //* extern "C"

void pause()
{
  T1CONCLR = 1<<15;
}

void unpause()
{
  T1CONSET = 1<<15;
}



void setup() 
{
  Serial.begin(9600);
  pinMode(pin, OUTPUT);
  // Init the timer.
  EnableTimer();
}

void loop() 
{
    if(digitalRead(7))
      unpause();
    else
      pause();
}
