using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMove : MonoBehaviour {
    public float speed = .002f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || 
            Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection(-transform.forward);
        }
        else if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || 
                 Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection(transform.forward);
        }
    }

    public void moveForward(SteamVR_Behaviour_Boolean fromBehaviour, 
                            SteamVR_Input_Sources fromSource, 
                            bool value)
    {
        Debug.Log(fromBehaviour.booleanAction.GetPath());
        moveDirection(transform.forward);
    }

    public void moveBackwards()
    {
        moveDirection(-transform.forward);
    }

    public void moveLeft()
    {
        moveDirection(-transform.right);
    }

    public void moveRight()
    {
        moveDirection(transform.right);
    }

    // Expects a vector of magnitude 1
    private void moveDirection(Vector3 directionVector)
    {
        transform.position += directionVector * speed;
    }
}
