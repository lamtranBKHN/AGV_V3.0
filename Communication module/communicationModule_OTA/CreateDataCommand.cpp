#include "CreateDataCommand.h"
#include "Setup.h"
#include "TimeStamp.h"

char* createRfidMeetFrame(char* rfidCode) {
//  Frame_head  Command_code  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  AGV_ID  RFID  RFID  RFID  RFID  RFID  RFID  RFID  Parity_bit
//      ab            01          34           
  char* bufferData = (char*) malloc(35);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"01");   
  strcat(bufferData, (char*)"34");  
  strcat(bufferData, (char*)agvId); 
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, rfidCode);
  
  // Check rfid length 
  int rfidLength = strlen(rfidCode) / 2;  

  if(rfidLength < 7) {
    int missingLength = 7 - rfidLength;
    while(missingLength--) {
      strcat(bufferData, (char*)"00");
    }  
  }
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}

// Send ready signal to Server
char* createLoginFrame() {
//  Frame_head  Command_code  Frame_length  AGV_ID   Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  Password   Parity_bit
//      ab            02          22           
  char* bufferData = (char*) malloc(23);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"02");   
  strcat(bufferData, (char*)"22");  
  strcat(bufferData, (char*)agvId);    
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)PASS_AGV_SYS);
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}

char* createHeartbeatFrame(int batteryPercent) {
//  Frame_head  Command_code  Frame_length   AGV_ID   Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5 RFID  RFID  RFID  RFID   RFID  RFID  RFID  Parity_bit
//      ab            03          36           
  char* bufferData = (char*) malloc(38);
  strcpy(bufferData, "ab"); 
  strcat(bufferData, "03");   
  strcat(bufferData, "38");  
  strcat(bufferData, agvId);   
  strcat(bufferData, getTimeStamp());   
  strcat(bufferData, (char*)batteryPercent); // Dummy battery level  
  strcat(bufferData, currentRfid);
  // Check rfid length 
  int rfidLength = strlen(currentRfid) / 2; 
  
  if(rfidLength < 7) {
    int missingLength = 7 - rfidLength;
    while(missingLength--) {
      strcat(bufferData, "00");
    }  
  }
  strcat(bufferData, "01");  // dummy direction
  strcat(bufferData, "99");  // dummy parity bit
  return bufferData;                                                      
}

// Send failure signal to Server
char* createFailureFrame(char* failureCode) {
//  Frame_head  Command_code  Frame_length  AGV_ID   Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  Failure_code   Parity_bit
//      ab            04          22           
  char* bufferData = (char*) malloc(23);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"04");   
  strcat(bufferData, (char*)"22");  
  strcat(bufferData, (char*)agvId);    
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)failureCode);
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}


// Send Ready signal to STM32
char* createReadyFrame() {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  Parity_bit
  //      aa        01          10                                                                   06
  char* bufferData = (char*) malloc(22);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"06");  
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;    
}

char* createRfidMeetFrameToStm32(char* rfidCode) {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  RFID1 RFID2 RFID3 RFID4 Parity_bit
  //      aa        01          14                                                                   01
  char* bufferData = (char*) malloc(31);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"01");  
  strcat(bufferData, rfidCode);
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;                                                      
}
