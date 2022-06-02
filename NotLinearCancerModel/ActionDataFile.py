import codecs

def writeTimeValueIntoFile(type, number, timeValue, path = "PredictData/PersonalPatients/"):
    path = path + str(type) + "/timeValue/txt/" + str(number) + str(type) + ".txt"
    with open(path, "w") as file:
        for i in range(len(timeValue[0])):
            file.write(f"{timeValue[0][i]}\t{timeValue[1][i]}\n".encode('utf-8').decode('utf-8'))


def writeDataIntoFile(type, number, xyzc, path = "PredictData/PersonalPatients/"):
    path = "PredictData/PersonalPatients/" + str(type) + "/txt/" + str(number) + str(type) + ".txt"
    with open(path, "w") as file:
        for i in range(len(xyzc[0])):
            file.write(f"{xyzc[0][i]}\t{xyzc[1][i]}\t{xyzc[2][i]}\t{xyzc[3][i]}\n".encode('utf-8').decode('utf-8'))


def getDataFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "../../../dataTumor/PredictData/PersonalPatients/"):
    """
    Get cancer data from file

    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[X-axis coordinates, Y-axis steep, Z-axis steep, degree of cancer damage (density)]
    """

    path = path + str(type) + "/txt/" + str(number) + str(type) + ".txt"
    valuesX = []
    valuesY = []
    valuesZ = []
    valuesC = []
    with open(path, "r") as file:
    # with open(f"../dataTumor/ModelData/personalPatients/poly3current/{type}/txt/{number}{type}.txt", "r") as file:
    # with open(f"'../dataTumor/ModelData/personalPatients/poly3current/Diameter/txt/1Diameter.txt'", "r") as file:
        i = 0
        for line in file.readlines():
            valuesString = line.split()
            try:
                valuesX.append(stepX * float(valuesString[0].replace(",", ".")))
            except IndexError:
                valuesX.append(0)
            try:
                valuesY.append(stepY * float(valuesString[1].replace(",", ".")))
            except IndexError:
                valuesY.append(0)
            try:
                valuesZ.append(stepZ * float(valuesString[2].replace(",", ".")))
            except IndexError:
                valuesZ.append(0)
            try:
                valuesC.append(float(valuesString[3].replace(",", ".")))
            except IndexError:
                valuesC.append(0)
            
            
            #print(i, number)
        valuesX.pop()
        valuesY.pop()
        valuesZ.pop()
        valuesC.pop()
        valuesX2 = []
        valuesY2 = []
        valuesZ2 = []
        valuesC2 = []
        for i in range(len(valuesC)):
            if valuesC[i] != 0:
                valuesX2.append(valuesX[i])
                valuesY2.append(valuesY[i])
                valuesZ2.append(valuesZ[i])
                valuesC2.append(valuesC[i])    
        # for i in range(len(valuesC)):
        #     print(f"{valuesX2[i]}\t{valuesY2[i]}\t{valuesZ2[i]}\t{valuesC2[i]}")
    writeDataIntoFile(type, number, [valuesX2, valuesY2, valuesZ2, valuesC2])
    return [valuesX2, valuesY2, valuesZ2, valuesC2]


def getTimeValueFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "../../../dataTumor/PredictData/PersonalPatients/"):
    """
    Get time, cancer-value (volume) from file


    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[time-values, volumeCancer-values]
    """

    path = path + str(type) + "/timeValue/txt/" + str(number) + str(type) + ".txt"
    valuesTime = []
    valuesCancer = []

    with open(path, "r") as file:
        for line in file.readlines():
            valuesString = line.split()

            try:
                valuesTime.append(float((valuesString[0].replace(",", ".").strip())))
            except IndexError:
                valuesTime.append(0)
            try:
                # valuesCancer.append(stepX * stepX * stepX * float(valuesString[1].replace(",", ".")))
                valuesCancer.append(float((valuesString[1].replace(",", ".").strip())))
            except IndexError:
                valuesCancer.append(0)
        # if len(valuesTime)>0: 
        #     valuesTime.pop()
        #     valuesCancer.pop()
        valuesTime2 = []
        valuesCancer2 = []
        for i in range(len(valuesTime)):
            # if valuesTime[i] not in valuesTime2 and valuesTime[i] < 125:
            if valuesTime[i] not in valuesTime2:
                valuesTime2.append(valuesTime[i])
                valuesCancer2.append(valuesCancer[i])
        # for i in range(len(valuesTime2)):        
        #     if valuesCancer2[i] == 0:
        #         valuesTime2.pop(i)
        #         valuesCancer2.pop(i)
        # if number == 1:
        #     for i in range(len(valuesTime2)):
        #         print(f"{valuesTime2[i]}\t{valuesCancer2[i]}")
        # for i in range(len(valuesTime2)):
        #     print(valuesTime2[i])
        #     print(valuesCancer2[i])
    writeTimeValueIntoFile(type, number, [valuesTime2, valuesCancer2])
    return [valuesTime2, valuesCancer2]


def getExperimentalDataFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "../../../dataTumor/ExperimentalData/"):
    """
    Get experimental time, cancer-value (volume) from file


    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[time-values, volumeCancer-values]
    """

    path = path + str(type) + "/" + str(number) + str(type) + ".txt"
    
    valuesTime = []
    valuesCancer = []
    with open(path, "r") as file:
        for line in file.readlines():
            valuesString = line.split()

            try:
                valuesTime.append(float((valuesString[0].replace(",", ".")).strip()) / 30)
                # valuesTime.append(float((valuesString[0].replace(",", ".")).strip()))
            except IndexError:
                valuesTime.append(0)
            try:
                # valuesCancer.append(stepX * stepX * stepX * float(valuesString[1].replace(",", ".")))
                valuesCancer.append(float((valuesString[1].replace(",", ".")).strip()))
            except IndexError:
                valuesCancer.append(0)

    return [valuesTime, valuesCancer]



def compareData(type, number, experimentalData, modelData, stepX=10, stepY=10, stepZ=10, path = "../../../dataTumor/ExperimentalData/"):
    """
    Get comparable data between


    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[time-values, volumeCancer-values]
    """

    absoluteError = []
    relativeError = 0
    difference = []
    for i in range(len(experimentalData[0])):

        for iLenModelData in range(len(modelData)):
            difference.append(abs(experimentalData[0][i] - modelData[0][iLenModelData]))
        indexMin = difference.index(min(difference))
        relativeError += abs(modelData[1][indexMin] - experimentalData[1][i]) / experimentalData[1][i]

    return relativeError / len(experimentalData[0])