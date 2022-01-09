using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;
using MarkLight.Views.UI;
using System.Text;
using System;
using System.Xml;
using System.IO;

public delegate void SwitchCallback();

public class MainView : View
{
    public ViewSwitcher mainViewSwitcher;
    public ModeView modeView;
    public LibraryView libraryView;
    public TourView tourView;

    public override void Initialize()
    {
        base.Initialize();
        modeView.setup(makeSwitchCallback("libraryView"), makeSwitchCallback("tourView"));
        libraryView.setup(makeSwitchCallback("modeView"));
        tourView.setup(makeSwitchCallback("modeView"));

    }

    private SwitchCallback makeSwitchCallback(String id)
    {
        return delegate () { mainViewSwitcher.SwitchTo(id); };
    }
}
