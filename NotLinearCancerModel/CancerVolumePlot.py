from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile

  
# temporarily absolute paths of files!!!!!!!!!!!!!!!!!!!!!
# get the cancer dataset and plot
type = "Volume"
quantity = 10
for number in range(1, quantity + 1):
    timeCancer = getTimeValueFromFile(type, number)
    timeValues = timeCancer[0]
    cancerVolume = timeCancer[1]

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
    plt.plot(timeValues, cancerVolume)
    ax.set_title(f"{number}-patient Time-Volume Dinamic")
    ax.set_xlabel('time (days)')
    ax.set_ylabel('volume (mm^2)')
    # plt.show()
    fig.savefig(f"../../../dataTumor/PredictData/PersonalPatients/{type}/timeValue/img/{number}{type}.png")