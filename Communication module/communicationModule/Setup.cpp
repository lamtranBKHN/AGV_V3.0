#include "Setup.h"
#include "CreateDataCommand.h"


char* currentRfid = "C3903C36";

void setupWifi() {
  delay(10);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
//    Serial.println(".");
    delay(500);
  }
  // Send ready signal to STM32 
  Serial.print(createReadyFrame());
  Serial.println();
//  Serial.println(WiFi.localIP());
  WiFi.softAP(mqtt_topic_pub, password);
}
