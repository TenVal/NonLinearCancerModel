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


if __name__ == "__main__":

    # get the cancer dataset and plot
    type = "Temperature"
    quantity = 10
    allTimeCancer = {"time" : [],
                     "temperature" : []
                     }

    for number in range(1, quantity + 1):
        # get Time Value Data from file
        pathGetTimeValue = f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/txt/{number}{type}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 
        allTimeCancer["time"].append(timeValues)
        allTimeCancer["temperature"].append(cancerValues)
        

        fig = plt.figure(figsize=(10, 10))
        ax = fig.add_subplot(111)

        fig.suptitle(f"Temperature dynamics of the patient's {number} tumor", fontsize=28)
        plt.xlabel('time (month)', fontsize=26)
        plt.ylabel('temperature', fontsize=26)
        plt.xticks(fontsize=24)
        plt.yticks(fontsize=24)
        plt.plot(timeValues, cancerValues)
        plt.grid(True)
        current_dir = dirname(__file__)
        pathSave = join(current_dir, f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/img/{number}{type}.png")
        fig.savefig(pathSave)
    plt.cla()
    plt.clf()
    plt.close()
