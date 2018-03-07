using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is another comment!
public class CameraController {

    private const float padCenterDistance = 0.7f;
    private const float zeroMovementMarginal = 0.05f;
    private const float maxMovementDistance = 0.35f;

    private GameObject widgetGO;
    private Widget widget;
    private UniviewConnection univiewConnection;

    private TransformWrapper controllerTransform;

    private Vector3 startPosition;

    enum mode { none, rotate, translate, roll, pan };
    mode currentMode = mode.none;
    

    public CameraController(Transform tr)
    {
        this.widgetGO = GameObject.Find("Widget");
        widget = widgetGO.GetComponent<Widget>();
        this.controllerTransform = new TransformWrapper(tr);
        univiewConnection = UniviewConnection.Instance; 
    }
    
    public void init()
    {
        widgetGO.SetActive(false);
    }

    public void Input(SteamVR_Controller.Device Controller)
    {
        if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
            if (Math.Abs(Controller.GetAxis().y) > padCenterDistance
            || Math.Abs(Controller.GetAxis().x) > padCenterDistance) {
                widgetGO.SetActive(true);
                checkAndDoForEachDirection(Controller, widget.rotateMode, widget.panMode, widget.translateMode, widget.rollMode);
                if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)){
                    checkAndDoForEachDirection(Controller, rotate, pan, translate, roll);
                }
                else {
                    currentMode = mode.none;
                    widget.fixedPos = false;
                    widget.offColorLeft();
                    widget.offColorRight();
                    widget.offColorUp();
                    widget.offColorDown();
                    widget.offColorForward();
                    widget.offColorBackward();
                    univiewConnection.sendCommand("camera.noinput\ncamera.translatemode false\n");
                }
            }
            else {
                reset();
            }   
        }
        else
        {
            reset();
        }
    }


    private void rotate()
    {
        if (currentMode != mode.rotate)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentMode = mode.rotate;
        float speedX = horizontalSpeed();
        float speedY = verticalSpeed();
        if (speedX > 0)
            widget.highlightRight();
        else
            widget.offColorRight();
        if (speedX < 0)
            widget.highlightLeft();
        else
            widget.offColorLeft();
        if (speedY < 0)
            widget.highlightDown();
        else
            widget.offColorDown();
        if (speedY > 0)
            widget.highlightUp();
        else
            widget.offColorUp();

        univiewConnection.sendCommand("camera.rotate " + speedX + " " + speedY + " 0 0\n");
        //univiewConnection.sendCommand("camera.rotate 1.0 0 0\n");
    }

    private void translate()
    {
        
        if (currentMode != mode.translate)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentMode = mode.translate;
        float speedTranslate = forwardSpeed();
        if (speedTranslate < 0)
            widget.highlightBackward();
        else
            widget.offColorBackward();

        if (speedTranslate > 0)
            widget.highlightForward();
        else
            widget.offColorForward();

        univiewConnection.sendCommand("camera.translate 0 " + speedTranslate + " 0\n");
    }

    private void roll()
    {
        if (currentMode != mode.roll)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentMode = mode.roll;
        float speedRoll = horizontalSpeed();
        if (speedRoll > 0)
            widget.highlightRight();
        else
            widget.offColorRight();

        if (speedRoll < 0)
            widget.highlightLeft();
        else
            widget.offColorLeft();

        univiewConnection.sendCommand("camera.rotate 0 0 " + speedRoll + " 0\n");
    }

    private void pan()
    {
        if (currentMode != mode.pan)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
            univiewConnection.sendCommand("camera.translatemode true\n");
        }
        currentMode = mode.pan;
        float forwardPanSpeed = forwardSpeed();
        float sidewaysPanSpeed = horizontalSpeed();
        if (sidewaysPanSpeed > 0)
            widget.highlightRight();
        else
            widget.offColorRight();
        if (sidewaysPanSpeed < 0)
            widget.highlightLeft();
        else
            widget.offColorLeft();
        if (forwardPanSpeed < 0)
            widget.highlightBackward();
        else
            widget.offColorBackward();
        if (forwardPanSpeed > 0)
            widget.highlightForward();
        else
            widget.offColorForward();

        univiewConnection.sendCommand("camera.rotate " + sidewaysPanSpeed  + " " + forwardPanSpeed + " 0 0\n");
    }

    private void checkAndDoForEachDirection(SteamVR_Controller.Device Controller, 
        Action ypos, Action yneg, Action xpos, Action xneg)
    {
        if (Controller.GetAxis().y > padCenterDistance)
        {
            ypos();
        }
        else if (Controller.GetAxis().y < -padCenterDistance)
        {
            yneg();
        }
        else if (Controller.GetAxis().x > padCenterDistance)
        {
            xpos();
        }
        else if (Controller.GetAxis().x < -padCenterDistance)
        {
            xneg();
        }
    }

    //???????????
    /// <summary>
    /// backwards linear interpolation, i.e.how far along are we between these two points? 
    /// <param mask="mask">Decides what axes to consider</param> 
    /// </summary>
    private float backLerp(Vector3 p, Vector3 a, Vector3 b, Vector3 mask)
    {
        a = vectorMask(a, mask);
        b = vectorMask(b, mask);
        p = vectorMask(p, mask);
        float ab = Vector3.Distance(a, b);
        float pb = Vector3.Distance(p, b);
        float pa = Vector3.Distance(p, a);

        
        if (pb>ab)
        {
            return 0;
        }
        if (pa>ab)
        {
            return 1;
        }
        return  pa / ab;
    }

    private Vector3 vectorMask(Vector3 v, Vector3 mask)
    {
        if (!(mask.x == 0 || mask.x == 1) ||
            !(mask.y == 0 || mask.y == 1) ||
            !(mask.z == 0 || mask.z == 1))
        {
            throw new System.ArgumentException("Mask vector has values other than 0 or 1");
        }
        return new Vector3(v.x * mask.x, v.y * mask.y, v.z * mask.z);
    }

    private void reset()
    {
        currentMode = mode.none;
        widget.fixedPos = false;
        widgetGO.SetActive(false);
        widget.offColorLeft();
        widget.offColorRight();
        widget.offColorUp();
        widget.offColorDown();
        widget.offColorForward();
        widget.offColorBackward();
        univiewConnection.sendCommand("camera.noinput\ncamera.translatemode false\n");
    }

    private float horizontalSpeed()
    {
        Vector3 a1, b1, a2, b2;
        a1 = b1 = a2 = b2 = startPosition;
        a1.z -= zeroMovementMarginal;
        b1.z -= maxMovementDistance;
        a2.z += zeroMovementMarginal;
        b2.z += maxMovementDistance;
        float speed1 = backLerp(controllerTransform.getPosition(), a1, b1, new Vector3(0, 0, 1));
        float speed2 = backLerp(controllerTransform.getPosition(), a2, b2, new Vector3(0, 0, 1));
        return speed1 - speed2;
    }

    private float verticalSpeed()
    {
        Vector3 a1, b1, a2, b2;
        a1 = b1 = a2 = b2 = startPosition;
        a1.y += zeroMovementMarginal;
        b1.y += maxMovementDistance;
        a2.y -= zeroMovementMarginal;
        b2.y -= maxMovementDistance;
        float speed1 = backLerp(controllerTransform.getPosition(), a1, b1, new Vector3(0, 1, 0));
        float speed2 = backLerp(controllerTransform.getPosition(), a2, b2, new Vector3(0, 1, 0));
        return speed1 - speed2;
    }

    private float forwardSpeed()
    {
        Vector3 a1, b1, a2, b2;
        a1 = b1 = a2 = b2 = startPosition;
        a1.x += zeroMovementMarginal;
        b1.x += maxMovementDistance;
        a2.x -= zeroMovementMarginal;
        b2.x -= maxMovementDistance;
        float speed1 = backLerp(controllerTransform.getPosition(), a1, b1, new Vector3(1, 0, 0));
        float speed2 = backLerp(controllerTransform.getPosition(), a2, b2, new Vector3(1, 0, 0));
        return speed1 - speed2;
    }

}
