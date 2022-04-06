from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile
  
# get the cancer dataset and plot
type = "Volume"
quantity = 10
for number in range(quantity):
    timeCancer = getTimeValueFromFile(type, number)
    timeValue = timeCancer[0]
    cancerVolume = timeCancer[1]

    xyzc = getDataFromFile(type, number)
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
    ax.set_title("3D cancer map")
    ax.set_xlabel('X-axis')
    ax.set_ylabel('Y-axis')
    ax.set_zlabel('Z-axis')
  
    # saving plot
    fig.savefig(f"dataTumor/PredictData/PersonalPatients/{type}/img/{i}{type}.png")

    plt.scatter(timeValues, cancerVolume)
    plt.show()
    fig.savefig(f"dataTumor/PredictData/PersonalPatients/{type}/timeValue/img/{i}{type}.png")