
def writeTimeValueIntoFile(type, number, timeValue):

    with open(f"PredictData/PersonalPatients/{type}/timeValue/txt/{number}{type}.txt", "w") as file:
        for i in range(len(timeValue[0])):
            file.write(f"{timeValue[0][i]}\t{timeValue[1][i]}\n".encode('utf-8').decode('utf-8'))


def writeDataIntoFile(type, number, xyzc):

    with open(f"PredictData/PersonalPatients/{type}/txt/{number}{type}.txt", "w") as file:
        for i in range(len(xyzc[0])):
            file.write(f"{xyzc[0][i]}\t{xyzc[1][i]}\t{xyzc[2][i]}\t{xyzc[3][i]}\n".encode('utf-8').decode('utf-8'))


def getDataFromFile(type, number, stepX=10, stepY=10, stepZ=10):
    """
    Get cancer data from file

    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep

    Return:
    Array[X-axis coordinates, Y-axis steep, Z-axis steep, degree of cancer damage (density)]
    """

    valuesX = []
    valuesY = []
    valuesZ = []
    valuesC = []
    with open(f"D:/VolSU/НИР/ScienceArticle/NotLinearCancerModel/NotLinearCancerModel/dataTumor/PredictData/PersonalPatients/{type}/txt/{number}{type}.txt", "r") as file:
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

def getTimeValueFromFile(type, number, stepX=10, stepY=10, stepZ=10):
    """
    Get time, cancer-value (volume) from file

    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Return:
    Array[time-values, volumeCancer-values]
    """

    valuesTime = []
    valuesCancer = []
    with open(f"D:/VolSU/НИР/ScienceArticle/NotLinearCancerModel/NotLinearCancerModel/dataTumor/PredictData/PersonalPatients/{type}/timeValue/txt/{number}{type}.txt", "r") as file:
    # with open(f"../dataTumor/ModelData/personalPatients/poly3current/{type}/txt/{number}{type}.txt", "r") as file:
    # with open(f"'../dataTumor/ModelData/personalPatients/poly3current/Diameter/txt/1Diameter.txt'", "r") as file:
        for line in file.readlines():
            valuesString = line.split()
            try:
                valuesTime.append(float(valuesString[0].replace(",", ".")))
            except IndexError:
                valuesTime.append(0)
            try:
                valuesCancer.append(stepX * stepX * stepX * float(valuesString[1].replace(",", ".")))
            except IndexError:
                valuesCancer.append(0)
        if len(valuesTime)>0: 
            valuesTime.pop()
            valuesCancer.pop()
        valuesTime2 = []
        valuesCancer2 = []
        for i in range(len(valuesTime)):
            if valuesTime[i] not in valuesTime2 and valuesTime[i] < 125:
                valuesTime2.append(valuesTime[i])
                valuesCancer2.append(valuesCancer[i])
        # for i in range(len(valuesTime2)):        
        #     if valuesCancer2[i] == 0:
        #         valuesTime2.pop(i)
        #         valuesCancer2.pop(i)
        # if number == 1:
        #     for i in range(len(valuesTime2)):
        #         print(f"{valuesTime2[i]}\t{valuesCancer2[i]}")
    writeTimeValueIntoFile(type, number, [valuesTime2, valuesCancer2])
    return [valuesTime2, valuesCancer2]


