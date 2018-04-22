using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float speed = .002f;

	// Use this for initialization
	void Start () {
        foreach (string s in Input.GetJoystickNames())
            Debug.Log(s);
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += -transform.forward * speed;
        } else if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * speed;
        }
    }
}
