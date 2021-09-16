#include "Mqtt.h"
#include "Setup.h"
#include "CreateDataCommand.h"
#include <PubSubClient.h>

WiFiClient espClient;
PubSubClient client(espClient);

void startMqtt() {
  client.setServer(mqtt_server, mqtt_port); 
  client.setCallback(callback);  
}

void callback(char* topic, byte* payload, unsigned int length) {  
  if(strcmp(topic, mqtt_topic_sub) == 0) {
    for (int i = 0; i < length; i++) {
      Serial.print(payload[i] < 0x10 ? "0" : "");
      Serial.print(payload[i], HEX);
    }
    Serial.println();
  }
}

void reconnect() {
  // Chờ tới khi kết nối
  while (!client.connected()) {
    Serial.print(".");
    if (client.connect("ESP8266Client",mqtt_user, mqtt_pwd)) {
    // if (client.connect("ESP8266Client")) {

      // Send login msg
      sendUpdate( createLoginFrame() );

      client.subscribe(mqtt_topic_sub);
    } else {
      delay(1000);
    }
  }
}

void sendUpdate(char* msg) {
  if (!client.connected()) {
    reconnect();
  }
  if(!client.loop()) client.connect("ESP8266Client");
  client.publish(AGV_SERVER, msg);
//  sendToStm32(msg);
}

void sendToStm32(char* data) {
  Serial.println(data);
}

bool checkConnection() {
  return client.connected();  
}

void clientLoop() {
  client.loop();
}

void sendHeartbeat(int batteryPercent) {
  sendUpdate(createHeartbeatFrame(batteryPercent));
}

void sendDebug(const char * msg) {
  client.publish(mqtt_debug_topic, msg);
}
void sendFailure(char* failureCode) {
  sendUpdate(createFailureFrame(failureCode));
}
