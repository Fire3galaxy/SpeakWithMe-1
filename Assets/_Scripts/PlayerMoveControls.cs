using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMoveControls : MonoBehaviour {
    public float movementSpeed = .02f;
    public float rotationSpeed = 15f;

    float joystickThreshold = .2f;
    SteamVR_Action_Boolean steamForwardAction;
    SteamVR_Action_Boolean steamBackwardAction;
    SteamVR_Action_Boolean steamRotateLeftAction;
    SteamVR_Action_Boolean steamRotateRightAction;
    SteamVR_Action_Vector2 steamMoveJoystick;

    Transform hmdTransform;

	// Use this for initialization
	void Start () {
        hmdTransform = transform.Find("Camera");
        steamForwardAction = SteamVR_Actions.demoControls_MoveForward;
        steamBackwardAction = SteamVR_Actions.demoControls_MoveBackward;
        steamRotateLeftAction = SteamVR_Actions.demoControls_RotateLeft;
        steamRotateRightAction = SteamVR_Actions.demoControls_RotateRight;
        steamMoveJoystick = SteamVR_Actions.demoControls_Move;
	}
	
	// Update is called once per frame
	void Update () {
        if (joystickExceedsThreshold())
        {
            moveWithJoystick(steamMoveJoystick.axis);
        }

        // Don't count joystick twice for motion
        if (movingForward() && !movingForwardJoystick())
        {
            moveForwardDPad();
        }
        
        if (movingBackward() && !movingBackwardsJoystick())
        {
            moveBackwardDPad();
        }

        if (rotatingRight())
        {
            rotateRightDPad();
        }

        if (rotatingLeft())
        {
            rotateLeftDPad();
        }
    }

    // Right now, only 2 options: steam or oculus/keyboard
    void moveForwardDPad()
    {
        transform.position += transform.forward * movementSpeed;
    }

    void moveBackwardDPad()
    {
        transform.position += -transform.forward * movementSpeed;
    }

    public void rotateLeftDPad()
    {
        transform.Rotate(0f, -rotationSpeed, 0f);
    }

    public void rotateRightDPad()
    {
        // Vector3 currRotation = transform.localEulerAngles;
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    public bool movingForward()
    {
        return OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || 
               Input.GetKey(KeyCode.UpArrow) ||
               steamForwardAction.state ||
               movingForwardJoystick();
    }

    public bool movingBackward()
    {
        return OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || 
               Input.GetKey(KeyCode.DownArrow) ||
               steamBackwardAction.state ||
               movingBackwardsJoystick();
    }

    public bool rotatingRight()
    {
        return steamRotateRightAction.stateDown;
    }

    public bool rotatingLeft()
    {
        return steamRotateLeftAction.stateDown;
    }

    void moveWithJoystick(Vector2 axis)
    {
        float playerYRotation = hmdTransform.eulerAngles.y * Mathf.Deg2Rad;
        float cosRot = Mathf.Cos(playerYRotation);
        float sinRot = Mathf.Sin(playerYRotation);

        Vector3 positionDelta = new Vector3(
                movementSpeed * (axis.x * cosRot + axis.y * sinRot),
                0f,
                movementSpeed * (-axis.x * sinRot + axis.y * cosRot)
            );
        transform.position += positionDelta;
    }

    bool joystickExceedsThreshold()
    {
        Vector2 axis = steamMoveJoystick.axis;
        return Mathf.Abs(axis.x) >= joystickThreshold || Mathf.Abs(axis.y) >= joystickThreshold;
    }

    bool movingForwardJoystick()
    {
        return joystickExceedsThreshold() && steamMoveJoystick.axis.y > 0f;
    }

    bool movingBackwardsJoystick()
    {
        return joystickExceedsThreshold() && steamMoveJoystick.axis.y < 0f;
    }
}
