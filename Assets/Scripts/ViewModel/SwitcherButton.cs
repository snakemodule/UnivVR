using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class SwitcherButton : UIView {

    //private ViewSwitcher switcher;
    //private int switchIndex = -1;
    //public Group navigationPopup;
    public SwitchCallback callback;

    public Button innerButton;
    public NavigatorPopup navPopup;
    //private bool mousein = false;

    public void SwitchView() {
        callback();
    }

    public void MouseIn()
    {
        if(navPopup.isEnabled)
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

    private void updateLayout()
    {
        View temp = this.Parent;
        while (temp != null)
        {
            temp.LayoutChanged();
            temp = temp.Parent;
        }
    }

    //public void setup(String label, ViewSwitcher switcher, VisibilityView visibilityView)
    public void setup(String label, SwitchCallback switchCallback)
    {
        navPopup.Offset.Value = new ElementMargin(innerButton.ActualWidth * 2, 0);
        //this.switcher = switcher;
        this.callback = switchCallback;
        innerButton.Text.Value = label;
    }
}
