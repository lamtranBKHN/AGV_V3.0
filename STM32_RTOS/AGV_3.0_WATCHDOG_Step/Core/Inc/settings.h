/*
 * settings.h
 *
 *  Created on: May 26, 2021
 *      Author: lamtd
 */

#ifndef INC_SETTINGS_H_
#define INC_SETTINGS_H_

#include <stdint.h>

struct SystemStatus {
	bool power;
	bool running;
	bool fail;
	char lastRfid[8];
};

extern struct SystemStatus systemStatus;

extern const char* AGV_ID ;
extern bool runningStatus;

extern uint32_t icValue1;
extern uint32_t icValue2;
extern bool isFirstCaptured;
extern uint32_t distance;

#define BAUD_RATE			115200

#define DIRECT_MOVE_STEP	100
#define TURN_90_STEP		98
#define TURN_180_STEP		194
#define SPEED				0.4
#define SAFETY_DISTANCE		40
#define MAX_DELAY			5
#define MIN_DELAY			1
#define ACC_DEC_RANGE		64		// 64 steps acceleration and deceleration
#endif /* INC_SETTINGS_H_ */
