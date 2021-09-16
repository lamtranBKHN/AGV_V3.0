#include "Setup.h"
#include "CreateDataCommand.h"
#include "TimeStamp.h"
#include "Rfid.h"
#include "Mqtt.h"

void setup() {
  Serial.begin(BaundRate);
  setupWifi();
  startMqtt();
  startReadRfid();
  startCalculateTimeStamp();
}

int heartbeatCoefficient = 0;
void loop() {
  readCard();
  if(heartbeatCoefficient >= HEARTBEAT_TIME) {
      sendHeartbeat();
      heartbeatCoefficient = 0;
  }
  heartbeatCoefficient++;
  delay(REFRESH_TIME);
}
