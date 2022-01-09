using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

public class NavigatorPopup : UICanvas { //UIView

    UniviewConnection connection;

    public VisibilityView visibilityView;

    public Button flytoButton;
    public Button jumptoButton;
    public Button targetButton;
    public Button surfaceButton;

    private String flytoCommand;
    private String jumptoCommand;
    private String targetCommand;
    private String surfaceCommand;

    private String infoText;
    private AudioClip infoClip;

    private CameraController CamController;

    public bool isEnabled { get; private set; }

    public List<String> buttonVisIDs;

    public override void Initialize()
    {
        base.Initialize();
        connection = UniviewConnection.Instance;
        
    }

    void Start()
    {
        CamController = GameObject.Find("Controller (right)").GetComponent<ControllerInputRight>().CamController;
    }

    public void clickFlyTo()
    {
        updatevisibilityView();
        StartCoroutine(connection.sendCommandInterruptInterpolation(flytoCommand));
        CamController.disablePan();
    }

    public void clickJumpTo()
    {
        updatevisibilityView();
        StartCoroutine(connection.sendCommandInterruptInterpolation(jumptoCommand));
        CamController.disablePan();
    }

    public void clickTarget()
    {
        updatevisibilityView();
        StartCoroutine(connection.sendCommandInterruptInterpolation(targetCommand));
        CamController.disablePan();
    }

    public void clickSurface()
    {
        updatevisibilityView();
        StartCoroutine(connection.sendCommandInterruptInterpolation(surfaceCommand));
        CamController.enablePan();
    }

    public void updatevisibilityView()
    {
        visibilityView.updatePanes(buttonVisIDs);
        visibilityView.presentInfo(infoText, infoClip);
    }


    public void setup(string flyto, string jumpto, string target, string surface, List<String> visibilityIDs, bool enabled, VisibilityView visibilityView, AudioClip infoClip, String infoText)
    {
        this.isEnabled = enabled;
        this.flytoCommand = flyto;
        this.jumptoCommand = jumpto;
        this.targetCommand = target;
        this.surfaceCommand = surface;
        this.buttonVisIDs = visibilityIDs;
        this.visibilityView = visibilityView;
        this.infoClip = infoClip;
        this.infoText = infoText;
    }
}
