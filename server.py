from flask import Flask
from flask import request
import io
import os
from array import array
import cv2
import numpy as np
from PIL import Image


app = Flask(__name__)
app.config['DEBUG'] = False

count = 0





@app.route('/', methods=['GET', 'PUT'])
def hello_world():
    
    global count 
    count += 1

    width = int(request.headers.get("width"))
    height = int(request.headers.get("height"))
    datasize = len(request.data)

    print("Data size: " + str(datasize))
    print("Width: " + str(width))
    print("Height: " + str(height))

    if datasize == 0:
        return "Nada"

    arr = np.frombuffer(request.data, dtype = np.dtype('b'))
    arr = np.reshape(arr, (height,width,4))
    arr = arr.astype(np.float32)

    gray = cv2.cvtColor(arr, cv2.COLOR_BGR2GRAY)
    gray = gray.astype(np.uint8)

    imggg = Image.fromarray(gray, 'L')
    imggg.show(title="Grayed")

    blur = cv2.GaussianBlur(gray,(5,5),0)

    imgg = Image.fromarray(blur, 'L')
    imgg.show(title="Blurred")

    ret,thresh1 = cv2.threshold(blur,70,255,cv2.THRESH_BINARY_INV+cv2.THRESH_OTSU)

    img = Image.fromarray(thresh1, 'L')
    img.show(title="Thresheld")


    contours, hierarchy = cv2.findContours(thresh1,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)
    drawing = np.zeros(arr.shape,np.uint8)
    drawing[:,:,3] = 255

    if len(contours) == 0:
        return "No contours!"

    max_area=0
    ci = 0
   
    for i in range(len(contours)):
            cnt=contours[i]
            area = cv2.contourArea(cnt)
            if(area>max_area):
                max_area=area
                ci=i


    cnt=contours[ci]
    hull = cv2.convexHull(cnt)
    moments = cv2.moments(cnt)

    if moments['m00']!=0:
                cx = int(moments['m10']/moments['m00']) # cx = M10/M00
                cy = int(moments['m01']/moments['m00']) # cy = M01/M00
              
    centr=(cx,cy)   

    cv2.circle(arr,centr,5,[0,0,255],2)       
    cv2.drawContours(drawing,[cnt],0,(0,255,0),2) 
    cv2.drawContours(drawing,[hull],0,(0,0,255),2) 
          
    cnt = cv2.approxPolyDP(cnt,0.01*cv2.arcLength(cnt,True),True)
    hull = cv2.convexHull(cnt,returnPoints = False)

    numdefects = 0
    defects = cv2.convexityDefects(cnt,hull)
    
    if defects is not None:
               
        numdefects = defects.shape[0]

        mind=0
        maxd=0
        for i in range(defects.shape[0]):

            s,e,f,d = defects[i,0]
            start = tuple(cnt[s][0])
            end = tuple(cnt[e][0])
            far = tuple(cnt[f][0])
            dist = cv2.pointPolygonTest(cnt,centr,True)

            cv2.line(arr,start,end,[0,255,0],2)
            cv2.circle(arr,far,5,[0,0,255],-1)

        img = Image.fromarray(drawing, 'RGBA')
        img.show(title="Final")


            

    print("Found " + str(numdefects) + " defects ")            
    return 'Hello, World!' + str(count)

if __name__ == '__main__':
    #app.run(host="0.0.0.0", port=8080)
    app.run()




def isqrt(n):
    x = n
    y = (x + 1) // 2
    while y < x:
        x = y
        y = (x + n // x) // 2
    return x
