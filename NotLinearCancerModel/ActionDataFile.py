import codecs
import locale
from os.path import dirname, join
from os.path import getctime, getmtime
from datetime import datetime as dt

locale.setlocale(locale.LC_ALL, 'en_US')


def writeTimeValueIntoFile(type, number, timeValue, path = "dataTumor/PredictData/PersonalPatients/"):
    path = path + str(type) + "/timeValue/txt/" + str(number) + str(type) + ".txt"
    current_dir = dirname(__file__)
    path = join(current_dir, path)

    with open(path, "w") as file:
        for i in range(len(timeValue[0])):
            file.write(f"{timeValue[0][i]}\t{timeValue[1][i]}\n".encode('utf-8').decode('utf-8'))


def writeDataIntoFile(type, number, xyzc, path = "dataTumor/PredictData/PersonalPatients/"):
    path = path + str(type) + "/txt/" + str(number) + str(type) + ".txt"
    current_dir = dirname(__file__)
    path = join(current_dir, path)

    with open(path, "w") as file:
        for i in range(len(xyzc[0])):
            file.write(f"{xyzc[0][i]}\t{xyzc[1][i]}\t{xyzc[2][i]}\t{xyzc[3][i]}\n".encode('utf-8').decode('utf-8'))


def writeAccuracyIntoFile(type, number, relativeError, path="dataTumor/PredictData/PersonalPatients/"):
    
    path = path + str(type) + "/txt/params/" + str(number) + "Params" + ".txt"
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
    return "Ok"


def getDataFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "dataTumor/PredictData/PersonalPatients/"):
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

    writeDataIntoFile(type, number, [valuesX2, valuesY2, valuesZ2, valuesC2])
    return [valuesX2, valuesY2, valuesZ2, valuesC2]


def getTimeValueFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "dataTumor/PredictData/PersonalPatients/"):
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

    #current_dir = dirname(__file__)
    #file_path = join(current_dir, "dataTumor/PredictData/PersonalPatients/file.txt")
    #with open(file_path, 'w') as f:
    #    f.write(file_path)
    #path1 = path + "file.txt"
    #path1 = "dataTumor/file.txt"
    #with open(path1, "r") as file:
    #    pass    
    
    path = path + str(type) + "/timeValue/txt/" + str(number) + str(type) + ".txt"
    current_dir = dirname(__file__)
    path = join(current_dir, path)
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


def getExperimentalDataFromFile(type, number, stepX=10, stepY=10, stepZ=10, path = "dataTumor/ExperimentalData/"):
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
    current_dir = dirname(__file__)
    path = join(current_dir, path)

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

def getParamsFromFile(type, stepX=10, stepY=10, stepZ=10, path="dataTumor/PredictData/PersonalPatients/"):
    """
    Get params data about patient from file


    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    Array[paramsName, paramsValues]
    """
    path = path + str(type) + "/txt/params/" + str(number) + "Params" + ".txt"
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
                # valuesCancer.append(stepX * stepX * stepX * float(valuesString[1].replace(",", ".")))
                paramsValues.append(float((valuesString[1].replace(",", ".").strip())))
            except IndexError:
                print(f"IndexError")
    return [paramsName, paramsvalues]


def findFileLastModification(type, number, pathFile1="dataTumor/PredictData/PersonalPatients/", pathFile2="dataTumor/PredictData/Any/"):
    """
    Find File of the last modification

    Positional arguments:
    type -- data type (Volume or Diameter)
    number -- patient number

    Keywords argiments:
    stepX -- X-axis steep
    stepY -- Y-axis steep
    stepZ -- Z-axis steep
    path -- path to directory file

    Return:
    String pathFileLastModification
    """
    pathFile1 = pathFile1 + str(type) + "/timeValue/txt/" + str(number) + str(type) + ".txt"
    pathFile2 = pathFile2 + str(type) + "/timeValue/txt/" + str(number) + str(type) + "Old.txt"
    dataModoficationFile1 = os.path.getmtime(pathFile1)
    dataModificationFile2 = os.path.getmtime(pathFile2)
    if dataModificationFile2 > dataModificationFile1:
        return pathFile2
    else:
        return pathFile1

def compareData(experimentalData, modelData):
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

    # print("\n{0}\t{1}\n".format(len(experimentalData[0]), len(modelData[0])))
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
        
        # print("index:{0}\tmodelX:{1}\texpX:{2}\tmodelY:{1}\texpY:{2}\n".format(indexMin, modelData[0][indexMin], experimentalData[0][i], modelData[1][indexMin], experimentalData[1][i]))
    return relativeError / len(experimentalData[0])