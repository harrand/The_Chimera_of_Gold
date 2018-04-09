using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCamera : CameraControl {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject current = GameObject.FindGameObjectWithTag("SkyNet");

        if (current != null)
        {
            //offset from tile

            SetCameraPosition(current.transform);
        }
    }
}
