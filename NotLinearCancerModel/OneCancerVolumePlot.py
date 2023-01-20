from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *
from os.path import dirname, join
import sys

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile
from ActionDataFile import getExperimentalDataFromFile
from ActionDataFile import writeAccuracyIntoFile
from ActionDataFile import compareData
from ActionDataFile import getParamsFromFile
from ActionDataFile import findFileLastModification
 
if __name__ == "__main__":

    numberPatient = int(sys.argv[1].strip())
    type = str(sys.argv[2]).strip()

    # get new Time Value Data from file
    pathNewTimeValue = f"dataTumor/PredictData/Any/{type}/timeValue/txt/{numberPatient}{type}.txt"
    timeCancerNew = getTimeValueFromFile(path=pathNewTimeValue)

    # get new Data from file
    pathNewData = f"dataTumor/PredictData/Any/{type}/txt/{numberPatient}{type}.txt"
    # get stepX -- H to get Data
    pathGetParams = f"dataTumor/PredictData/Any/{type}/txt/params/{numberPatient}Params.txt"
    params = getParamsFromFile(pathGetParams)
    stepX = params[1][params[0].index('H')]
    pathNewData = f"dataTumor/PredictData/Any/{type}/txt/{numberPatient}{type}.txt"
    # get Data from file
    xyzc = getDataFromFile(stepX=stepX, stepY=stepX, stepZ=stepX, path=pathNewData)
    x = xyzc[0]
    y = xyzc[1]
    z = xyzc[2]
    c = xyzc[3]
  
    # creating figures
    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111, projection='3d')

    # creating the cancer map (heatmap)
    img = ax.scatter(x, y, z, c, marker='o')
    plt.colorbar(img)
  
    # adding title and labels
    title3D = f"3D model cancer {numberPatient}"
    if numberPatient == 0:
        title3D = f"3D model cancer"
    ax.set_title(title3D, fontsize=28)
    ax.set_xlabel('X (mkm)', fontsize=18)
    ax.set_ylabel('Y (mkm)', fontsize=18)
    ax.set_zlabel('Z (mkm)', fontsize=18)
    ax.tick_params(axis='both', which='major', labelsize=14)
    ax.tick_params(axis='both', which='minor', labelsize=14)
  
    # saving plot
    current_dir = dirname(__file__)
    pathSave = join(current_dir, f"dataTumor/PredictData/Any/{type}/img/{numberPatient}{type}.png")
    fig.savefig(pathSave)

    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111)
    
    plt.plot(timeCancerNew[0], timeCancerNew[1], label="New results")

    pathToLatestModificationFile = f"dataTumor/PredictData/Any/{type}/timeValue/txt/{numberPatient}{type}Old.txt"
    if numberPatient != 0:
        # find latest data
        pathOld1 = f"dataTumor/PredictData/Any/{type}/timeValue/txt/{numberPatient}{type}Old.txt"
        pathOld2 = f"dataTumor/PredictData/PersonalPatients/{type}/timeValue/txt/{numberPatient}{type}.txt"
        pathToLatestModificationFile = findFileLastModification(pathOld1, pathOld2) 
   
        # get experimental data to put it on plot
        pathExperimentalData = f"dataTumor/ExperimentalData/{type}/{numberPatient}{type}.txt"
        experimentalData = getExperimentalDataFromFile(path=pathExperimentalData)
        plt.scatter(experimentalData[0], experimentalData[1], c="red", label=f"Clinical patient-{numberPatient} data")

    pathToLatestModificationFile = join(current_dir, pathToLatestModificationFile)
    timecancerOldOne = getTimeValueFromFile(path=pathToLatestModificationFile)    
    plt.plot(timecancerOldOne[0], timecancerOldOne[1], color="#964b00", label="Previous results")

    # title for plot time value
    titleTimeValue = f"Tumor dynamics of the patient-{numberPatient}"
    if numberPatient == 0:
        titleTimeValue = f"Tumor dynamics"
    fig.suptitle(titleTimeValue, fontsize=28)
    plt.xlabel('время (месяцы)', fontsize=26)
    plt.ylabel('объем (мл)', fontsize=26)
    plt.xticks(fontsize=24)
    plt.yticks(fontsize=24)
    plt.legend(fontsize=16)
    plt.grid(True)

    pathSave = join(current_dir, f"dataTumor/PredictData/Any/{type}/timeValue/img/{numberPatient}{type}.png")
    fig.savefig(pathSave)
