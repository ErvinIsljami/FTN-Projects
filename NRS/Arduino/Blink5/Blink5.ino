/*
  Blink 5
  Turns on/off LED according to SW position.
  Use digitalRead2 function whit direct access to GPIO registers
  ZADATAK: napraviti direktne pinModeOut i digitalWrite2 funkcije
 */

#define dump(v) Serial.print(v)

#define bit2mask(bit) (1<<bit) 
enum gpio_ports { PORT_A=1, PORT_B, PORT_C, PORT_D, PORT_E, PORT_F, PORT_G };
unsigned int gpio_base_addr[PORT_G] = 
    {0, 0xBF886050, 0xBF886090, 0xBF8860D0, 0xBF886110, 0xBF886150, 0xBF886190 };
 
//************************************************************************
int digitalRead2( int port, int bit)
{
  int highLow = LOW;
  volatile uint32_t *port_addr;
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return -1;
    
  int bmask = bit2mask( bit );
  port_addr = (uint32_t *)(0xBF886110)

  if ((*port_addr & bmask) != 0) 
  {
    return HIGH;
  }
  else
  {
    return LOW;
  }
  
}

void digitalWrite2(int port, int bit, int value)
{
  int highLow = LOW;
  volatile uint32_t *port_addr;
  volatile uint32_t *port_addr2;
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return;
    
  int bmask = bit2mask( bit );
  port_addr = (uint32_t *)(gpio_base_addr[port - 1] + 0x10 + 0x04);
  port_addr2 = (uint32_t *)(gpio_base_addr[port - 1] + 0x10 + 0x08);
  if (value == HIGH) 
  {
    *port_addr2 = bmask;
  }
  else
  {
    *port_addr = bmask;
  }
}

void pinModeOut(int port, int bit)
{
  int highLow = LOW;
  volatile uint32_t *port_addr;
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return;
    
  int bmask = bit2mask( bit );
  port_addr = (uint32_t *)(gpio_base_addr[port - 1] - 0x10 + 0x04);

  *port_addr = bmask; 
}

// the setup function runs once when you press reset or power the board
void setup() 
{
  // initialize digital pin 27 as an output.
  pinModeOut(PORT_E, 1);

  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);  
}

// the loop function runs over and over again forever
void loop() 
{
  delay(20);
  
  if( digitalRead2(PORT_D, 9) )
    digitalWrite2(PORT_E, 1, HIGH);
  else
    digitalWrite2(PORT_E, 1, LOW);
}
