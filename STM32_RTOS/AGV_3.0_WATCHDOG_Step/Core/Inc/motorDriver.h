/*
 * motorDriver.h
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */

#ifndef INC_MOTORDRIVER_H_
#define INC_MOTORDRIVER_H_



#include <stdint.h>


void stepDirectMove(uint32_t step);
void stepRotate(bool clockwise, uint32_t step);
void directMove();
void turnLeft();
void turnRight();
void turnBack();
void moveBack();


#endif /* INC_MOTORDRIVER_H_ */
