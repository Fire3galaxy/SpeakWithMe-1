using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMove : MonoBehaviour {
    public float movementSpeed = .002f;
    public float rotationSpeed = 15f;

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

    public void moveForward()
    {
        moveDirection(transform.forward);
    }

    public void moveBackwards()
    {
        moveDirection(-transform.forward);
    }

    public void rotateLeft()
    {
        transform.Rotate(0f, -rotationSpeed, 0f);
    }

    public void rotateRight()
    {
        // Vector3 currRotation = transform.localEulerAngles;
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    // Expects a vector of magnitude 1
    private void moveDirection(Vector3 directionVector)
    {
        transform.position += directionVector * movementSpeed;
    }
}
