#define RED_WIRE A0
#define GREEN_WIRE A1
#define BLUE_WIRE A2

#define SLOT_0_VALUE 40
#define SLOT_1_VALUE 225
#define SLOT_2_VALUE 20

#define ROUNDFACTOR 5

void setup() {
  Serial.begin(9600);
  Serial.println("TEST");

  pinMode(13, OUTPUT);

  pinMode(RED_WIRE, INPUT);
  pinMode(GREEN_WIRE, INPUT);
  pinMode(BLUE_WIRE, INPUT);

  digitalWrite(RED_WIRE, HIGH);
  digitalWrite(GREEN_WIRE, HIGH);
  digitalWrite(BLUE_WIRE, HIGH);
}

int roundVal(int value) {
  int remainder = value % ROUNDFACTOR;

  if (remainder < 3)
    value -= remainder;
  else
    value += ROUNDFACTOR - remainder;

  return value;
}

int getSlotNumber(int voltage) {
  switch (voltage) {
    case SLOT_0_VALUE:
      return 0;

    case SLOT_1_VALUE:
      return 1;

    case SLOT_2_VALUE:
      return 2;
  }

  return -1;
}

void loop() {
  int red_wire = analogRead(RED_WIRE);
  int green_wire = analogRead(GREEN_WIRE);
  int blue_wire = analogRead(BLUE_WIRE);

  red_wire = roundVal(red_wire);
  green_wire = roundVal(green_wire);
  blue_wire = roundVal(blue_wire);

  red_wire = getSlotNumber(red_wire);
  green_wire = getSlotNumber(green_wire);
  blue_wire = getSlotNumber(blue_wire);

  // Serial.write((uint8_t)233);
  // Serial.write((uint8_t)red_wire);
  // Serial.write((uint8_t)green_wire);
  // Serial.write((uint8_t)blue_wire);

  Serial.println("");
  Serial.println(red_wire);
  Serial.println(green_wire);
  Serial.println(blue_wire);

  delay(1000);
}
