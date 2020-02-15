int8_t cnt = 0;
bool mode = false;

void setup() {
  Serial.begin(9600);
}

void loop() {

  char ch = Serial.read();
  Serial.print(ch);
  delay(200);
}
