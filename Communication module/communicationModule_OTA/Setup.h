#ifndef Setup_H
#define Setup_H
#include <ESP8266WiFi.h>

#define ssid              "Home"
#define wifi_password     "244466666"
#define mqtt_server       "192.168.1.3" 
#define mqtt_topic_pub    "AGV01"  
#define mqtt_topic_sub    "agvWorker1"
#define mqtt_debug_topic  "agv01_debug"
#define mqtt_user         "lam"    
#define mqtt_pwd          "a"
#define agvId             "01"
#define AGV_SERVER        "agvHandler"
#define PASS_AGV_SYS      "12"
#define SS_PIN            2
#define RST_PIN           0
#define BaundRate         115200
#define QOS_MQTT          2
#define MQTT_SUB_START    "AGV01Start"
#define MQTT_SUB_STOP     "AGV01Stop"
#define MQTT_SUB_ON       "AGV01On"
#define MQTT_SUB_OFF      "AGV01Off"
#define REFRESH_TIME      10    // ms
#define HEARTBEAT_TIME    100  // 1000ms : 10
#define BATTERY_LOW       1000 // test dummy
#define BATTERY_FULL      3000 // test dummy

const uint16_t mqtt_port      = 1883;
const long utcOffsetInSeconds = 0; 
extern char* currentRfid;

void setupWifi();
#endif
