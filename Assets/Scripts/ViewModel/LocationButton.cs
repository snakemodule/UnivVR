using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class LocationButton : UIView
{

    public Button innerButton;
    public NavigatorPopup navPopup;
    public bool navigationPopupEnabled;

    public void MouseIn()
    {
        if (navPopup.isEnabled)
        {
            navPopup.Activate();
            navPopup.OverrideSorting.Value = true;
        }
    }

    public void MouseOut()
    {
        if (navPopup.isEnabled)
        {
            navPopup.Deactivate();
        }
    }

    public void onClick()
    {
        navPopup.clickJumpTo();
    }

    public void setup(string label, ViewSwitcher viewSwitcher, VisibilityView visibilityView)
    {
        innerButton.Text.Value = label;
        navPopup.Offset.Value = new ElementMargin(this.innerButton.ActualWidth * 2, 0);
    }
}
