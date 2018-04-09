using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyRecon : NetworkBehaviour {
    Vector3 target, position;
    Quaternion rotation, targetRot;

	// Use this for initialization
	void Start () {
        target = position = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if(Mathf.Round(target.x) == Mathf.Round(position.x) || Mathf.Round(target.y) == Mathf.Round(position.y) || Mathf.Round(target.z) == Mathf.Round(position.z))
            target = new Vector3(Random.Range(0, 220), Random.Range(5, 100), Random.Range(0,100));

        
        position = transform.position;
        transform.position = Vector3.Lerp(position, target, Time.deltaTime * 0.2f);
        

	}
}
