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

if __name__ == "__main__":
    # get the cancer dataset and plot
    type = "Volume"
    quantity = 10
    allTimeCancer = {"time" : [],
                     "cancer" : []
                     }
    allExperimentalTimeCancer = {"time" : [],
                     "cancer" : []
                     }

    for number in range(1, quantity + 1):
        pathGetTimeValue = f"dataTumor/PredictData/PersonalPatients/{type}/timeValue/txt/{number}{type}.txt"
        timeCancer = getTimeValueFromFile(path=pathGetTimeValue)
        timeValues = timeCancer[0]
        cancerValues = timeCancer[1] 
        allTimeCancer["time"].append(timeValues)
        allTimeCancer["cancer"].append(cancerValues)

        pathGetExperimental = f"dataTumor/ExperimentalData/{type}/{number}{type}.txt"
        experimentalData = getExperimentalDataFromFile(path=pathGetExperimental)
        experimentalTimeValues = experimentalData[0]
        experimentalCancerValues = experimentalData[1]
        allExperimentalTimeCancer["time"].append(experimentalTimeValues)
        allExperimentalTimeCancer["cancer"].append(experimentalCancerValues)


        pathGetData = f"dataTumor/PredictData/PersonalPatients/{type}/txt/{number}{type}.txt"
        xyzc = getDataFromFile(path=pathGetData)
        x = xyzc[0]
        y = xyzc[1]
        z = xyzc[2]
        c = xyzc[3]

        # calculate relative Error
        relativeError = compareData(experimentalData, timeCancer)
        pathAccuracy = f"dataTumor/PredictData/PersonalPatients/{type}/txt/params/{number}Params.txt"
        writeAccuracyIntoFile(relativeError, path=pathAccuracy)
  
        # creating figures
        fig = plt.figure(figsize=(10, 10))
        ax = fig.add_subplot(111, projection='3d')
        if number==10:
            print(f"{number}\n\n")

        # creating the cancer map (heatmap)
        img = ax.scatter(x, y, z, c, marker='o')
        plt.colorbar(img)
  
        # adding title and labels
        ax.set_title("3D моделирование опухоли")
        ax.set_xlabel('X (мм)')
        ax.set_ylabel('Y (мм)')
        ax.set_zlabel('Z (мм)')
  
        # saving plot
        fig.savefig(f"dataTumor/PredictData/PersonalPatients/{type}/img/{number}{type}.png")
        # print(type(timeValues))
        # print(type(cancerVolume))
        # plt.show()

        fig = plt.figure(figsize=(10, 10))
        ax = fig.add_subplot(111)

        #fig.suptitle(f"{number}-patient Time-Volume Dinamic", fontsize=28)
        fig.suptitle(f"Динамика опухоли пациента {number}", fontsize=28)
        #plt.xlabel('time (month)', fontsize=22)
        plt.xlabel('время (месяцы)', fontsize=26)
        #plt.ylabel('volume (mL)', fontsize=26)
        plt.ylabel('объем (мл)', fontsize=26)
        plt.xticks(fontsize=24)
        plt.yticks(fontsize=24)
        plt.plot(timeValues, cancerValues)
        plt.scatter(experimentalTimeValues, experimentalCancerValues, c = "red")  
        # plt.show()
        current_dir = dirname(__file__)
        pathSave = join(current_dir, f"dataTumor/PredictData/PersonalPatients/{type}/timeValue/img/{number}{type}.png")
        fig.savefig(pathSave)
    plt.cla()
    plt.clf()
    plt.close()

    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111)
    colors = ["blue", "orange", "green", "red", "purple", "brown", "pink", "gray", "tan", "cyan"]
    for i in range(10):
        #print(allTimeCancer["time"][i])
        plt.plot(allTimeCancer["time"][i], 
                 allTimeCancer["cancer"][i], 
                 color = colors[i],
                 linestyle = "-", 
                 #label=f"Simulated data patient {i+1}")
                 label=f"Смоделированные данные пацента-{i+1}")
        plt.plot(allExperimentalTimeCancer["time"][i], 
                 allExperimentalTimeCancer["cancer"][i], 
                 color = colors[i],
                 linestyle = "--",
                 #label=f"Experimental data patient {i+1}")
                 label=f"Клинические данные пацента-{i+1}")

    plt.legend()
    #fig.suptitle(f"Time-Volume Dinamic for every patient", fontsize=24)
    fig.suptitle(f"Динамика опухоли всех пациентов", fontsize=24)
    #plt.xlabel('time (month)', fontsize=22)
    #plt.xlabel('time (days)', fontsize=22)
    plt.xlabel('время (месяцы)', fontsize=22)
    #plt.ylabel('volume (mL)', fontsize=22)
    plt.ylabel('объем (мл)', fontsize=22)
    plt.xticks(fontsize=20)
    plt.yticks(fontsize=20)
    # plt.show()
    plt.grid(True)
    current_dir = dirname(__file__)
    pathSave = join(current_dir, f"dataTumor/PredictData/Total/Volume/img/All.png")
    fig.savefig(pathSave)
    plt.cla()
    plt.clf()
    plt.close()