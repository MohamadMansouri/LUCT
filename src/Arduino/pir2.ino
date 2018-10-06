/*     Arduini PIR Motion Sensor Tutorial
 *      
 *  by Dejan Nedelkovski, www.HowToMechatronics.com
 *    
 */
int pirSensor = 3;
int relayInput = 13;
void setup() {
  pinMode(pirSensor, INPUT);
  pinMode(relayInput, OUTPUT);  
}
void loop() {
  int sensorValue = digitalRead(pirSensor);
  if (sensorValue == 1) {
    digitalWrite(relayInput, HIGH); // The Relay Input works Inversly
  }
}
