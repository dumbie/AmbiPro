#include "FastLED.h"
#define LedAmount 60
#define UsedDataPin 7
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
  if (Serial.available())
  {
    //Read incoming data and check header
    if (Serial.read() != 'A') { return; }
    if (Serial.read() != 'd') { return; }
    if (Serial.read() != 'a') { return; }

    //Read incoming data and set colors
    for (uint16_t LedId = 0; LedId < LedAmount; LedId++)
    {
      LedArray[LedId].r = Serial.read();
      LedArray[LedId].g = Serial.read();
      LedArray[LedId].b = Serial.read();
    }

    //Update the led strip
    FastLED.show();
  }
}
