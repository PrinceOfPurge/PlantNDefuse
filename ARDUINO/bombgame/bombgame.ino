#define READ_PIN 11

void setup() {
  Serial.begin(9600);
  Serial.println("TEST");

  pinMode(13, OUTPUT);

  pinMode(READ_PIN, INPUT);
  digitalWrite(READ_PIN, HIGH);
}

void loop() {
  int v = digitalRead(READ_PIN);
  digitalWrite(13, v);

  if (v == LOW)
  {
    Serial.println("Connected");
  }
  else{
    Serial.println("Not connected");
  }
}
