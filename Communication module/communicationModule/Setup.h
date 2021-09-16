#ifndef Setup_H
#define Setup_H
#include <ESP8266WiFi.h>

#define ssid            "AGV3.0"
#define password        "test12345"
#define mqtt_server     "192.168.1.68" 
#define mqtt_topic_pub  "AGV01"  
#define mqtt_topic_sub  "agvWorker1"
#define mqtt_user       "lam"    
#define mqtt_pwd        "a"
#define agvId           "01"
#define AGV_SERVER      "agvHandler"
#define PASS_AGV_SYS    "12"
#define SS_PIN          2
#define RST_PIN         0
#define BaundRate       19200
#define QOS_MQTT        2
#define MQTT_SUB_START  "AGV01Start"
#define MQTT_SUB_STOP   "AGV01Stop"
#define MQTT_SUB_ON     "AGV01On"
#define MQTT_SUB_OFF    "AGV01Off"
#define REFRESH_TIME    10  // ms
#define HEARTBEAT_TIME  100  // 1000ms : 10

const uint16_t mqtt_port      = 1883;
const long utcOffsetInSeconds = 0;

extern char* currentRfid;

void setupWifi();
#endif
