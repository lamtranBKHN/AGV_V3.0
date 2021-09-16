/*
 * validateDataFrame.cpp
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */
#include "validateDataFrame.h"
#include "settings.h"
#include "math.h"
#include "converter.h"

extern const char* AGV_ID;

bool checkEmptyFrame(std::vector<char> parsedData) {
	if(parsedData.size() == 0) return false;
	else return true;
}
bool checkFrameHead(std::vector<char> parsedData) {
	// Frame Head is always "BA"
	bool checkingResult = false;

	if(parsedData.at(0) == 'B') checkingResult = true;
	if(parsedData.at(1) == 'A') checkingResult = true;

	return checkingResult;
}

bool checkAgvId(std::vector<char> parsedData) {
	bool checkingResult = false;
	if(parsedData.at(2) == AGV_ID[0] && parsedData.at(3) == AGV_ID[1]) checkingResult = true;
	return checkingResult;
}

bool checkFrameLength(std::vector<char> parsedData) {
//	int frameLenght = hex2dec(&parsedData.at(4), &parsedData.at(5));
	int frameLenght = twoHex2dec(&parsedData.at(4), &parsedData.at(5), &parsedData.at(6), &parsedData.at(7));
	int actualFrameLength = parsedData.size() / 2;
	bool checkingResult = false;
	if(frameLenght == actualFrameLength) checkingResult = true;
	return checkingResult;
}

bool checkDataFrame(std::vector<char> parsedData) {
	if(!checkEmptyFrame(parsedData)) return false;
	if((checkFrameHead(parsedData) && checkFrameLength(parsedData)) && checkAgvId(parsedData)) return true;
	else return false;
}



