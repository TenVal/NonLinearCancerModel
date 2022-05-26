from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile
from ActionDataFile import getExperimentalDataFromFile;

  
# temporarily absolute paths of files!!!!!!!!!!!!!!!!!!!!!
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
    timeCancer = getTimeValueFromFile(type, number)
    timeValues = timeCancer[0]
    cancerValues = timeCancer[1] 
    allTimeCancer["time"].append(timeValues)
    allTimeCancer["cancer"].append(cancerValues)

    experimentalData = getExperimentalDataFromFile(type, number)
    experimentalTimeValues = experimentalData[0]
    experimentalCancerValues = experimentalData[1]
    allExperimentalTimeCancer["time"].append(experimentalTimeValues)
    allExperimentalTimeCancer["cancer"].append(experimentalCancerValues)

    xyzc = getDataFromFile(type, number)
    x = xyzc[0]
    y = xyzc[1]
    z = xyzc[2]
    c = xyzc[3]
  
    # creating figures
    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111, projection='3d')
    if number==10:
        print(f"{number}\n\n")
    # for i in range(len(x)):
    #     print(f"{x[i]}\t{y[i]}\t{z[i]}\t{c[i]}")

    # creating the cancer map (heatmap)
    img = ax.scatter(x, y, z, c, marker='o')
    plt.colorbar(img)
  
    # adding title and labels
    ax.set_title("3D cancer map")
    ax.set_xlabel('X-axis (mm)')
    ax.set_ylabel('Y-axis (mm)')
    ax.set_zlabel('Z-axis (mm)')
  
    # saving plot
    fig.savefig(f"../../../dataTumor/PredictData/PersonalPatients/{type}/img/{number}{type}.png")
    # print(type(timeValues))
    # print(type(cancerVolume))
    # plt.show()

    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111)
    plt.plot(timeValues, cancerValues)
    plt.scatter(experimentalTimeValues, experimentalCancerValues, c = "red")
    ax.set_title(f"{number}-patient Time-Volume Dinamic")
    # ax.set_xlabel('time (days)')
    ax.set_xlabel('time (month)')
    ax.set_ylabel('volume (mL)')
    # plt.show()
    fig.savefig(f"../../../dataTumor/PredictData/PersonalPatients/{type}/timeValue/img/{number}{type}.png")
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
             label=f"Simulated data patient {i}")
    plt.plot(allExperimentalTimeCancer["time"][i], 
             allExperimentalTimeCancer["cancer"][i], 
             color = colors[i],
             linestyle = ":", 
             label=f"Experimental data patient {i}")

plt.legend()
ax.set_title(f"Time-Volume Dinamic for everu patient")
# ax.set_xlabel('time (days)')
ax.set_xlabel('time (month)')
ax.set_ylabel('volume (mL)')
# plt.show()
fig.savefig(f"../../../dataTumor/PredictData/Total/Volume/img/All.png")
plt.cla()
plt.clf()
plt.close()