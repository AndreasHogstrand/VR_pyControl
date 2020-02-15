#include <elapsedMillis.h>

const int REWARD_AVAILABLE_MILLIS = 1500;
void setup() {
  Serial.begin(9600);
}

void loop() {
  elapsedMillis timeout = 0;

  while (timeout < REWARD_AVAILABLE_MILLIS) {
    
    char ch = Serial.read();
    if(ch == 'a'){
      timeout = 0;
      Serial.print('o');
    }
    
  }
  Serial.print('x');

}
