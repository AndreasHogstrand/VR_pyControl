//Edit this threshold to change the sensibility of the lickometer
int lickThreshold = 80;

//Synchronization variables
bool isLicking = false;
int lickoVal = 0;
int rewardConsumed = 0;

void setup() {
  Serial.begin(57600);
  pinMode(A4, INPUT);
}

void loop() {
  //Flush the serial console (to avoid buffering)
  Serial.flush();

  //This code implements the FSM of the attached figure
  if(isLicking){
    lickoVal = analogRead(A4);
    if(lickoVal <= lickThreshold) isLicking = false;
  } else {
    lickoVal = analogRead(A4);
    if(lickoVal > lickThreshold){
      isLicking = true;
      Serial.print('l');
    } else isLicking = false;
  }

}
