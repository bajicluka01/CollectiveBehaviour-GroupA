import cv2 as cv
import numpy as np
#import imutils

outputFolder = "output"

def displayImage(im, windowSize = [909, 433]):
    #resize image to fit the window
    #(h, w) = im.shape[:2]
    #r = windowSize[0] / float(w)
    #dim = (windowSize[1], int(h * r))
    #im = cv.resize(im, dim)
    #im = cv.resize(im, (windowSize[0], windowSize[1]), interpolation=cv.INTER_AREA)
    #im = imutils.resize(im, height=windowSize[1])

    cv.namedWindow("resized_window", cv.WINDOW_NORMAL | cv.WINDOW_KEEPRATIO) 
    cv.resizeWindow("resized_window", windowSize[0], windowSize[1])
    cv.imshow("resized_window", im)
    k = cv.waitKey(0) # Wait for a keystroke in the window
    if k == 's':
        return

def writeImage(im, filename):
    cv.imwrite(filename, im)


def thr(im):
    #image=im.copy()
    #im = cv.cvtColor(image, cv.COLOR_BGR2GRAY)
    #thresh, im_bw = cv.threshold(im, 127, 255, cv.THRESH_BINARY)
    #ret, thresh = cv.threshold(im, 127, 255, 0)
    #find contours
    #contours, hierarchy = cv.findContours(thresh, cv.RETR_TREE, cv.CHAIN_APPROX_SIMPLE)
    #image = cv.drawContours(image, contours, -1, (0,255,0), 10)

    print(im.shape[0], im.shape[1])
    for i in range(0, im.shape[0]):
        for j in range(0, im.shape[1]):
            if im[i][j] >= 227 and im[i][j] < 243:
            #if im[i][j] == 226 or im[i][j] == 223:
            #if i%10!=0:
                im[i][j] = 255
            else:
                im[i][j] = 0
    

    return im



img = cv.imread("new.png")
im = cv.cvtColor(img, cv.COLOR_BGR2GRAY)
#writeImage(im)
#im = thr(im)
#i = cv.bitwise_not(im)
#displayImage(img)
#img = findPoints(img)
#displayImage(i)
#writeImage(im)

im = cv.GaussianBlur(im, (7,7), 0)
im = thr(im)

#mask = np.zeros((im.shape[0]+2, im.shape[1]+2), dtype=np.uint8)
#holes = cv.floodFill(im.copy(), mask, (0, 0), 255)[1]
#holes = ~holes
#im[holes == 255] = 255



#writeImage(im, "testbig.png")

cv.imshow('image', im)
k = cv.waitKey(0)
