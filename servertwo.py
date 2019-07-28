
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


    ret, the = cv2.threshold(gray, 70, 255, cv2.THRESH_BINARY)

    contours,_ = cv2.findContours(the.copy(),cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)

    hull = [cv2.convexHull(c) for c in contours]
    final = cv2.drawContours(arr, hull, -1, (255,0,0))

    cv2.imshow('Originals', arr)
    cv2.imshow('Thresh',the)
    cv2.imshow('Convex hull',final)

    cv2.waitKey(0)
    cv2.destroyAllWindows()       

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
