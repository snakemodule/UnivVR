using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class Subtree : Group {

    public Button innerButton;

    public Group SubItemGroup;

    public NavigatorPopup navPopup;
    public bool navigationPopupEnabled;

    public CloseSiblingsCallback closeSiblings;

    private bool stateOpen = false;

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

    public void OnClick()
    {
        
        if (stateOpen)
            close();
        else
            open();

    }

    public void close()
    {
        SubItemGroup.Deactivate();
        updateLayout();
        stateOpen = false;
    }

    public void open()
    {
        closeSiblings();
        SubItemGroup.Activate();
        updateLayout();
        stateOpen = true;
    }

    private void updateLayout()
    {
        View temp = SubItemGroup;
        while (temp != null)
        {
            temp.LayoutChanged();
            temp = temp.Parent;
        }
    }

    public void setup(String label, CloseSiblingsCallback cb)
    {
        innerButton.Text.Value = label;
        navPopup.Offset.Value = new ElementMargin(this.innerButton.ActualWidth * 2, 0);
        this.closeSiblings = cb;
    }
}
