#include "Rfid.h"
#include "Mqtt.h"
#include "CreateDataCommand.h"
#include "Setup.h"
#include <SPI.h>
#include <MFRC522.h>

MFRC522 mfrc522(SS_PIN, RST_PIN);   // Create MFRC522 instance
//char* currentRfid = "dummy";

void startReadRfid() {
  SPI.begin();          // Initiate  SPI bus
  mfrc522.PCD_Init();   // Initiate MFRC522  
}

void readCard() {
  if (!checkConnection()) {
    reconnect();
  }
  clientLoop();
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

  // Compare to old RFID  
  if(strcmp(char_array, currentRfid) == 0) return;
  sendUpdate(createRfidMeetFrame(char_array));
  // Send to Stm32
  Serial.print(createRfidMeetFrameToStm32(char_array));
  Serial.println();
  // Save old RFID
  strcpy(currentRfid, char_array);
}

void remove_spaces(char* s) {
    const char* d = s;
    do {
        while (*d == ' ') {
            ++d;
        }
    } while (*s++ = *d++);
}
