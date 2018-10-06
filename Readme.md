Build a complete automated home using electronic sensors, keypads, power relays, screens, camera, gsm module, raspberry pi3 and 2 arduinos uno...
The project consist of 1 raspberry pi working as a master controlling 3 arduino controllers working as slaves, a gsm module , a camera and a 7" touch screen...

The raspberry pi:

forces the camera to capture the person's faces in front of the door and send the pictures to a microsoft server which authenticate the user ( this service is a project from oxford invested by microsoft )

controls the first slave controller that is responsible for all security missions as : opening door locks, controling keypads to authenticate user...

controls the second slave which is responsible for all other facilities such as tempreture sensing, tank water level detecting, electric current switching for lamps and all electric devices ...

controls the third slave arduino ( nano ) with wirless comunication using nordic radio frequency chips( nrf24L01 ) connected to the 2 ends, this slave is inserted in a pill pack and connected to a small circuit that keeps track of the opening and closing of the pack so it report usage of the medicine helping the old people remember the time of medicine take...

Controls the gsm module ( neoway M590E ) that has a sim card inserted to report data and keep the user updated, and even to give the user some control facilities using sms...

Uses the touch screen to give the user full control and monitoring of all this project facilities through a well designed easy to use interface, the interface have a screen lock that displays uptodated results of tempteture, power comsumption, humidity and other things. It also contains several applications that permit the user to control several devices in each room , and to change setting to turn off and on some preferences...

Used programing languages:
* c ( for arduino )
* c# ( for raspberry pi that runs windows iot platform )
* AT commands ( for gsm module )
