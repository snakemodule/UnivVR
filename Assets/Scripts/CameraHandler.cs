using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler {


    private const float padCenterDistance = 0.7f;
    private const float zeroMovementMarginal = 0.05f;
    private const float maxMovementDistance = 0.35f;


//    private GameObject widgetGO;
    private Widget widget;
    private UniviewConnection univiewConnection;

    private enum PadTouchState { none, middle, left, top, right, bottom };

    private PadTouchState currentPressState;

    private Vector3 startPosition;

    private bool panEnabled = false;

    public void enablePan()
    {
        panEnabled = true;
    }

    public void disablePan()
    {
        panEnabled = false;
    }

    private TransformWrapper controllerTransform;


    public CameraHandler(Transform tr)
    {
        this.controllerTransform = new TransformWrapper(tr);
        GameObject widgetGO = GameObject.Find("Widget");
        widget = widgetGO.GetComponent<Widget>();
        widget.hide();
        univiewConnection = UniviewConnection.Instance;
    }

    public void handleInput(SteamVR_Controller.Device Controller)
    {
        if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (Math.Abs(Controller.GetAxis().y) > padCenterDistance
            || Math.Abs(Controller.GetAxis().x) > padCenterDistance)
            {
                if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    checkEachDirection(Controller, true);
                }
                else
                {
                    checkEachDirection(Controller, false);
                }
            }
            else
            {
                handleState(PadTouchState.middle, false);
            }
        }
        else
        {
            handleState(PadTouchState.none, false);
        }
    }

    private void handleState(PadTouchState touch, bool pressed)
    {

        if(pressed)
        {
            switch (touch)
            {
                case PadTouchState.left:
                    widget.rollMode();
                    roll();
                    break;
                case PadTouchState.top:
                    widget.rotateMode();
                    rotate();
                    break;
                case PadTouchState.right:
                    widget.translateMode();
                    translate();
                    break;
                case PadTouchState.bottom:
                    if(panEnabled){
                        widget.panMode();
                        pan();
                    }else
                    {
                        widget.hide();
                        noInput();
                    }

                    break;
                default:
                    widget.hide();
                    noInput();
                    break;
            }
        } else
        {
            switch (touch)
            {
                case PadTouchState.left:
                    widget.rollMode();
                    break;
                case PadTouchState.top:
                    widget.rotateMode();
                    break;
                case PadTouchState.right:
                    widget.translateMode();
                    break;
                case PadTouchState.bottom:
                    if(panEnabled)
                        widget.panMode();
                    else
                        widget.hide();
                    break;
                default:
                    widget.hide();
                    break;
            }
            noInput();
        }
    }
    
    private void noInput()
    {
        widget.fixedPos = false;
        widget.setTransparent();
        currentPressState = PadTouchState.none;
        univiewConnection.sendCommand("camera.noinput\ncamera.translatemode false\n");
    }

    private void checkEachDirection(SteamVR_Controller.Device Controller, bool pressed)
    {
        if (Controller.GetAxis().y > padCenterDistance)
        {
            handleState(PadTouchState.top, pressed);
        }
        else if (Controller.GetAxis().y < -padCenterDistance)
        {
            handleState(PadTouchState.bottom, pressed);
        }
        else if (Controller.GetAxis().x > padCenterDistance)
        {
            handleState(PadTouchState.right, pressed);
        }
        else if (Controller.GetAxis().x < -padCenterDistance)
        {
            handleState(PadTouchState.left, pressed);
        }
    }

    private void rotate()
    {
        if (currentPressState != PadTouchState.top)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentPressState = PadTouchState.top;
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
    }

    private void translate()
    {

        if (currentPressState != PadTouchState.right)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentPressState = PadTouchState.right;
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
        if (currentPressState != PadTouchState.left)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
        }
        currentPressState = PadTouchState.left;
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
        if (currentPressState != PadTouchState.bottom)
        {
            startPosition = controllerTransform.getPosition();
            widget.fixedPos = true;
            univiewConnection.sendCommand("camera.translatemode true\n");
        }
        currentPressState = PadTouchState.bottom;
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

        univiewConnection.sendCommand("camera.rotate " + sidewaysPanSpeed + " " + forwardPanSpeed + " 0 0\n");
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


        if (pb > ab)
        {
            return 0;
        }
        if (pa > ab)
        {
            return 1;
        }
        return pa / ab;
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
