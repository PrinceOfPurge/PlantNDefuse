#define RED_WIRE A0
#define GREEN_WIRE A1
#define BLUE_WIRE A2

#define SLOT_0_MIN 36
#define SLOT_0_MAX 210
#define SLOT_1_MIN 211
#define SLOT_1_MAX 500
#define SLOT_2_MIN 5
#define SLOT_2_MAX 35

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

int getSlotNumber(int resistance) {
  if (resistance >= SLOT_0_MIN && resistance <= SLOT_0_MAX)
  {
    return 0;
  }
  else if (resistance >= SLOT_1_MIN && resistance <= SLOT_1_MAX)
  {
    return 1;
  }
  else if (resistance >= SLOT_2_MIN && resistance <= SLOT_2_MAX)
  {
    return 2;
  }

  return -1;
}

void loop() {
  int red_wire = analogRead(RED_WIRE);
  int green_wire = analogRead(GREEN_WIRE);
  int blue_wire = analogRead(BLUE_WIRE);

  red_wire = getSlotNumber(red_wire);
  green_wire = getSlotNumber(green_wire);
  blue_wire = getSlotNumber(blue_wire);

  if (true)
  {
    Serial.write((uint8_t)233);
    Serial.write((uint8_t)red_wire);
    Serial.write((uint8_t)green_wire);
    Serial.write((uint8_t)blue_wire);
  }
  else
  {
    Serial.println("");
    Serial.println(red_wire);
    Serial.println(green_wire);
    Serial.println(blue_wire);
  }

  delay(100);
}
