/*
 * parseDataCommand.cpp
 *
 *  Created on: Mar 11, 2021
 *      Author: lamtd
 */
#include "parseDataCommand.h"
#include "converter.h"
#include "validateDataFrame.h"

std::vector<DataStruct> parseData(std::vector<char> receivedRawData) {
	std::vector<DataStruct> parsedData;

	// Validate received data
	if(!checkDataFrame(receivedRawData)) return parsedData;

	// Parse data
	for(uint32_t i = 16; i < receivedRawData.size() - 14; i += 14) {
		struct DataStruct singleData;

		singleData.action = hex2dec(&receivedRawData.at(i), &receivedRawData.at(i + 1));
		for(int index = 0; index < 8; index++)  {
			singleData.rfid[index] =  receivedRawData.at(i + 2 + index);
		}
		singleData.velocity = hex2dec(&receivedRawData.at(i + 10), &receivedRawData.at(i + 11));
		singleData.distance = hex2dec(&receivedRawData.at(i + 12), &receivedRawData.at(i + 13));
		parsedData.push_back(singleData);
	}
	return parsedData;
}



