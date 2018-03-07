using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommand : MonoBehaviour {
	// Use this for initialization
	void Awake () {
        //Button 
        
	}

    public void testFunction()
    {
        UniviewConnection fxCommand = UniviewConnection.Instance;
        fxCommand.sendCommand("earth.planetfx\n");
    }
}
