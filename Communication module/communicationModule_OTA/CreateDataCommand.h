#ifndef CreateDataCommand_H
#define CreateDataCommand_H

char* createRfidMeetFrame(char* rfidCode);
char* createLoginFrame();
char* createReadyFrame();
char* createRfidMeetFrameToStm32(char* rfidCode);
char* createHeartbeatFrame(int batteryPercent);
char* createFailureFrame(char* failureCode);

#endif
