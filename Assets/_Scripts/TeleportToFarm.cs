using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToFarm : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(-26.357f, transform.position.y, -19.226f);
            transform.eulerAngles = new Vector3(0.0f, -16.197f, 0.0f);
        }
	}
}
