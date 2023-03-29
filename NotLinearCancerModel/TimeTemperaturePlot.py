from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *
from os.path import dirname, join

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile
from ActionDataFile import getExperimentalDataFromFile
from ActionDataFile import writeAccuracyIntoFile
from ActionDataFile import compareData
from ActionDataFile import getParamsFromFile
from ActionDataFile import getSingleDataFromFile


if __name__ == "__main__":

    # get the cancer dataset and plot
    type = "Temperature"
    quantity = 10

    pathHeatDissipation = f"dataTumor/PredictData/Total/heatDissipation.txt"
    heatDissipationValues = getSingleDataFromFile(pathHeatDissipation)
    for number in range(1, quantity + 1):
        # get Time Value Data from file
        pathGetTimeValue = f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/txt/{number}{type}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 

        heatDissipationValues[number-1] = round(heatDissipationValues[number-1], 1)

        fig = plt.figure(figsize=(10, 10))
        ax = fig.add_subplot(111)

        fig.suptitle(f"Dynamics of the patient's {number} tumor", fontsize=28)
        plt.xlabel('time (month)', fontsize=26)
        plt.ylabel('Radius (mm)', fontsize=26)
        plt.xticks(fontsize=24)
        plt.yticks(fontsize=24)
        plt.plot(timeValues, cancerValues, label=f"Heat {heatDissipationValues[number-1]}")
        plt.grid(True)
        plt.legend(prop={"size":20})
        current_dir = dirname(__file__)
        pathSave = join(current_dir, f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/img/{number}{type}.png")
        fig.savefig(pathSave)
    plt.cla()
    plt.clf()
    plt.close()
