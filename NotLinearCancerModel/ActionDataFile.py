
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
    with open(f"dataTumor/PredictData/PersonalPatients/{type}/txt/{number}{type}.txt", "r") as file:
    # with open(f"../dataTumor/ModelData/personalPatients/poly3current/{type}/txt/{number}{type}.txt", "r") as file:
    # with open(f"'../dataTumor/ModelData/personalPatients/poly3current/Diameter/txt/1Diameter.txt'", "r") as file:
        for line in file.readlines():
            valuesString = line.split()
            valuesX.append(stepX * float(valuesString[0]))
            valuesY.append(stepX * float(valuesString[1]))
            valuesZ.append(stepX * float(valuesString[2]))
            valuesC.append(float(valuesString[3]))
    return [valuesX, valuesY, valuesZ, valuesC]

def getTimeValueFromFile(type, number):
    """
    Get time, cancer-value (volume) from file

    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Return:
    Array[]
    """

    valuesTime = []
    valuesCancer = []
    with open(f"dataTumor/PredictData/PersonalPatients/{type}/timeValue/txt/{number}{type}.txt", "r") as file:
    # with open(f"../dataTumor/ModelData/personalPatients/poly3current/{type}/txt/{number}{type}.txt", "r") as file:
    # with open(f"'../dataTumor/ModelData/personalPatients/poly3current/Diameter/txt/1Diameter.txt'", "r") as file:
        for line in file.readlines():
            valuesString = line.split()
            valuesTime.append(float(valuesString[0]))
            valuesCancer.append(stepX * stepX * stepX * float(valuesString[1]))
    return [valuesTime, valuesCancer]