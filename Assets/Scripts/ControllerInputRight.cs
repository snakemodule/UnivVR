using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputRight : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public CameraController CamController { get; private set; }

    public GameObject ViewPresenter;

    // Use this for initialization
    void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        CamController = new CameraController(transform, GameObject.Find("CameraReference").GetComponent<CameraReference>());
    }
	
	// Update is called once per frame
	void Update () {
        CamController.Input(Controller);

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            if(ViewPresenter.activeSelf)
            {
                ViewPresenter.SetActive(false);
            } else
            {
                ViewPresenter.SetActive(true);
            }
        }

    }
}
