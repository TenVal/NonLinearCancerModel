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
    if len(sys.argv) > 1 and len(sys.argv) < 3:
        numberPatient = sys.argv[1]
    else:
        numberPatient = 0;
    # get the new and old cancer dataset and plot
    type = "Volume"

    pathNew = f"dataTumor/PredictData/Any/{type}/timeValue/txt/{numberPatient}{type}.txt"
    timeCancerNew = getTimeValueFromFile(path=pathNew)

    pathNew = f"dataTumor/PredictData/Any/{type}/txt/{numberPatient}{type}.txt"
    xyzc = getDataFromFile(path=pathNew)
    x = xyzc[0]
    y = xyzc[1]
    z = xyzc[2]
    c = xyzc[3]
  
    # creating figures
    fig = plt.figure(figsize=(15, 10))
    ax = fig.add_subplot(111, projection='3d')

    # creating the cancer map (heatmap)
    img = ax.scatter(x, y, z, c, marker='o')
    plt.colorbar(img)
  
    # adding title and labels
    #ax.set_title("3D cancer map")
    #ax.set_xlabel('X-axis (mm)')
    #ax.set_ylabel('Y-axis (mm)')
    #ax.set_zlabel('Z-axis (mm)')
    ax.set_title("3D моделирование опухоли")
    ax.set_xlabel('X (мм)')
    ax.set_ylabel('Y (мм)')
    ax.set_zlabel('Z (мм)')
  
    # saving plot
    current_dir = dirname(__file__)
    pathSave = join(current_dir, f"dataTumor/PredictData/Any/{type}/img/{numberPatient}{type}.png")
    fig.savefig(pathSave)
    # print(type(timeValuesNew))
    # print(type(cancerVolumeNew))
    # plt.show()

    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111)

    plt.plot(timeCancerNew[0], timeCancerNew[1], label="Новые результаты")


    pathOld = f"dataTumor/PredictData/Any/{type}/timeValue/txt/{numberPatient}{type}Old.txt"
    timecancerOldOne = getTimeValueFromFile(path=pathOld)
    plt.plot(timecancerOldOne[0], timecancerOldOne[1], color="#964b00", label="Предыдущие результаты")
    if numberPatient != 0:
        # get experimental data to put it on plot
        pathExperimentalData = f"dataTumor/ExperimentalData/{type}/{numberPatient}{type}.txt"
        experimentalData = getExperimentalDataFromFile(path=pathExperimentalData)
        plt.scatter(experimentalData[0], experimentalData[1], c="red", label=f"Клинические данные пацента-{numberPatient}")
    plt.legend()

    ax.set_title("Динамика опухоли")
    ax.set_xlabel('время (месяцы)')
    ax.set_ylabel('объем (мл)')
    plt.grid(True)
    # plt.show()
    current_dir = dirname(__file__)
    pathSave = join(current_dir, f"dataTumor/PredictData/Any/{type}/timeValue/img/{numberPatient}{type}.png")
    fig.savefig(pathSave)
