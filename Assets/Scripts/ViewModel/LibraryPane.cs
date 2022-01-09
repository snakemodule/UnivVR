using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class LibraryPane : UIView {

    public Group organizer;
    public Button backbutton;

    //private List<TreeListButton> treeListButtons;


    //public ViewSwitcher switcher;
    //public int backIndex;
    private SwitchCallback switcherCallback;
    private SwitchCallback closeCallback;

    public void Back()
    {
        //switcher.SwitchTo(backIndex);
        switcherCallback();
    }

    public void closeTreeListButtons()
    {
        foreach (TreeListButton tlb in organizer.GetChildren<TreeListButton>(false))
        {
            tlb.close();
        }
    }

    public void clickClose()
    {
        closeCallback();
    }

    public void setup(SwitchCallback cb, SwitchCallback closeCallback)
    {
        switcherCallback = cb;
        this.closeCallback = closeCallback;
    }
}
