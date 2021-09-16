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
  // Recieved start command from Server
  if(strcmp(topic, MQTT_SUB_START) == 0) {
    Serial.print(createStartFrame());
    Serial.println();
  } 
  // Recieved stop command from server
  else if(strcmp(topic, MQTT_SUB_STOP) == 0) {
    Serial.print(createStopFrame());
    Serial.println();
  }
  // Recieved Power ON command from server
  else if(strcmp(topic, MQTT_SUB_ON) == 0) {
    Serial.print(createPowerOnFrame());
    Serial.println();
  }
  // Recieved Power OFF command from server
  else if(strcmp(topic, MQTT_SUB_OFF) == 0) {
    Serial.print(createPowerOffFrame());
    Serial.println();
  }
  else if(strcmp(topic, mqtt_topic_sub) == 0) {
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
    if (client.connect("ESP8266Client",mqtt_user, mqtt_pwd)) {

      // Send login msg
      sendUpdate( createLoginFrame() );

      client.subscribe(mqtt_topic_sub);
      client.subscribe(MQTT_SUB_START);
      client.subscribe(MQTT_SUB_STOP);
      client.subscribe(MQTT_SUB_ON);
      client.subscribe(MQTT_SUB_OFF);
    } else {
      delay(5000);
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

void sendHeartbeat() {
  sendUpdate(createHeartbeatFrame());
}
