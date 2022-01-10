/*
  UART Demo 1
  Receive and send back character
 */

volatile uint32_t *u1mode_reg = (uint32_t *) 0xBF806000;

#define UTXBF_MSK 0x0200   // bit 9 (1 << 9)
volatile uint32_t *u1sta_reg  = (uint32_t *) 0xBF806010;

volatile uint32_t *u1tx_reg   = (uint32_t *) 0xBF806020;
volatile uint32_t *u1rx_reg   = (uint32_t *) 0xBF806030;
volatile uint32_t *u1brg_reg  = (uint32_t *) 0xBF806040;

void uart_send( char b )
{
  // sacekaj ako nema mesta u Tx bufferu
  while( (*u1sta_reg & UTXBF_MSK) == 1 );
  *u1tx_reg = b;
}

char uart_rec()
{
  return *u1rx_reg;
}

bool uart_available()
{
  return *u1sta_reg & 1; //bit maska za nulti bit je 1
}

void uart_begin(int baudRate)
{
  *u1sta_reg = 0;
  *u1mode_reg = 0;

  *u1brg_reg =  ((__PIC32_pbClk / 16 / baudRate) - 1);
  *(u1mode_reg + 2) = 1<<15;  //mode ima reg,clr,set,inv podregistre, kad se povecamo za 2 pozicioniramo se na set
  *(u1sta_reg + 2) = 1<<10 + 1<<12;
  
}
// the setup function runs once when you press reset or power the board
void setup() 
{
  // initialize serial communication at 9600 bits per second:
  uart_begin(9600);  
}

// the loop function runs over and over again forever
void loop() 
{
  Serial.println("fasdfa"); 
  // send data only when you receive data:
  if (uart_available() ) 
  {
    
    // read the incoming byte:
    char inByte = uart_rec();
    // say what you got:
    uart_send(inByte);
  }


    
}
