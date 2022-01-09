using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReference : MonoBehaviour {

    public GameObject rotateLabel;
    public GameObject zoomLabel;
    public GameObject rollLabel;
    public GameObject panLabel;

    public void showPan()
    {
        panLabel.SetActive(true);
    }

    public void hidePan()
    {
        panLabel.SetActive(false);
    }
}
