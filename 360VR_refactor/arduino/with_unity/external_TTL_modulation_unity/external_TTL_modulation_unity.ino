/**** This script should be used together with Unity ****/

// Laser pulses configuration parameters
const int numberOfPulses = 10;
const int pulseOnDuration = 20;
const int pulseOffDuration = 80;
const int LED_duration = 30; //duration in milliseconds of the led blink 

//GPIO connection from/to Arduino
const int laser = 4;
const int miniscope = 6;
const int external_LED = 8;


// Function declarations
void TTLPulse(const int, int);
void TTLBurst(const int, int, int, int);
void TTLTrain(const int, int, int, int, int, int);

void setup() {
  // initialization of the serial console
  Serial.begin(57600);

  // init of the external pheripherals
  pinMode(laser,OUTPUT);
  pinMode(miniscope,OUTPUT);
  pinMode(external_LED,OUTPUT);

  // reset the state of the laser pin
  digitalWrite(laser,LOW);  
  digitalWrite(miniscope,LOW); 
  digitalWrite(external_LED,LOW);
}

void loop() {

  char stateChar = (char)Serial.read();
  if (stateChar == 'L')
  {
    TTLBurst(laser, numberOfPulses, pulseOnDuration, pulseOffDuration);
  }
  else if (stateChar == 'S')
  {
    digitalWrite(miniscope,HIGH);

  }
  else if (stateChar == 'T')
  {
    digitalWrite(miniscope,LOW);
  }
  else if (stateChar == 'B')
  {
    digitalWrite(external_LED,HIGH);
    delay(LED_duration); //only trial at start NOTE: during this time, the arduino will not receive any other signal from unity
    digitalWrite(external_LED,LOW);
  }
}

// Implementation of the TTLTrain function 
void TTLTrain(const int gpio, int numberOfBursts, int numberOfPulses, int pulseWidth, int interPulseDelay, int interBurstDelay){
  for(int i=0; i<numberOfBursts; i++){
    TTLBurst(gpio, numberOfPulses, pulseWidth, interPulseDelay);
    delay(interBurstDelay);
  }
  
}

// Implementation of the TTLBurst function 
void TTLBurst(const int gpio, int numberOfPulses, int pulseWidth, int interPulseDelay){
  for(int i=0; i<numberOfPulses; i++){
    TTLPulse(gpio, pulseWidth);
    delay(interPulseDelay);
  }
}

// Implementation of the TTLPulse function 
void TTLPulse(const int gpio, int pulseWidth){
  Serial.println("Sending a TTL pulse...");
  digitalWrite(gpio,HIGH);
  delay(pulseWidth);
  digitalWrite(gpio,LOW);
}
