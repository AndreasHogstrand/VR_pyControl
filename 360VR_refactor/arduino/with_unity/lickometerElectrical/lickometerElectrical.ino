//Synchronization variables
bool isLicking = false;
int lickoVal = 0;
int rewardConsumed = 0;

void setup() {
  Serial.begin(57600);
  pinMode(4, INPUT);
}

void loop() {
  //Flush the serial console (to avoid buffering)
  Serial.flush();

  //This code implements the FSM of the attached figure
  if(isLicking){
    lickoVal = digitalRead(4);
    if(lickoVal == LOW) isLicking = false;
  } else {
    lickoVal = digitalRead(4);
    if(lickoVal == HIGH){
      isLicking = true;
      Serial.print('l');
      delay(70);
    } else isLicking = false;
  }
  /*
  lickoVal = digitalRead(A4);
  Serial.println(lickoVal);
*/
}
