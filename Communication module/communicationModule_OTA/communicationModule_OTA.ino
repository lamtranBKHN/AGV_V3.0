#include "Setup.h"
#include "CreateDataCommand.h"
#include "TimeStamp.h"
#include "Rfid.h"
#include "Mqtt.h"
#include <ESP8266mDNS.h>
#include <WiFiUdp.h>
#include <ArduinoOTA.h>

void setup() {
  Serial.begin(BaundRate);
  setupWifi();
  startMqtt();
  startReadRfid();
  startCalculateTimeStamp();
}

int heartbeatCoefficient  = 0;
double batteryVoltage     = 0;
int batteryPercentage     = 0;

void loop() {
  readCard();
  batteryVoltage = ( batteryVoltage + analogRead(A0) ) / 2;
  
  // if(heartbeatCoefficient >= HEARTBEAT_TIME) {
  //   batteryPercentage = 100 * (batteryVoltage - BATTERY_LOW) / (BATTERY_FULL - BATTERY_LOW);
  //   if(batteryVoltage <= BATTERY_LOW) {
  //     // Send battery low to server
  //     sendFailure("02");
  //   } else {
  //     // Calculate battery percentage      
  //     sendHeartbeat(batteryPercentage);
  //     heartbeatCoefficient = 0;
  //   }
    
  //   ArduinoOTA.handle();
  // }
  // heartbeatCoefficient++;
  ArduinoOTA.handle();
  delay(REFRESH_TIME);
}
