/*
 * servoDriver.cpp
 *
 *  Created on: May 4, 2021
 *      Author: lamtd
 */
#include "stm32f4xx_hal.h"
#include "settings.h"
#include "cmsis_os.h"
#include "servoDriver.h"

TIM_HandleTypeDef htim12;

void servoInit() {
	// Set servo to 0 degree
	// Duty 1 / 20
	int duty = TIM9->ARR * 0.05;
	__HAL_TIM_SET_COMPARE(&htim12, TIM_CHANNEL_1, duty);
	HAL_TIM_PWM_Start(&htim12, TIM_CHANNEL_1);
	return;
}

void liftUp() {
	// Set servo to 0 degree
	// Duty 1 / 10
	int duty = TIM9->ARR * 0.1;
	__HAL_TIM_SET_COMPARE(&htim12, TIM_CHANNEL_1, duty);
	return;
}

void liftDown() {
	// Set servo to 0 degree
	// Duty 1 / 20
	int duty = TIM9->ARR * 0.05;
	__HAL_TIM_SET_COMPARE(&htim12, TIM_CHANNEL_1, duty);
	return;
}

void turnOffServo() {
	HAL_TIM_PWM_Stop(&htim12, TIM_CHANNEL_1);
	return;
}
