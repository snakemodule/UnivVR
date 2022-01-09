using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;
using System;

public class CommandCheckBox : CheckBox  {

    public String checkCommand;
    public String uncheckCommand;

    private VisibilityView visView;

    private UniviewConnection connection;
    private bool checkedByDefault = false;
    private string infoText;
    private AudioClip infoClip;

    private bool initialized = false;
    
    public override void Initialize()
    {
        base.Initialize();
        connection = UniviewConnection.Instance;
    }

    public void setup(ElementAlignment align, String label, String checkCommand, String uncheckCommand, 
        String infoText, AudioClip infoClip, String checkedbyDefault, int fontsize, VisibilityView visView)
    {
        this.Alignment.Value = ElementAlignment.TopLeft;
        this.Text.Value = label;
        this.checkCommand = checkCommand;
        this.uncheckCommand = uncheckCommand;
        this.infoText = infoText;
        this.infoClip = infoClip;
        if (checkedbyDefault == "true")
        {
            this.checkedByDefault = true;
        }
        if(checkedByDefault)
        {
            this.IsChecked.Value = true;
        }
        this.FontSize.Value = fontsize;
        this.visView = visView;
        
    }

    void Start ()
    {
        initialized = true;
    }

    public void setDefault() {
        this.IsChecked.Value = checkedByDefault;
    }


    public override void IsCheckedChanged()
    {
        base.IsCheckedChanged();

        if (IsChecked)
        {
            if(checkCommand != null && initialized)
            {
                connection.sendCommand(checkCommand);
                Debug.Log(checkCommand);
                visView.presentInfo(infoText, infoClip);
                
            }
        }
        else
        {
            if(uncheckCommand != null && initialized)
            {
                connection.sendCommand(uncheckCommand);
                Debug.Log(uncheckCommand);
            }
        }

    }


}
