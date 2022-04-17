
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
                valuesY.append(stepX * float(valuesString[1].replace(",", ".")))
            except IndexError:
                valuesY.append(0)
            try:
                valuesZ.append(stepX * float(valuesString[2].replace(",", ".")))
            except IndexError:
                valuesZ.append(0)
            try:
                valuesC.append(float(valuesString[3].replace(",", ".")))
            except IndexError:
                valuesC.append(0)
            
                
            i +=1
            #print(i, number)
            valuesX.pop(-1)
            valuesY.pop(-1)
            valuesZ.pop(-1)
            valuesC.pop(-1)
    return [valuesX, valuesY, valuesZ, valuesC]

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
            valuesTime.pop(-1)
            valuesCancer.pop(-1)
    return [valuesTime, valuesCancer]