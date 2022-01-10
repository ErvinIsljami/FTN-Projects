/*
  Blink 5
  Turns on/off LED according to SW position.
  Use digitalRead2 function whit direct access to GPIO registers
  ZADATAK: napraviti direktne pinModeOut i digitalWrite funkcije
 */

#define dump(v) Serial.print(v)

#define bit2mask(bit) (1<<bit) 
enum gpio_ports { PORT_A=1, PORT_B, PORT_C, PORT_D, PORT_E, PORT_F, PORT_G };
unsigned int gpio_base_addr[PORT_G] = 
    {0xBF886000, 0xBF886040, 0xBF886080, 0xBF8860C0, 0xBF886100, 0xBF886140, 0xBF886180 };
enum gpio_regs { GPIO_REG=0, GPIO_CLR, GPIO_SET, GPIO_INV }; 

//************************************************************************
int digitalRead2(int port, int bit)
{
  int highLow = LOW;
  volatile uint32_t *port_addr;

  // proveri broj porta i bita
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return -1;
  
  // namesti masku pozicionu  
  int bmask = bit2mask( bit );
  
  // namesti adresu
  port_addr = (uint32_t *)(gpio_base_addr[port - 1] + 0x10); 

  // ucitaj sadrzaj registra
  uint32_t reg = *port_addr;

  //dump("PORT=");dump(port); dump(" BIT=");dump(bit);
  //dump(" DATA=");dump(reg);
  //dump("\n");

  //* Get the pin state.
  if ((reg & bmask) != 0) 
  {
    highLow = HIGH;
  }
  else
  {
    highLow = LOW;
  }
  return(highLow);
}

void pinModeOut(int port, int bit)
{
  // proveri broj porta i bita
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return;
  
  // namesti masku pozicionu  
  int bmask = bit2mask( bit );
  
  // namesti adrese
  volatile uint32_t *tris_addr = (uint32_t *)gpio_base_addr[port - 1];
  //volatile uint32_t *lat_addr = (uint32_t *)(gpio_base_addr[port - 1] + 0x20);
  volatile uint32_t *lat_addr = tris_addr + 8;

  // ucitaj sadrzaj registra
  *(tris_addr + GPIO_CLR) = bmask;      // set OUTPUT
  *(lat_addr + GPIO_CLR) = bmask;       // set val = 0
}

void digitalWrite2(int port, int bit, bool val)
{
  // proveri broj porta i bita
  if( (port < PORT_B) || (port > PORT_G) || (bit > 15) )
    return;
  
  // namesti masku pozicionu  
  int bmask = bit2mask( bit );
  
  // namesti adrese
  volatile uint32_t *lat_addr = (uint32_t *)(gpio_base_addr[port - 1] + 0x20);

  //* Set the pin state
  if (val == LOW)
  {
      *(lat_addr + GPIO_CLR) = bmask;
  }
  else
  {
      *(lat_addr + GPIO_SET) = bmask;
  }
}

// the setup function runs once when you press reset or power the board
void setup() 
{
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);  
  // initialize digital pin 27 as an output. Proveri u Board_Data.c
  pinModeOut(PORT_E, 1);
}

// the loop function runs over and over again forever
void loop() 
{
  delay(2000);
  
  if( digitalRead2(PORT_D, 9) )
    digitalWrite2(PORT_E, 1, HIGH);
  else
    digitalWrite2(PORT_E, 1, LOW);

  //Serial.println( digitalRead2(PORT_E, 1));
}
