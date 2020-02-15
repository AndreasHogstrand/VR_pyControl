/**
 * This script should be uploaded to the Arduino board that handles the pumps, the speaker and the 7-seg display
 * and
 */

#include <SPI.h>
#include "DDM4.h"

//Control variables
int dropDuration = 500;
int punishmentDuration = 500;

//7-segment display handler
DDM4 ddm4;

//GPIO connection from/to Arduino
const int speaker = 3;
//const int ON = 6;
const int reward = 7;
const int punishment = 6; 
const int axona = 9; 
const int buzzer = 12;

//Synchronization variables
int rewardConsumed = 0;
bool stopWhiteNoise = false;
bool playWhiteNoise = false;

// Currently not in use
/*int time_on = 6000000; 
unsigned long current_time = 0;
bool first_time = true;
bool go_timer = false;
*///////

void setup() {
  // initialization of the serial console
  Serial.begin(57600);

  // init of the external pheripherals
  pinMode(5,OUTPUT);
  pinMode(speaker,OUTPUT);
  pinMode(6,OUTPUT);
  pinMode(reward,OUTPUT);
  pinMode(punishment,OUTPUT);
  pinMode(9,OUTPUT);
  pinMode(12,OUTPUT);
  ddm4.initDDM4();
  ddm4.writeDDM4(rewardConsumed,0);

  //To avoid high impedance of the speaker just play/stop a tone
  tone(speaker, 4000, 1);
  delay(125);
  noTone(speaker);
  
}

void loop() {
  // read the state char sent from the Unity project
  char stateChar = (char)Serial.read();

  //Try to remove this snippet
  /*
  if (go_timer == true)
  {
    current_time += 1;
    if (current_time - time_on > 0)
    {
      analogWrite(ON,0);
      go_timer = false;
      first_time = true;
      current_time = 0;
    }
  }
  */
  /////

  // upon the state character go into a state and do relative actions
  if (stateChar == 's')
  {
    digitalWrite(axona,HIGH);
    Serial.print('h');
    digitalWrite(axona,LOW);
  }
  
  if (stateChar == 'a') //The REWARD state
  {
    // open the pump, leave it opened for 250ms, then close the pump
    analogWrite(reward,dropDuration);
    Serial.print('p'); //try to remove it
    delay(dropDuration);
    analogWrite(reward,0);
  } 

  if (stateChar == 'p') //The PUNISHMENT state
  {
    // open the pump, leave it opened for 250ms, then close the pump
    analogWrite(punishment,punishmentDuration);
    delay(punishmentDuration);
    analogWrite(punishment,0);
  } 
  
  if (stateChar == 'b') // The BUZZER state
  {
    /*
    if (first_time == true)
    {
      go_timer = true;
      first_time = false;
      analogWrite(ON,250);
    }
    */
    analogWrite(buzzer,10);
    delay(500);
    analogWrite(buzzer,0);
  }

  if(stateChar == 'c') //The CONSUMED state
  {
    //If here, then Unity project detected that a reward has been consumed, so increment the counter displayed on the 7-seg
    ddm4.writeDDM4(++rewardConsumed,0);
  }

  if(stateChar == 'S') //The SUCCESS state
  {
    //If here, then Unity project requested for a `Success' sound
    noTone(speaker);
    tone(speaker, 6000, 125);
    delay(125);
    
    tone(speaker, 4000, 125);
    delay(125);
    noTone(speaker);
  }

  if(stateChar == 'M') //The MISSED state
  {
    //If here, the Unity project requested for a `FAILURE' sound
    noTone(speaker);
    tone(speaker, 9000, 100);
    delay(100);
    
    tone(speaker, 10000, 100);
    delay(100);
    
    tone(speaker, 11000, 50);
    delay(50);
    noTone(speaker);
  }

  if(stateChar == 'w') //The BEGIN WHITE NOISE state
  {
    //If here, then Unity project request to start the execution of the white noise
    playWhiteNoise = true;
    stopWhiteNoise = false;
  }

  if(stateChar == 'W') //The END WHITE NOISE state
  {
    //If here, then Unity project request to stop the execution of the white noise
    playWhiteNoise = false;
    stopWhiteNoise = true;
    noTone(3);
  }

  if(stateChar == '1') //The 1.5KHz state
  {
    //If here, then Unity project requested for a `1.5KHz' sound
    noTone(speaker);
    tone(speaker, 1500, 1000);
    delay(1000);
    
  }

  if(stateChar == '3') //The 3KHz state
  {
    //If here, then Unity project requested for a `3KHz' sound
    noTone(speaker);
    tone(speaker, 3000, 1000);
    delay(1000);
    
  }

  if(stateChar == '4') //The 4.5KHz state
  {
    //If here, then Unity project requested for a `4.5KHz' sound
    noTone(speaker);
    tone(speaker, 4500, 1000);
    delay(1000);
    
  }

  if(stateChar == '7') //The 7KHz state
  {
    //If here, then Unity project requested for a `7KHz' sound
    noTone(speaker);
    tone(speaker, 7000, 1000);
    delay(1000);
    
  }

  if(stateChar == '9') //The 9.5KHz state
  {
    //If here, then Unity project requested for a `9.5KHz' sound
    noTone(speaker);
    tone(speaker, 9500, 1000);
    delay(1000);
    
  }
  
  if(playWhiteNoise && !stopWhiteNoise) //This branch decides if the speaker should execute a random frequency (for the white noise) or not
  {
    int freq = random(10000, 20000);
    tone(speaker, freq, .1);
    delay(.1);
  }
}
