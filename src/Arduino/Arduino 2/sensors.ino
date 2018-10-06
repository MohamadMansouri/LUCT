  #include <dht.h>
  #include <NewPing.h>
  #include <Chrono.h>
  #include <Wire.h>
  #include <SoftwareSerial.h>
  #include <SPI.h>   /
  #include "RF24.h" 
  SoftwareSerial SIM900(2, 3);
  
  
  
  
  #define TRIGGER_PIN 7
  #define ECHO_PIN     6
  #define temp_hum A0
  #define pir_pin 8
  #define current_pin1 A1
  #define switch1 5
  #define switch2 4
  #define current_pin2 A2



RF24 myRadio (9,10); 
byte addresses[][6] = {"1Node"}; 
int dataReceived=0; 

  
  float water_level=0;
  float temprature=0;
  float humidity=0;
  float detected=0;
  float current1=0;
  float current2=0;
  int pills=0;
  int rflag=-1;  




  
  int Switch1=0;
  int Switch2=0;
  int sflag=0;
  int sendsms=0;





   
  int phoneQouta=1;
  String phone[10]={"+96170878739"};
  
  
  
  
  


  
  byte dataSent[9];
  int motion=0;
  #define voltage 220;
  #define cn 3
  String Commands[cn]={"log","shutdown","sleep mode"};
  String stext[6]={" ","succesfully shutdowned" , "have a good sleep" , "time to take your pill" , "several bad attempts to open you door" , "your water tank is empty"};
  String rtext;
  String number="";
  
  
  void func()
  {
  String a=String("your log is: hum=");a += humidity;
  String b="  temp=";b+=temprature;
  String c="  water level=";c+=(10-water_level)*10;
  String d="  motion=";d+=detected;
  String e="  energy consumption";e+=(current1+current2)*220*0.9;
  a+=b;
  a+=c;
  a+=d;
  a+=e;
  
  stext[0]=a;
  }
  
  
  
  Chrono timer1(Chrono::SECONDS);
  dht temphum; 







  
  
  NewPing Water(TRIGGER_PIN, ECHO_PIN, 3000);
  
 /* void electrical()
  {float average1 = 0,average2 = 0;
   for(int i = 0; i < 1000; i++) {
       average1 = average1 + (.0264 * analogRead(current_pin1) -13.51);
              average2 = average2 + (.0264 * analogRead(current_pin2) -13.51);

       delay(1);}
  current1=average1/1000;
    current2=average2/1000;

  power1=current1*voltage;
  }
  
  */
  
   void temp()
   {
    
    temphum.read11(temp_hum);
    temprature=temphum.temperature;
   
   }
  
  
  
   
   
   void hum()
   {
    
    temphum.read11(temp_hum);
    humidity=temphum.humidity;
    
    }
  
  
  
  
   
   
   void Water_level()
   { 
    float y;
  water_level=Water.ping_cm();
  for(int i=0;i<30;i++)
  { y=Water.ping_cm();
    if(water_level<y)
  water_level=y;}
    delay(1);
    
    }
  
  
  
  
  
  
   void Detection()
  {
       if(!detected)
    
       {
          motion=digitalRead(pir_pin);
          detected=motion;
          timer1.restart();
   
        }
      else 
        {
          if(timer1.elapsed()>5)
            {
              detected=0;
              }
          }
  motion=digitalRead(pir_pin);
  if(motion)  timer1.restart();
  
   }
  
bool on1=0;
bool on2=0;

  void switchLamp()
  {
   if (Switch1==1) if(on1==0)  { on1=1;}
                    else  on1=0;
   if (Switch2==1)  if (on2==0) { on2=1;}
                    else on2=0;


  delay(500);
  }
  
 
  
  
  
  
  void State()
  {
    float x=analogRead(current_pin1);
  for(int i=0;i<500;i++)
  {float y=analogRead(current_pin1);
  x=max(x,y);
  }
  current1=x*5/1024*0.035;

  float z=analogRead(current_pin2);
  for(int i=0;i<500;i++)
  {float y=analogRead(current_pin2);
  z=max(z,y);
  }
  current2=z*5/1024*0.038*7;


  }
  
  
  

  
  void recieveSMS()
  {
    char incoming_char;
    String test="";
    while(SIM900.available() >0)
    {
      
      incoming_char=SIM900.read();

       test+=incoming_char;
       delay(100);
    }
       Serial.println(test);
       
       if(test.indexOf("+CMT:")>-1)
       {
        Serial.println("found");
        int i=0;
        
         rflag=0;
     /*   
       for(int i=0;i<test.length()-test.indexOf("+08\"")-8;i++)

        {     
          Serial.println("testing");
          rtext+=test[test.indexOf("+08\"")+i+6];    
        }
       }
       */}
       test="";

        




  
  
    Serial.print("numvber=");
    Serial.println(number);
     Serial.print("rtext =");
    Serial.println(rtext);
    
  }
  
   /* void compare(){
    
    for(int i=0;i<cn;i++)
    
    
    if(rtext.indexOf(Commands[i])>0)
    {
        rflag=i;
        
              Serial.println("found cm");
              
    
    }
    rtext="";
    number="";
    }
*/
 /*   
  void sendSMS()
  {
    
  for(int i=0;i<phoneQouta;i++)
  {
   
    delay(100);
    SIM900.println("AT+CMGS=\"" + phone[i] + "\"");
    delay(500);
    Serial.println("AT+CMGS=\"" + phone[i] + "\"");
    SIM900.print(stext[sflag]);
    delay(500);

    SIM900.print((char)26);
    SIM900.print("\n");
    
    delay(15000);
  }
  }
  */
  void sendSMS()
{
  Serial.println("SMS send started");
  for(int i=0;i<phoneQouta;i++)
  {
    delay(300);
      SIM900.println("AT+CMGF=1"); 
    delay(300);
      SIM900.println("AT+CSCS=\"GSM\""); 
      delay(100);
       String val="";
  while (SIM900.available()) { 
     int ch = SIM900.read();
      val += char(ch);
      delay(100);
    }
    Serial.println(val);
  SIM900.println("AT+CMGS=\"" + phone[i] + "\"");
  delay(500);
  SIM900.print(stext[sflag]);
  delay(500);
  SIM900.print((char)26);
  delay(500);
  Serial.println("SMS send complete");
 val="";
  while (SIM900.available()) { 
     int ch = SIM900.read();
      val += char(ch);
      delay(100);
    }
    Serial.println(val);
  delay(1000);
  }
}

  void Pill(){
 if ( myRadio.available()) // Check for incoming data from transmitter
  {
    
 
  for(int i=0;i<5;i++)
  {
      myRadio.read( &pills, sizeof(pills) ); 
      if (pills==1){break;}
      else pills=0;
    
    }
 Serial.print("Data received = ");
    Serial.println(pills);
    
  }
  }
       
  
  String val="";
    int ch=0;




   void receiveEvent(int howMany) 
  {
    while ( Wire.available())
    {
      Serial.println("Start debug");
      sflag=Wire.read();
      Serial.println(sflag);
      Switch1=Wire.read();
            Serial.println(Switch1);
      Switch2=Wire.read();
            Serial.println(Switch2);
      sendsms=Wire.read();
            Serial.println(sendsms);
    
      Serial.println("end debug");
    }
  }
  
  
  
  
  
  
  void SYNC()
  {
    
    dataSent[0]=water_level ;
    dataSent[1]=temprature;
    dataSent[2]=humidity;
    dataSent[3]=detected;
    dataSent[4]=current1*1000;
    dataSent[5]=current2*1000;
    dataSent[6]=rflag;
    dataSent[7]=pills;
    
    
  }



void requestEvent() 
{
  
    Wire.write(dataSent , 8);
    rflag=-1;
  }

  
   
   
  void setup() 
  {
   
      Wire.begin(9);                // join i2c bus with address #8
      Wire.onRequest(requestEvent); // register event// register event
      Wire.onReceive(receiveEvent);
      Serial.begin(9600); 
      SIM900.begin(115200);
      myRadio.begin();
      
      myRadio.setChannel(108);
      myRadio.setPALevel(RF24_PA_MIN);
      myRadio.openReadingPipe(1, addresses[0]); 
      myRadio.startListening();
  
         
      SIM900.print("AT+IPR=9600\n");

      delay(100);
      Serial.begin(9600);
     
      delay(100);
      SIM900.begin(9600);
      SIM900.println("AT+CSCS=\"GSM\""); 
      delay(300);
      SIM900.println("AT+CMGF=1"); 
    delay(300);
      
   
      delay(100);
      SIM900.print("AT+CNMI=2,2,0,0,0\n");
      SYNC();     
      pinMode(switch1,OUTPUT);
      pinMode(switch2,OUTPUT);
      //sendsms=1;
      //delay(5000);
  

  }

   
  void loop() 
  {
    digitalWrite(switch1,on1);
digitalWrite(switch2,on2);
    if(rflag!=-1) sendsms=1;
  //if(Serial.available())
//  sendsms=1;
   Water_level();
   temp();
   hum();
   Detection();
  //electrical();
   State();
   SYNC();
   func();

   if (sendsms==1)
   {
    delay(2000);
    sendSMS();
    rflag=-1;
    sendsms=0;
    //delay(1000);
   }

   
/*      
    while (SIM900.available())
    {
     
      ch = SIM900.read();
      val += char(ch);
      delay(10);
    }
    Serial.println(val);
    }
 */
   recieveSMS();
//   compare();
   Pill();
Serial.println("\n\n");
Serial.println(water_level);
Serial.println(temprature);
Serial.println(humidity);

Serial.println(detected);
Serial.println(current1);
Serial.println(current2);
Serial.println(rflag);
Serial.println(pills);

switchLamp();
  }

  
  
  
  




  
  
