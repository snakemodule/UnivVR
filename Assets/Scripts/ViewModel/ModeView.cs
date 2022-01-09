using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;
using MarkLight.Views.UI;
using System.Text;
using System;

public class ModeView : View
{
    public Button ExploreModeButton;
    public Button TourModeButton;
    public UserInterface ModeUI;

    public GameObject TourViewObject;
    public GameObject LibraryViewObject;

    private SwitchCallback toLibrary;
    private SwitchCallback toTours;



    public void clickExplore()
    {
        toLibrary();
    }

    public void clickTours()
    {
        toTours();
    }

    void Update()
    {
        ModeUI.RectTransform.localScale = new Vector3(1, 1, 1);
        ModeUI.RectTransform.eulerAngles = new Vector3(0, 90, 0);
        ModeUI.RectTransform.position = new Vector3(3, 1, 0);
    }

    internal void setup(SwitchCallback switchToLibrary, SwitchCallback switchToTours)
    {
        toLibrary = switchToLibrary;
        toTours = switchToTours;
    }
}