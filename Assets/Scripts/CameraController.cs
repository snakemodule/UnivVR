using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController {

    private CameraHandler camHandler;
    private CameraReference camRef;

    public CameraController(Transform tr, CameraReference camRef)
    {  
        this.camHandler = new CameraHandler(tr);
        this.camRef = camRef;
    }

    public void Input(SteamVR_Controller.Device Controller)
    {
        camHandler.handleInput(Controller);
    }

    public void enablePan()
    {
        camRef.showPan();
        camHandler.enablePan();
    }

    public void disablePan()
    {
        camRef.hidePan();
        camHandler.disablePan();
    }
}
