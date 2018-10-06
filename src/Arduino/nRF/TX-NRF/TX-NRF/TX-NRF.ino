#include <SPI.h>   
#include "RF24.h"  

RF24 myRadio ( 8,7); 

byte addresses[][6] = {"1Node"};
int dataTransmitted;  

void setup()   
{
  pinMode(0,INPUT);
  delay(1000);
  
  myRadio.begin();  
  myRadio.setChannel(108);  
 
  myRadio.setPALevel(RF24_PA_MIN);
 

  myRadio.openWritingPipe( addresses[0]); 
  delay(1000);
}


void loop()   
{int a=analogRead(a);
Serial.println(a);
if(a<700)
dataTransmitted=1;
else dataTransmitted=0;
  myRadio.write( &dataTransmitted, sizeof(dataTransmitted) ); //  Transmit the data
  delay(500);

}
