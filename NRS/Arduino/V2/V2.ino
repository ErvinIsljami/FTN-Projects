extern "C" void __attribute__((interrupt(),nomips16)) Timer1Handler(void);

#define bit2mask(bit) (1<<bit) 
enum gpio_ports { PORT_A=1, PORT_B, PORT_C, PORT_D, PORT_E, PORT_F, PORT_G };
unsigned int gpio_base_addr[PORT_G] = 
    {0xBF886000, 0xBF886040, 0xBF886080, 0xBF8860C0, 0xBF886100, 0xBF886140, 0xBF886180 };
enum gpio_regs { GPIO_REG=0, GPIO_CLR, GPIO_SET, GPIO_INV };  

int frequency = 1; // Podesiti po potrebi

int task_id;    //dodali zbog taskova
//************************************************************************
void EnableTimer(void)
{
  T1CON = TACON_PS_256;                             // izbor preskalera
  
  TMR1 = 0;                                         // Obrisi Timer1 counter
  PR1 = (__PIC32_pbClk / 256 / frequency);         
  // Serial.println(PR1);
  
  // inicijalizacija prekida
  setIntVector(_TIMER_1_VECTOR, Timer1Handler);
  clearIntFlag(_TIMER_1_IRQ);
  setIntPriority(_TIMER_1_VECTOR, _T1_IPL_IPC, _T1_SPL_IPC);
  setIntEnable(_TIMER_1_IRQ);  

  T1CONSET = TACON_ON;      // pusti ga da broji
}
extern "C"
{
  void __attribute__((interrupt(),nomips16)) Timer1Handler(void)
  {
    //ovde ide logika timer-a



    clearIntFlag(_TIMER_1_IRQ);
  }
};


void taskFun(int id, void *ptr)
{
  //isto radite logiku
  
}

void setup() 
{



  task_id = createTask(taskFun, 30, TASK_ENABLE, NULL); //30 je na koliko ms ce se ponavljati task
}

void loop() 
{
  
}
