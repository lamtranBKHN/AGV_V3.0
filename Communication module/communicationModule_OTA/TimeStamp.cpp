#include "TimeStamp.h"
#include "Setup.h"
#include <NTPClient.h>
#include <WiFiUdp.h>

// Define NTP Client to get time
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org", utcOffsetInSeconds); 

char* getTimeStamp() {
  char * timeStamp = (char *) malloc(11);
  timeClient.update();
  long newTimeStamp = timeClient.getEpochTime();
  sprintf(timeStamp, "%ld", newTimeStamp);
  return timeStamp;
}

void startCalculateTimeStamp() {
  timeClient.begin();  
}
