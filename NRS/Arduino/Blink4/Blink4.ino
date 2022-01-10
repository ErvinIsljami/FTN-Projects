#define dump(v) Serial.print(v)

//************************************************************************
int digitalRead1(uint8_t pin)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  int      highLow;
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return 0;
  }
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return LOW;
  }
  iop = (p32_ioport *)portRegisters(port);
  bit = digitalPinToBitMask(pin);
  //DOVDE NAPAMET
  if ((iop->port.reg & bit) != 0) 
  {
    return HIGH;
  }
  else
  {
    return LOW;
  }
}

void digitalWrite1(uint8_t pin, int value)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  int      highLow;
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return;
  }
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return;
  }
  iop = (p32_ioport *)portRegisters(port);
  bit = digitalPinToBitMask(pin);

  if(value == HIGH)
    iop->lat.set = bit;
  else
    iop->lat.clr = bit;
}

void pinModeOut(uint8_t pin)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  int      highLow;
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return;
  }
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return;
  }
  iop = (p32_ioport *)portRegisters(port);
  bit = digitalPinToBitMask(pin);

  iop->tris.clr = bit;
}

// the setup function runs once when you press reset or power the board
void setup() 
{
  // initialize digital pin 27 as an output.
  pinModeOut(27);

  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);  
}

void loop() 
{
  delay(20);
  
  if( digitalRead1(7) )
    digitalWrite1(27, HIGH);
  else
    digitalWrite1(27, LOW);
}
