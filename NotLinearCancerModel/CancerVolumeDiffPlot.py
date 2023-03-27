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
    type1 = "VolumeLin"
    type2 = "Volume"
    quantity = 10
    allTimeCancer = {"timeLin" : [],
                     "volumeLin" : [],
                     "timeNonLin" : [],
                     "volumeNonLin" : [],
                     "timeEx" : [],
                     "volumeEx" : []
                     }

    for number in range(1, quantity + 1):
        # get Time Value Data from file
        pathGetTimeValue = f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/txt/{number}{type1}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 
        allTimeCancer["timeNonLin"].append(timeValues)
        allTimeCancer["volumeNonLin"].append(cancerValues)
        pathGetTimeValue = f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/txt/{number}{type2}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 
        allTimeCancer["timeLin"].append(timeValues)
        allTimeCancer["volumeLin"].append(cancerValues)
        pathGetTimeValue = f"dataTumor/ExperimentalData/{type}/{number}{type}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 
        allTimeCancer["timeEx"].append(timeValues)
        allTimeCancer["volumeEx"].append(cancerValues)

        fig = plt.figure(figsize=(10, 10))
        ax = fig.add_subplot(111)

        fig.suptitle(f"Dynamics of the patient's {number} tumor", fontsize=28)
        plt.xlabel('time (month)', fontsize=26)
        plt.ylabel('volume (mL)', fontsize=26)
        plt.xticks(fontsize=24)
        plt.yticks(fontsize=24)
        plt.plot(allTimeCancer["timeNonLin"][number-1], allTimeCancer["volumeNonLin"][number-1], c = "blue")
        plt.plot(allTimeCancer["timeLin"][number-1], allTimeCancer["volumeLin"][number-1], "-.", c = "orange")
        plt.scatter(allTimeCancer["timeEx"][number-1], allTimeCancer["volumeEx"][number-1], c = "red")
        plt.grid(True)
        plt.legend()
        current_dir = dirname(__file__)
        pathSave = join(current_dir, f"dataTumor/PredictData/PersonalPatients/Volume/timeValue/img/{number}{type1}.png")
        fig.savefig(pathSave)
    plt.cla()
    plt.clf()
    plt.close()
