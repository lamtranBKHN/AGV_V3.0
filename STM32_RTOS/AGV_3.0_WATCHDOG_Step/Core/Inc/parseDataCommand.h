/*
 * parseDataCommand.h
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */

#ifndef INC_PARSEDATACOMMAND_H_
#define INC_PARSEDATACOMMAND_H_


#include <vector>
#include "dataStruct.h"

std::vector<DataStruct> parseData(std::vector<char> receivedData);


#endif /* INC_PARSEDATACOMMAND_H_ */
