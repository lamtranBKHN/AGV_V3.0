#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <SPI.h>
#include <MFRC522.h>
#include <NTPClient.h>
#include <WiFiUdp.h>

#define ssid            "P1705"
#define password        "1234567890"

#define mqtt_server     "192.168.1.68" 
#define mqtt_topic_pub  "AGV01"  
#define mqtt_topic_sub  "agvWorker1"
#define mqtt_user       "lam"    
#define mqtt_pwd        "a"
#define agvId           "01"
#define AGV_SERVER      "AGV01"
#define PASS_AGV_SYS    "12"

const uint16_t mqtt_port      = 1883;
const long utcOffsetInSeconds = 0;

#define SS_PIN 2
#define RST_PIN 0
MFRC522 mfrc522(SS_PIN, RST_PIN);   // Create MFRC522 instance

WiFiClient espClient;
PubSubClient client(espClient);
// Define NTP Client to get time
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org", utcOffsetInSeconds); 

// ************************* USER FUNCTIONS ******************************** //
void remove_spaces(char* s) {
    const char* d = s;
    do {
        while (*d == ' ') {
            ++d;
        }
    } while (*s++ = *d++);
}

void sendUpdate(char* msg) {
  if (!client.connected()) {
    reconnect();
  }
  if(!client.loop()) client.connect("ESP8266Client");
  client.publish(AGV_SERVER, msg);
  sendToStm32(msg);
}

char* getTimeStamp() {
  char * timeStamp = (char *) malloc(11);
  timeClient.update();
  long newTimeStamp = timeClient.getEpochTime();
  sprintf(timeStamp, "%ld", newTimeStamp);
  return timeStamp;
}

char* createRfidMeetFrame(char* rfidCode) {
//  Frame_head  Command_code  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  AGV_ID  RFID  RFID  RFID  RFID  Parity_bit
//      ab            01          28           
  char* bufferData = (char *) malloc(29);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"01");   
  strcat(bufferData, (char*)"28 ");  
  strcat(bufferData, (char*)agvId); 
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, rfidCode);
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}

char* createLoginFrame() {
//  Frame_head  Command_code  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  AGV_ID  RFID  RFID  RFID  RFID  Parity_bit
//      ab            02          22           
  char* bufferData = (char *) malloc(23);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"02");   
  strcat(bufferData, (char*)"22");  
  strcat(bufferData, (char*)agvId); 
  strcat(bufferData, (char*)PASS_AGV_SYS); 
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}

void sendToStm32(char* data) {
  Serial.println(data);
}
// ************************************************************************* //

void setup() {
  Serial.begin(9600);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port); 
  client.setCallback(callback);
  SPI.begin();          // Initiate  SPI bus
  mfrc522.PCD_Init();   // Initiate MFRC522
  timeClient.begin();
}
// Hàm kết nối wifi
void setup_wifi() {
  delay(10);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.println(".");
    delay(500);
  }
  Serial.println(WiFi.localIP());
}


// Hàm call back để nhận dữ liệu
void callback(char* topic, byte* payload, unsigned int length) {
  for (int i = 0; i < length; i++) {
      Serial.print(payload[i] < 0x10 ? "0" : "");
      Serial.print(payload[i], HEX);
//      Serial.print(" ");
  }
  Serial.println();
}



// Hàm reconnect thực hiện kết nối lại khi mất kết nối với MQTT Broker
void reconnect() {
  // Chờ tới khi kết nối
  while (!client.connected()) {
    if (client.connect("ESP8266Client",mqtt_user, mqtt_pwd)) {

      // Send login msg
      sendUpdate( createLoginFrame() );

      client.subscribe(mqtt_topic_sub);
    } else {
      delay(5000);
    }
  }
}

// read rfid card
void readCard() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
  if ( ! mfrc522.PICC_IsNewCardPresent()) 
  {
    return;
  }
  if ( ! mfrc522.PICC_ReadCardSerial()) 
  {
    return;
  }
  String content= "";
  for (byte i = 0; i < mfrc522.uid.size; i++) 
  {
     content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
     content.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  content.toUpperCase();
  int str_len = content.substring(1).length() + 1;
  char char_array[str_len];
  char pre_char_array[str_len];
  content.substring(1).toCharArray(char_array, str_len);
  remove_spaces(char_array);
  sendUpdate( createRfidMeetFrame(char_array));
}

void loop() {
  readCard();
  delay(100);
}
