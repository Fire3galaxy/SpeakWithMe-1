using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMove : MonoBehaviour {
    public float ovrMovementSpeed = .002f;
    public float steamVrMovementSpeed = .02f;
    public float rotationSpeed = 15f;

    // Variables that considers all input methods
    [HideInInspector]
    public bool movingForward {get; private set;} = false;
    [HideInInspector]
    public bool movingBackward {get; private set;} = false;

    private float joystickThreshold = .2f;
    private Transform hmdTransform;

	// Use this for initialization
	void Start () {
        hmdTransform = transform.Find("Camera");
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || 
            Input.GetKey(KeyCode.DownArrow))
        {
            moveBackwards(false);
        }
        else if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || 
                 Input.GetKey(KeyCode.UpArrow))
        {
            moveForward(false);
        }
    }

    // For SteamVR D-Pad controls
    public void moveForward(SteamVR_Behaviour_Boolean fromBehaviour,
                            SteamVR_Input_Sources fromSource,
                            bool newValue)
    {
        if (newValue) moveForward(true);
        movingForward = newValue;
    }

    // Right now, only 2 options: steam or oculus/keyboard
    private void moveForward(bool isSteam)
    {
        transform.position += transform.forward * 
                              (isSteam ?  steamVrMovementSpeed : ovrMovementSpeed);
    }

    public void moveBackwards(SteamVR_Behaviour_Boolean fromBehaviour, 
                              SteamVR_Input_Sources fromSource, 
                              bool newValue)
    {
        if (newValue) moveBackwards(true);
        movingBackward = newValue;
    }

    private void moveBackwards(bool isSteam)
    {
        transform.position += -transform.forward * 
                              (isSteam ? steamVrMovementSpeed : ovrMovementSpeed);
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

    // For joystick controls
    public void handleAxisChange(SteamVR_Behaviour_Vector2 fromBehaviour, 
                                 SteamVR_Input_Sources fromSource, 
                                 Vector2 newAxis, 
                                 Vector2 newDelta)
    {
        // Threshold for moving
        if (Mathf.Abs(newAxis.x) < joystickThreshold && Mathf.Abs(newAxis.y) < joystickThreshold)
        {
            movingBackward = false;
            movingForward = false;
            return;
        }

        // Set moving forward/backward flags
        // Note: Threshold doesn't prevent y from being smaller than threshold. 
        // X would exceed threshold in this case.
        if (newAxis.y > 0f)
        {
            movingForward = true;
            movingBackward = false;
        } 
        else if (newAxis.y < 0f)
        {
            movingForward = false;
            movingBackward = true;
        }

        float playerYRotation = hmdTransform.eulerAngles.y * Mathf.Deg2Rad;
        float cosRot = Mathf.Cos(playerYRotation);
        float sinRot = Mathf.Sin(playerYRotation);

        Vector3 positionDelta = new Vector3(
                ovrMovementSpeed * (newAxis.x * cosRot + newAxis.y * sinRot),
                0f,
                ovrMovementSpeed * (-newAxis.x * sinRot + newAxis.y * cosRot)
            );
        transform.position += positionDelta;
    }
}
