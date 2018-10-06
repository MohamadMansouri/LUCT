#include <SPI.h>   
#include "RF24.h"  

RF24 myRadio ( 9,10); 

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
if(a<650)
dataTransmitted=1;
else dataTransmitted=0;
  myRadio.write( &dataTransmitted, sizeof(dataTransmitted) ); //  Transmit the data
  delay(500);

}
