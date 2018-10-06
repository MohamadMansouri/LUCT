# LUCT

The aim of the project is to build a complete automated home using electronic sensors, keypads, power relays, screens, camera, gsm module, raspberry pi3 and 2 arduinos uno...
The project consist of 1 raspberry pi working as a master controlling 3 arduino controllers working as slaves, a gsm module , a camera and a 7" touch screen...

**The raspberry** pi orchestrates the work of the other devices using the i2c protocol
**The camera** is fixed above the front door to capture human faces and send the pictures to a microsoft server which authenticate the user (this service is a project from oxford invested by microsoft).
**Arduino 1** is the first slave which is responsible for all security missions as : opening door locks, controling keypads to authenticate user.
**Arduino 2** is the second which is responsible for all other functionalities such as tempreture sensing, tank's water level reporting, electric current switching for lamps and all electric devices.
**Arduino nano** is the third slave, it uses wirless comunication using nordic radio frequency chips( nrf24L01 ) connected to the 2 ends, this slave is inserted in a pill pack and connected to a small circuit that keeps track of the openin and closing of the pack. This is used for helping the old people to remember the times to take their pills.
**The Gsm module ( neoway M590E )** is the device that supports the communication of the smart home with the outer world, it uses a sim card to report informations about the house and keep the user updated by his phone, moreover it gives the user the abbility to control the house through sending sms...
**The touch screen**  gives the user full control and monitoring of all this project functionalities through a well designed easy to use interface, the interface have a screen lock that displays feedbacks of the tempteture, power comsumption, humidity and many more. It also contains several applications for the user to control several devices in each room , and to configure the settings and preferences.

# Used programing languages:
* c ( for arduino )
* c# ( for raspberry pi that runs windows iot platform )
* AT commands ( for gsm module )
