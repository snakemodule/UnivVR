using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

public class Options : UIView
{
    public _float Volume;
    public _bool EasyMode;
    public _string PlayerName;

    public void ResetDefaults()
    {
        SetValue(() => Volume, 75.0f); // Volume = 75.0f
        SetValue(() => EasyMode, true); // EasyMode = true
        SetValue(() => PlayerName, "Player"); // PlayerName = "Player"
    }
}
