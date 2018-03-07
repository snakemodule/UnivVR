using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeTextureRotation : MonoBehaviour {

    public float DomeAngle = 45f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var rot = Quaternion.Euler(DomeAngle, 0, 0);
        var m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        GetComponent<Renderer>().sharedMaterial.SetMatrix("_TextureRotation", m);
    }
}
