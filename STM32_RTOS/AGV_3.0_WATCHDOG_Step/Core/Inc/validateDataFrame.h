/*
 * validateDataFrame.h
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */

#ifndef INC_VALIDATEDATAFRAME_H_
#define INC_VALIDATEDATAFRAME_H_


#include <vector>

bool checkEmptyFrame(std::vector<char> parsedData);
bool checkFrameHead(std::vector<char> parsedData);
bool checkAgvId(std::vector<char> parsedData);
bool checkFrameLength(std::vector<char> parsedData);
bool checkDataFrame(std::vector<char> parsedData);


#endif /* INC_VALIDATEDATAFRAME_H_ */
