
/*        Pin | arduinoPin
 *   LCD :
 *        SCL -> A5
 *        SDA -> A4
 * KeyPad:       
 *         1 ->2
 *         2 ->3
 *         3 ->4
 *         4 ->5
 *         5 ->11
 *         6 ->8
 *         7 ->7
 *         8 ->6
 * 
 * RFID:   SDA->10       
 *         SCK->13
 *         MOSI->11
 *         MISO->12
 *         RST->9
 *         
 * LEDS:   RED -> A0
 *         Blue -> A1
 *         Green -> A2
 */       
  
  
  
 /* The circuit:
 * LCD RS pin to digital pin A3
 * LCD Enable pin to digital A2
 * LCD D4 pin to digital pin A1
 * LCD D5 pin to digital pin A0
 * LCD D6 pin to digital pin 1
 * LCD D7 pin to digital pin 0
 * LCD R/W pin to ground
 * 10K resistor:
 * ends to +5V and ground
 * wiper to LCD VO pin (pin 3)
 */
  
  
  
  
  
  #include <SPI.h>
  #include <MFRC522.h>
  #include <Keypad.h>
  #include <Chrono.h>
  #include <Wire.h> 
  #include <LiquidCrystal.h>
  Chrono time1(Chrono::SECONDS);
  Chrono time2(Chrono::SECONDS);
 

  
  #define SS_PIN 10
  #define RST_PIN 9
  #define D5 A0
  #define D4 A1
  #define E A2
  #define RS A2
  MFRC522 mfrc522(SS_PIN, RST_PIN);











  
/* GLOBAL VARIABLES */

bool passwordCheck=0;
bool ban=0;
int banTime=20;
int banQouta=3;
char Master[11] = "123456"; 
int Password_Lenght=7 ;// Give enough room for six chars + NULL char

bool RFIDcheck=0;
int countID=1;
String cardID[20] ={"0A DC 1F 2B"};
int dataSent[3];
//"86 0D B0 2D"


/* END OF GLOBAL VARIABLES */






  
 
  char Data[11]; // 6 is the number of chars it can hold + the null char = 7

  byte data_count = 0, master_count = 0;
  bool Pass_is_good;
  char customKey;
  int badTries=0;
  
  
  const byte ROWS = 4;
  const byte COLS = 3;
  char keys[ROWS][COLS] = {
    {'1','2','3'},
    {'4','5','6'},
    {'7','8','9'},
    {'*','0','#'}
  };
  
  byte rowPins[ROWS] = {
    2,3,4,5}; //connect to the row pinouts of the keypad
  byte colPins[COLS] = {
    8,7,6}; //connect to the column pinouts of the keypad
  
  Keypad customKeypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS); //initialize an instance of class NewKeypad 



  void clearData()
  {
    while(data_count !=0)
    {   // This can be used for any array size, 
      Data[data_count--] = 0; //clear array for new data
    }
    return;
  }



bool checkPass(int *j=0)
{ 

  bool check;
  
 while (1)
  { 
    if (time2.elapsed()>=10)
   {  

    check=0; 
    clearData();
    break;

   }
  lcd.setCursor(0,0);
  lcd.print("Enter Password");  
    customKey = customKeypad.getKey();
    if (customKey) // makes sure a key is actually pressed, equal to (customKey != NO_KEY)
    {
      Data[data_count] = customKey; // store char into data array
      lcd.setCursor(data_count,1); // move cursor to show each new char
      lcd.print(Data[data_count]);
      delay(300);// print char at said cursor
      lcd.setCursor(data_count,1);
      lcd.print("*");
      data_count++; // increment data array by 1 to store new char, also keep track of the number of chars entered
    }
  
    if(data_count == Password_Lenght-1) // if the array index is equal to the number of expected chars, compare data to master
    {
      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print("Password is ");
  
      if(!strcmp(Data, Master)) // equal to (strcmp(Data, Master) == 0)
        {
        
        lcd.print("Good");
        //digitalWrite(BLUE,LOW);
      //  BLINK(GREEN);
        
        check=true;
        *j=0;
        delay(2000);
   // digitalWrite(GREEN,LOW);
  // digitalWrite(BLUE,HIGH);
        }
        
   
      else
      {
        
        lcd.print("Bad");
       // digitalWrite(BLUE,LOW);
       // BLINK(RED);
       // digitalWrite(RED,LOW);
        
        check=false;
        (*j)++;
     time1.restart();
      }
  
      lcd.clear();
      clearData(); 
      //lcd.print("Enter Password");  
      break;
      
    }
    }
    return check;
}


void Ban()
{
  if ( banQouta == badTries)
  {
      if (time1.elapsed()<=banTime)
      {
        ban=1;
        }
      else
      {
        ban=0;
        badTries=0;
        }
    }
}


void checkRFID()
{
  bool found=0;
  String content= "";
  byte letter;
  for (byte i = 0; i < mfrc522.uid.size; i++) 
  {
     Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
     Serial.print(mfrc522.uid.uidByte[i], HEX);
     content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
     content.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  Serial.println();
  Serial.print("Message : ");
  content.toUpperCase();
  for(int j=0;j<countID;j++)
  {
  if (content.substring(1) == cardID[j]) 
  {
    found=1;
    RFIDcheck=1;
    badTries=0;
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Authorized Access");
    
//    digitalWrite(RED,LOW);
  //  digitalWrite(BLUE,LOW);
    //BLINK(GREEN);
   
    delay(1000); 
//    digitalWrite(GREEN,LOW);
  //    digitalWrite(BLUE,HIGH);

    break;
  }
}
  if(!found) 
  {
    RFIDcheck=0;
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Access Denied");
    
    digitalWrite(GREEN,LOW);
    digitalWrite(BLUE,LOW);
   // BLINK(RED);
   
    delay(1000); 
    //digitalWrite(RED,LOW);
    //digitalWrite(BLUE,HIGH);
  }
  
}

/*void BLINK(int pin)
{
  for(int i=0; i<3 ; i++)
  {
    digitalWrite(pin,LOW);
    delay(150);
    digitalWrite(pin,HIGH);
    delay(150);
  }
}
*/


 /*void openDoor()
  {
   digitalWrite(A3,HIGH);
   delay(2000);
   digitalWrite(A3,LOW);
   
  }

*/


void receiveEvent(int howMany) 
{
  while ( Wire.available())
  { 
    banQouta=Wire.read();
    banTime=Wire.read();
    Password_Lenght=Wire.read();
    countID=Wire.read();
    for(int i=0;i<Password_Length;i++)
    {
      Master[i] =Wire.read();
    } 
    for(int i=0;i<countID;i++)
    {
      char a[10];
      for(int j=0;j<10;j++)
      {
        a[j]=Wire.read();
        
      }
      cardID[i] =a;
    } 

  }
}



void requestEvent() 
{
  for(int i=0;i<3;i++)
  {
    Wire.write(dataSent[i]);
  }
}





void SYNC_feedback()
{
  dataSent[0]=ban ;
  dataSent[1]=passwordCheck;
  dataSent[2]=RFIDcheck;
}

  
  
  void setup()
  {
   lcd.begin(16, 2);
   
    SPI.begin();
    Serial.begin(9600);
    mfrc522.PCD_Init();
    lcd.setCursor(0,0);
    lcd.print("* : RFID");
    lcd.setCursor(0,1);
    lcd.print("# : KeyPad");
  //  pinMode(RED,OUTPUT);
   // pinMode(BLUE,OUTPUT);
    //pinMode(GREEN,OUTPUT);
   // digitalWrite(RED,LOW);
   // digitalWrite(BLUE,HIGH);
    //digitalWrite(GREEN,LOW);
     SYNC();     
    Wire.begin(8);                // join i2c bus with address #8
    Wire.onReceive(receiveEvent); 
    Wire.onRequest(requestEvent); // register event// register event
    Serial.begin(9600); 
         
   
    
  }




  




 
  void loop()
{


  
customKey = customKeypad.getKey();
    

if(customKey=='#')
{
  while(1)
  {
  time2.restart();
  
  if(ban==0)
    {
 //   digitalWrite(RED,LOW);
   // digitalWrite(BLUE,HIGH);
    //digitalWrite(GREEN,LOW);
   
    lcd.clear();
    passwordCheck=checkPass(&badTries);
    Serial.println("password");
    Serial.println( passwordCheck);
       if (time2.elapsed()>=10 || passwordCheck) 
       {

    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("* : RFID");
    lcd.setCursor(0,1);
    lcd.print("# : KeyPad");
        break;
       }
    }
  else
  {
    //digitalWrite(RED,HIGH);
    //digitalWrite(BLUE,LOW);
    //digitalWrite(GREEN,LOW);
   
    lcd.setCursor(0,0);
    lcd.print("ACCESS DENIED");
    lcd.setCursor(0,1);
    lcd.print("for 20 sec");
    
  }
    
    Ban();

    
    Serial.print("badTries=");
    Serial.println(badTries);
    Serial.print("ban=");
    Serial.println(ban);

    Serial.println(time2.elapsed());
    

  }
    
}
if(customKey=='*')
  {
     //BLINK(BLUE);
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Scan Card");
    time2.restart();
     while ( ! mfrc522.PICC_IsNewCardPresent()) { if (time2.elapsed()>=10) goto label;}
     while ( ! mfrc522.PICC_ReadCardSerial()) {if (time2.elapsed()>=10) goto label;}


  Serial.print("UID tag :");
  
  checkRFID();
    label:                      
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("* : RFID");
    lcd.setCursor(0,1);
    lcd.print("# : KeyPad");
  }
  if ( passwordCheck || RFIDcheck)
  {
   // openDoor();
  }


  
Serial.println(ban);

SYNC();



}



