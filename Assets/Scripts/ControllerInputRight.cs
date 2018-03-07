using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputRight : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    CameraController CamController;

    UniviewConnection conn;

    // Use this for initialization
    void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        CamController = new CameraController(transform);
        conn = UniviewConnection.Instance;
    }

    void Start()
    {
        CamController.init();
    }
	
	// Update is called once per frame
	void Update () {
        CamController.Input(Controller);

        // 2
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }

        // 3
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }

        // 4
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
            conn.sendCommand("earth.planetfx\n");
        }

        // 5
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
    }
}
