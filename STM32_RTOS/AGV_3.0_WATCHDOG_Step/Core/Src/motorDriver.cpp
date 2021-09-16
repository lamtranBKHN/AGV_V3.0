/*
 * motorDriver.cpp
 *
 *  Created on: May 26, 2021
 *      Author: lamtd
 */
#include "settings.h"
#include "motorDriver.h"
#include "main.h"
#include "cmsis_os.h"

uint32_t accelerateDelayCalculation(uint32_t remainStep, uint32_t movingStep) {
	// Cruise
	if(remainStep > ACC_DEC_RANGE && remainStep < movingStep - ACC_DEC_RANGE) return MIN_DELAY / SPEED;
	// Accelerate
	else if( remainStep <= ACC_DEC_RANGE ) {
		uint32_t delayTime = (double)( MAX_DELAY / SPEED ) - (double)( ( ( MAX_DELAY - MIN_DELAY ) * remainStep) / (ACC_DEC_RANGE * SPEED) );
		return delayTime;
	} else {	// Decelerate
		uint32_t delayTime = (double)( MIN_DELAY / SPEED ) + (double)( ( (MAX_DELAY - MIN_DELAY ) * remainStep) / (ACC_DEC_RANGE * SPEED));
		return delayTime;
	}
}

void stepDirectMove(uint32_t step) {
	uint32_t movingStep = step;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, movingStep) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void stepRotate(bool clockwise, uint32_t step) {
	uint32_t movingStep = step;
	// Set direction
	if(clockwise) {
		HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	} else {
		HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_SET);
	}

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, movingStep) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void directMove() {
	uint32_t step = DIRECT_MOVE_STEP;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_SET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_SET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, DIRECT_MOVE_STEP) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void turnLeft() {
	uint32_t step = TURN_90_STEP;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_SET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, TURN_90_STEP) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void turnRight() {
	uint32_t step = TURN_90_STEP;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_SET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, TURN_90_STEP) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void turnBack() {
	uint32_t step = TURN_180_STEP;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_SET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, TURN_180_STEP) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}

void moveBack() {
	uint32_t step = DIRECT_MOVE_STEP;
	// Set direction
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);

	double delaytime  = (double)( 1 / SPEED ) ;

	while(step--) {
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_SET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_SET);
		osDelay( delaytime );
		HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
		osDelay( delaytime );
//		osDelay( accelerateDelayCalculation(step, DIRECT_MOVE_STEP) );
	}

	// Reset
	HAL_GPIO_WritePin(motor_left_dir_GPIO_Port, motor_left_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_dir_GPIO_Port, motor_right_dir_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_left_step_GPIO_Port, motor_left_step_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(motor_right_step_GPIO_Port, motor_right_step_Pin, GPIO_PIN_RESET);
}
