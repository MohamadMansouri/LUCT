                       

#define RELAY1  7
#define RELAY2 8                        
void setup()

{    


Serial.begin(9600);
  pinMode(RELAY1, OUTPUT);       
  pinMode(RELAY2, OUTPUT);       

}

  void loop()

{

   digitalWrite(RELAY1,0);     
      digitalWrite(RELAY2,1);          
     
   Serial.println("Light ON");
   delay(10000);                                     

   digitalWrite(RELAY1,1); 
      digitalWrite(RELAY2,0);         
        
   Serial.println("Light OFF");
   delay(10000);
   
}
