/*
  Blink 4
  Turns on/off LED according to SW position.
  Reveals logic behind Arduino digitalRead function
  Napisi svoju digitalWrite i pinModeOut funkciju
 */

#define dump(v) Serial.print(v)
 

//************************************************************************
int digitalRead1(uint8_t pin)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  int      highLow;

  /* Check if pin number is in valid range.
  */
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return 0;
  }

  //* Get the port number for this pin.
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return LOW;
  }

  //* Obtain pointer to the registers for this io port.
  iop = (p32_ioport *)portRegisters(port);

  //* Obtain bit mask for the specific bit for this pin.
  bit = digitalPinToBitMask(pin);

  //dump("PORT=");dump(port); dump(" BIT=");dump(bit);
  //dump(" DATA=");dump(iop->port.reg);
  //dump("\n");

  //* Get the pin state.
  if ((iop->port.reg & bit) != 0) 
  {
    highLow = HIGH;
  }
  else
  {
    highLow = LOW;
  }

  return(highLow);
}

void digitalWrite1(uint8_t pin, uint8_t val)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  
  /* Check if pin number is in valid range.
  */
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return;
  }

  //* Get the port number for this pin.
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return;
  }

  //* Obtain pointer to the registers for this io port.
  iop = (p32_ioport *)portRegisters(port);

  //* Obtain bit mask for the specific bit for this pin.
  bit = digitalPinToBitMask(pin);

  //* Set the pin state
  if (val == LOW)
  {
      iop->lat.clr = bit;
  }
  else
  {
      iop->lat.set = bit;
  }
}

void pinModeOut(uint8_t pin)
{
  volatile p32_ioport *iop;
  uint8_t  port;
  uint16_t bit;
  
  // Check if pin number is in valid range.
  if (pin >= NUM_DIGITAL_PINS_EXTENDED)
  {
    return;
  }

  //* Get the port number for this pin.
  if ((port = digitalPinToPort(pin)) == NOT_A_PIN)
  {
    return;
  }

  //* Obtain pointer to the registers for this io port.
  iop = (p32_ioport *)portRegisters(port);

  //* Obtain bit mask for the specific bit for this pin.
  bit = digitalPinToBitMask(pin);

  iop->tris.clr = bit;      // make the pin an output
  iop->lat.clr  = bit;      // clear to output bit   
}

// the setup function runs once when you press reset or power the board
void setup() 
{
  // initialize digital pin 27 as an output.
  //pinMode(27, OUTPUT);
  pinModeOut(27);

  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);  
}

// the loop function runs over and over again forever
void loop() 
{
  delay(200);
  
  if( digitalRead1(7) )
    digitalWrite1(27, HIGH);
  else
    digitalWrite1(27, LOW);
}
