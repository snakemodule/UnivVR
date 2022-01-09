using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class TreeListButton : Group {

    public Button innerButton;
    public Group SubItemGroup;
    
    public bool stateOpen = false;

    public NavigatorPopup navPopup;
    public bool navigationPopupEnabled;

    private CloseSiblingsCallback closeSiblings;

    //public List<Subtree> subtreeButtons;

    //public Frame buttonFrame;

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
        {
            close();
        } else 
        {
            open();
        }
    }

    public void close()
    {
        closeSubtrees();
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

    public void closeSubtrees()
    {
        foreach (Subtree s in SubItemGroup.GetChildren<Subtree>(false))
        {
            s.close();
        }
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
        navPopup.Offset.Value = new ElementMargin(innerButton.ActualWidth * 2, 0);
        this.closeSiblings = cb;
        //subtreeButtons = new List<Subtree>();
    }
}
