#ifndef Mqtt_H
#define Mqtt_H

#include "Setup.h"

void startMqtt();
void callback(char* topic, byte* payload, unsigned int length);
void reconnect();
void sendUpdate(char* msg);
void sendToStm32(char* data);
bool checkConnection();
void clientLoop();
void sendHeartbeat();

#endif
