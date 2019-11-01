#include "FastLED.h"
#define LedAmount 60
#define UsedDataPin 6
#define SerialRate 115200
CRGB LedArray[LedAmount];

void setup()
{
  //Add all leds to FastLed
  FastLED.addLeds<NEOPIXEL, UsedDataPin>(LedArray, LedAmount);

  //Begin serial connection
  Serial.begin(SerialRate);

  //Startup RGB color test
  LEDS.showColor(CRGB(255, 0, 0));
  delay(750);
  LEDS.showColor(CRGB(0, 255, 0));
  delay(750);
  LEDS.showColor(CRGB(0, 0, 255));
  delay(750);
  LEDS.showColor(CRGB(0, 0, 0));
}

void loop()
{
  //Read incoming data and check begin
  while (!Serial.available());
  if ('A' != Serial.read()) return;
  while (!Serial.available());
  if ('d' != Serial.read()) return;
  while (!Serial.available());
  if ('a' != Serial.read()) return;

  //Read incoming data and updated leds
  for (uint8_t LedId = 0; LedId < LedAmount; LedId++)
  {
    while (!Serial.available());
    LedArray[LedId].r = Serial.read();
    while (!Serial.available());
    LedArray[LedId].g = Serial.read();
    while (!Serial.available());
    LedArray[LedId].b = Serial.read();
  }
  FastLED.show();
}
