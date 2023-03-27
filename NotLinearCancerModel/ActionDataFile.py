import codecs
import locale
from os.path import dirname, join
from os.path import getctime, getmtime
from datetime import datetime as dt

locale.setlocale(locale.LC_ALL, 'en_US')


def writeTimeValueIntoFile(timeValue, path = "dataTumor/PredictData/PersonalPatients/"):
    """
    Write data time value cancer data into file


    Positional argument:
    timeValue -- timeValue data

    Keywords argiments:
    path -- path to directory file

    Return:
    1
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)

    with open(path, "w") as file:
        for i in range(len(timeValue[0])):
            file.write(f"{timeValue[0][i]}\t{timeValue[1][i]}\n".encode('utf-8').decode('utf-8'))
    return 1


def writeDataIntoFile(xyzc, path = "dataTumor/PredictData/PersonalPatients/"):
    """
    Write data cancer data into file


    Positional argument:
    xyzc -- timeValue data

    Keywords argiments:
    path -- path to directory file

    Return:
    1
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)

    with open(path, "w") as file:
        for i in range(len(xyzc[0])):
            file.write(f"{xyzc[0][i]}\t{xyzc[1][i]}\t{xyzc[2][i]}\t{xyzc[3][i]}\n".encode('utf-8').decode('utf-8'))
    return 1


def writeAccuracyIntoFile(relativeError, path="dataTumor/PredictData/PersonalPatients/"):
    """
    Write accuracy data into file


    Positional argument:
    relativeError -- relative error

    Keywords argiments:
    path -- path to directory file

    Return:
    1
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)

    relativeError = str(relativeError).replace(".", ",")
    # calculate line count
    with open(path, "r") as file:
        count = sum(1 for line in file if line.rstrip('\n'))  
    # delete last line if it exists because there are relativeError 
    if count > 11:
        with open(path, 'r') as file:
            lines = file.readlines()
            lines = lines[:-1]

        with open(path, 'w') as file:
            file.writelines(lines)
    with open(path, 'a') as file:
        file.writelines("RelativeError\t{}".format(relativeError))
    return 1


def getDataFromFile(stepX=10, stepY=10, stepZ=10, path = "dataTumor/PredictData/PersonalPatients/"):
    """
    Get cancer data from file


    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[X-axis coordinates, Y-axis steep, Z-axis steep, degree of cancer damage (density)]
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)
    valuesX = []
    valuesY = []
    valuesZ = []
    valuesC = []
    with open(path, "r") as file:
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

    return [valuesX2, valuesY2, valuesZ2, valuesC2]


def getTimeValueFromFile(path = "dataTumor/PredictData/PersonalPatients/"):
    """
    Get time, cancer-value (volume) from file


    Keywords argiments:
    path -- path to directory file

    Return:
    Array[time-values, volumeCancer-values]
    """
    
    current_dir = dirname(__file__)
    path = join(current_dir, path)
    print("path\t" + path)
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
                valuesCancer.append(float((valuesString[1].replace(",", ".").strip())))
            except IndexError:
                valuesCancer.append(0)

        valuesTime2 = []
        valuesCancer2 = []
        for i in range(len(valuesTime)):
            if valuesTime[i] not in valuesTime2:
                valuesTime2.append(valuesTime[i])
                valuesCancer2.append(valuesCancer[i])
    return [valuesTime2, valuesCancer2]


def getExperimentalDataFromFile(path = "dataTumor/ExperimentalData/"):
    """
    Get experimental time, cancer-value (volume) from file


    Keywords argiments:
    path -- path to directory file

    Return:
    Array[time-values, volumeCancer-values]
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)

    valuesTime = []
    valuesCancer = []
    with open(path, "r") as file:
        for line in file.readlines():
            valuesString = line.split()

            try:
                valuesTime.append(float((valuesString[0].replace(",", ".")).strip()) / 30)
            except IndexError:
                valuesTime.append(0)
            try:
                valuesCancer.append(float((valuesString[1].replace(",", ".")).strip()))
            except IndexError:
                valuesCancer.append(0)

    return [valuesTime, valuesCancer]


def getParamsFromFile(path="dataTumor/PredictData/PersonalPatients/"):
    """
    Get params data about patient from file
 

    Keywords argiments:
    path -- path to directory file

    Return:
    Array[paramsName, paramsValues]
    """

    current_dir = dirname(__file__)
    path = join(current_dir, path)
    paramsName = []
    paramsValues = []
    with open(path, "r") as file:
         for line in file.readlines():
            valuesString = line.split()

            try:
                paramsName.append(valuesString[0].strip())
            except IndexError:
                print(f"IndexError")
            try:
                paramsValues.append(float((valuesString[1].replace(",", ".").strip())))
            except IndexError:
                print(f"IndexError")
    return [paramsName, paramsValues]


def findFileLastModification(pathFile1="dataTumor/PredictData/PersonalPatients/", pathFile2="dataTumor/PredictData/Any/"):
    """
    Find File of the last modification

    Keywords argiments:
    pathFile1 -- path to directory file1
    pathFile2 -- path to directory file2

    Return:
    String pathFileLastModification
    """

    dataModificationFile1 = getmtime(join(dirname(__file__), pathFile1))
    dataModificationFile2 = getmtime(join(dirname(__file__), pathFile2))
    if dataModificationFile2 > dataModificationFile1:
        return pathFile2
    else:
        return pathFile1


def compareData(experimentalData, modelData):
    """
    Return comparable data between


    Positional arguments:
    experimentalData -- experimental data (Volume or Diameter)
    modelData -- ,odel data (predict)

    Return:
    Array[time-values, volumeCancer-values]
    """

    absoluteError = []
    relativeError = 0
    difference = []
    for i in range(len(experimentalData[0])):
        indexMin = 0
        difference = []
        difference.clear()
        for iLenModelData in range(len(modelData[0])):
            difference.append(abs(experimentalData[0][i] - modelData[0][iLenModelData]))
        indexMin = difference.index(min(difference))
        relativeError += abs(modelData[1][indexMin] - experimentalData[1][i]) / experimentalData[1][i]
        
    return relativeError / len(experimentalData[0])