/**
 * This script should be uploaded to the Arduino board that handles the pump, the speaker and the 7-seg display
 * and
 */
#include <SPI.h>
#include "DDM4.h"

DDM4 ddm4;
const int reward = 7;
const int punishment = 6;
const int speaker = 3;
int drops = 0;

void successTone();

//////////////// CONFIGURE HERE ////////////////

bool pumpFlush = false; //set it to TRUE if you need to flush the pump
bool stopPump = true; //if this variable is TRUE, then the pump is deactivated
int totalNumOfDrops = 20; //number of activation of the pump. After this value, the pump will be disabled.
int interDropDelay  = 1000; //the delay in milliseconds between a pump drop and another
int dropDuration    = 50; //the time in millliseconds in which the pump is dropping water
bool playTone = false;

///////////////////////////////////////////////

void setup() {
  // put your setup code here, to run once:
  Serial.begin(57600);
  pinMode(reward,OUTPUT); //init the pump
  ddm4.initDDM4(); //init the 7-seg display
  ddm4.writeDDM4(drops,0);
  
  if(pumpFlush){
    interDropDelay = 0;
    dropDuration = 100000; 
  }

  if(playTone){
    pinMode(speaker,OUTPUT); //init the speaker, if requested
  }
}

void loop() {
  
  if(stopPump) return; //deactivate the pump if stopPump is true

  if(drops++ < totalNumOfDrops){
    ddm4.writeDDM4(drops,0);
    analogWrite(reward,250);
    analogWrite(punishment,250);
    delay(dropDuration);
    analogWrite(reward,0);
    analogWrite(punishment,0);
    if(playTone) successTone();
    delay(interDropDelay);
    
  }
    
}

void successTone()
{
    noTone(speaker);
    tone(speaker, 6000, 125);
    delay(125);
    
    tone(speaker, 4000, 125);
    delay(125);
    noTone(speaker);
  }
