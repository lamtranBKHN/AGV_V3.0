#ifndef CreateDataCommand_H
#define CreateDataCommand_H

char* createRfidMeetFrame(char* rfidCode);
char* createLoginFrame();
char* createReadyFrame();
char* createStartFrame();
char* createStopFrame();
char* createPowerOnFrame();
char* createPowerOffFrame();
char* createRfidMeetFrameToStm32(char* rfidCode);
char* createHeartbeatFrame();

#endif
