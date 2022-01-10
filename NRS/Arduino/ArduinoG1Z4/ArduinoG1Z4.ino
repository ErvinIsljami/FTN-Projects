// cita sa analognog ulaza
// kad ucita broj prve 4 lampice treba da svetle kao taj broj u binarnom obliku

void upaliBinarno(int n)
{
  int i = 26;
  while(n != 0)
  {
    Serial.print(n%2);
    Serial.print(" ");
    Serial.println(n);
    
    digitalWrite(i, n%2);   //upisujemo ostatak pri deljenju sa dvojkom
    n = n>>1;
    
    i++;
  }
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  for(int i = 0; i < 8; i++)
  {
    pinMode(26 + i, OUTPUT);
    digitalWrite(26 + i, LOW);    
  }
}

void loop() {
  if(Serial.available() > 0)
  {
    char a = Serial.read();
    a -= '0';   //od karaktera 7 dobijemo broj 7

    int i = 26;
  while(a != 0)
  {
    Serial.print(a%2);
    Serial.print(" ");
    Serial.println(a);
    
    digitalWrite(i, a%2);   //upisujemo ostatak pri deljenju sa dvojkom
    a = a>>1;
    
    i++;
  }
  }

}
