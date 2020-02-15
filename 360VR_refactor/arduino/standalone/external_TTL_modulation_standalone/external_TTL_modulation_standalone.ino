/**** This script does NOT communicate with Unity ****/
/**** This script should be uploaded to the Arduino that has to handle 
      TTL signals to imaging and laser (and maybe axona in the future) ****/

// Laser pulses configuration parameters in miliseconds
const int totalNumberOfBursts = 3;
const int numberOfPulses = 10;
const int pulseOnDuration = 10;
const int pulseOffDuration = 80;
const int interBurstDelay = 1000;
const int startupDelay =1000;

//GPIO connection from/to Arduino
const int laser = 4;
const int miniscope = 6;
const int external_LED = 8;

// Function declarations
void TTLPulse(const int, int);
void TTLBurst(const int, int, int, int);
void TTLTrain(const int, int, int, int, int, int);

// The main function that will be executed by Arduino
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

  // wait a bit
  delay(startupDelay);

  // start the imaging
  digitalWrite(miniscope, HIGH);

  // start the train of pulses
  TTLTrain(laser, totalNumberOfBursts, numberOfPulses, pulseOnDuration, pulseOffDuration, interBurstDelay);

  // stop the imaging
  digitalWrite(miniscope, LOW);
}

// The loop function is empty because Arduino should execute the TTL Train only once (in the setup() function)
void loop() {}

// Implementation of the TTLTrain function 
void TTLTrain(const int gpio, int numberOfBursts, int numberOfPulses, int pulseWidth, int interPulseDelay, int interBurstDelay){
  for(int i=0; i<numberOfBursts; i++){
    TTLBurst(gpio, numberOfPulses, pulseWidth, interPulseDelay);
    delay(interBurstDelay);
  }
  
}

// Implementation of the TTLBurst function 
void TTLBurst(const int gpio, int numberOfPulses, int pulseWidth, int interPulseDelay){
  digitalWrite(external_LED,HIGH);
  for(int i=0; i<numberOfPulses; i++){
    TTLPulse(gpio, pulseWidth);
    delay(interPulseDelay);
  }
  digitalWrite(external_LED,LOW);
}

// Implementation of the TTLPulse function 
void TTLPulse(const int gpio, int pulseWidth){
  Serial.println("Sending a TTL pulse...");
  digitalWrite(gpio,HIGH);
  delay(pulseWidth);
  digitalWrite(gpio,LOW);
}
