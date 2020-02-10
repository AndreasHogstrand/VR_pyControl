char data;
boolean newData = false;

void setup() {
  char data = '\0';
  Serial.begin(9600);
  Serial1.begin(9600);
  Serial.write("Ready!");
}

void loop() {
  if (!newData) {
    if (Serial1.available() > 0) {
      data = Serial1.read();
      newData = true;
    }
  }
  else {
    Serial.write(data);
    newData = false;
  }
}
