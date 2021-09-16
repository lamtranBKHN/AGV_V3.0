/*
 * converter.cpp
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */
#include "converter.h"
#include "math.h"

int hex2dec(char hex1[1], char hex2[1])
{
    int result = 0;
    result += char2dec(hex1, 1);
    result += char2dec(hex2, 0);
    return result;
}

int twoHex2dec(char hex1[1], char hex2[1], char hex3[1], char hex4[1])
{
    int result = 0;
    result += char2dec(hex1, 1);
    result += char2dec(hex2, 0);
    result += char2dec(hex3, 3);
    result += char2dec(hex4, 2);
    return result;
}

int char2dec (char charNum[1], int i) {
	int result = 0;
	if (charNum[0] >= 48 && charNum[0] <= 57) {
		result += (charNum[0] - 48) * pow(16, i);
	} else if (charNum[0] >= 65 && charNum[0] <= 70) {
		result += (charNum[0] - 55) * pow(16, i);
	} else if (charNum[0]>=97 && charNum[0]<=102) {
		result += (charNum[0] - 87) * pow(16, i);
	}
	return result;
}


