#include "CreateDataCommand.h"
#include "Setup.h"
#include "TimeStamp.h"

char* createRfidMeetFrame(char* rfidCode) {
//  Frame_head  Command_code  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  AGV_ID  RFID  RFID  RFID  RFID  Parity_bit
//      ab            01          28           
  char* bufferData = (char*) malloc(29);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"01");   
  strcat(bufferData, (char*)"28");  
  strcat(bufferData, (char*)agvId); 
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, rfidCode);
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

char* createHeartbeatFrame() {
//  Frame_head  Command_code  Frame_length   AGV_ID   Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5 RFID  RFID  RFID  RFID  Parity_bit
//      ab            03          30           
  char* bufferData = (char*) malloc(31);
  strcpy(bufferData, (char*)"ab"); 
  strcat(bufferData, (char*)"03");   
  strcat(bufferData, (char*)"30");  
  strcat(bufferData, (char*)agvId); 
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"99"); // Dummy battery level
  strcat(bufferData, currentRfid);
  strcat(bufferData, (char*)"99");  // dummy parity bit
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

// Send start signal to STM32
char* createStartFrame() {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  Parity_bit
  //      aa        01          10                                                                   02
  char* bufferData = (char*) malloc(22);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"02");  
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;    
}

// Send stop signal to STM32
char* createStopFrame() {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  Parity_bit
  //      aa        01          10                                                                   03
  char* bufferData = (char*) malloc(22);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"03");  
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;    
}

// Create and send power ON signal to STM32
char* createPowerOnFrame() {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  Parity_bit
  //      aa        01          10                                                                   04
  char* bufferData = (char*) malloc(22);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"04");  
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;    
}

// Create and send power OFF signal to STM32
char* createPowerOffFrame() {
  //  Frame_head  AGV_ID  Frame_length  Timestamp1  Timestamp2  Timestamp3  Timestamp4  Timestamp5  code  Parity_bit
  //      aa        01          10                                                                   05
  char* bufferData = (char*) malloc(22);
  strcpy(bufferData, (char*)"aa"); 
  strcat(bufferData, (char*)agvId);   
  strcat(bufferData, (char*)"10");  
  strcat(bufferData, getTimeStamp()); 
  strcat(bufferData, (char*)"05");  
  strcat(bufferData, (char*)"99");  // dummy parity bit
  return bufferData;    
}
// 
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
