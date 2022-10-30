from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import numpy as np
from pylab import *

from ActionDataFile import getDataFromFile
from ActionDataFile import getTimeValueFromFile

  
# get the new and old cancer dataset and plot
type = "Volume"
number = 1

pathOld = 1;
pathNew = f"../../../dataTumor/PredictData/Any/"
timeCancerNew = getTimeValueFromFile(type, number, path=pathNew)
timecancerOld = get
timeValuesNew = timeCancerNew[0]
cancerVolumeNew = timeCancerNew[1]

xyzc = getDataFromFile(type, number, path=pathNew)
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
fig.savefig(f"../../../dataTumor/PredictData/Any/{type}/img/{number}{type}.png")
# print(type(timeValuesNew))
# print(type(cancerVolumeNew))
# plt.show()

fig = plt.figure(figsize=(10, 10))
ax = fig.add_subplot(111)
plt.plot(timeValuesNew, cancerVolumeNew)
ax.set_title("Динамика опухоли")
ax.set_xlabel('время (месяцы)')
ax.set_ylabel('объем (мл)')
# plt.show()
fig.savefig(f"../../../dataTumor/PredictData/Any/{type}/timeValue/img/{number}{type}.png")
 