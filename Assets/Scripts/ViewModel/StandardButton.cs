using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;

public class StandardButton : Button
{

    public int switchIndex = -1;
    public ViewSwitcher Switcher;
    //public Button innerButton;
    //public Group navigationPopup;
    public NavigatorPopup navPopup;
    public bool navigationPopupEnabled;

    public void SwitchView()
    {
        if (switchIndex >= 0)
        {
            Switcher.SwitchTo(switchIndex);
        }
    }

    public void MouseIn()
    {
        navPopup.Activate();
    }

    public void MouseOut()
    {
        navPopup.Deactivate();
    }



}
