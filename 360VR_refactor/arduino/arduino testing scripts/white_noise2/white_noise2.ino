#include "elapsedMillis.h"
#include "pitches.h"

elapsedMillis timeElapsed;
const int NOISE_DURATION = 1000; //Time in milliseconds

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  
}

void loop() {
/*
tone(3,5000,150);
delay(200);
tone(3,5000,150);
delay(200);
tone(3,5000,150);
delay(200);
tone(3,5000,150);
delay(200);
tone(3,5000,150);
delay(200);
*/



tone(3,1500,1000);
delay(1200);

tone(3,3000,1000);
delay(1200);

tone(3,4500,1000);
delay(1200);

tone(3,7000,1000);
delay(1200);

tone(3,9500,1000);
delay(1200);

  /*
delay(1000);

  tone(3, 6000, 125);
  delay(125);
  
  tone(3, 4000, 125);
  delay(125);
  */
  
  
  
    /*
  timeElapsed = 0;
  while(timeElapsed < NOISE_DURATION){
    int freq = random(10000, 20000);
    int noteDuration = .1;
    tone(3, freq, noteDuration);
    delay(noteDuration);
    //delayMicroseconds(50);

    
  }
*/
  noTone(3);
  delay(3000);
  
}
