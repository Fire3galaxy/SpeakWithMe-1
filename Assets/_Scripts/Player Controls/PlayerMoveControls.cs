using UnityEngine;
using Valve.VR;

using HeadsetType = PlayerLoader.HeadsetType;

public class PlayerMoveControls : MonoBehaviour {
    [Tooltip("For VR and non-VR")]
    public float movementSpeed = .02f;
    [Tooltip("For VR")]
    [Range(0f, 360f)]
    public float rotationAmountPerPress = 15f;
    [Tooltip("For non-VR")]
    [Range(0f, 15f)]
    public float rotationSpeed = 1.7f;

    float joystickThreshold = .2f;
    SteamVR_Action_Boolean steamForwardAction;
    SteamVR_Action_Boolean steamBackwardAction;
    SteamVR_Action_Boolean steamRotateLeftAction;
    SteamVR_Action_Boolean steamRotateRightAction;
    SteamVR_Action_Vector2 steamMoveJoystick;
    Transform hmdTransform;
    HeadsetType hmdType;

	// Use this for initialization
	void Start () {
        hmdType = GetComponent<PlayerLoader>().headsetType;
        string cameraLocation = hmdType == HeadsetType.OVR ?    "OVRPlayerController/OVRCameraRig/" +
                                                                    "TrackingSpace/CenterEyeAnchor" :
                                hmdType == HeadsetType.OpenVR ? "OpenVR Player/Camera" : 
                                                                "Standard Camera";
        hmdTransform = transform.Find(cameraLocation);
        steamForwardAction = SteamVR_Actions.demoControls_MoveForward;
        steamBackwardAction = SteamVR_Actions.demoControls_MoveBackward;
        steamRotateLeftAction = SteamVR_Actions.demoControls_RotateLeft;
        steamRotateRightAction = SteamVR_Actions.demoControls_RotateRight;
        steamMoveJoystick = SteamVR_Actions.demoControls_Move;
	}
	
	void FixedUpdate () {
        // JOYSTICK CONTROLS
        if (joystickExceedsThreshold()) moveWithJoystick(steamMoveJoystick.axis);

        // DPAD CONTROLS
        // Don't count joystick twice for motion
        if (movingForward() && !movingForwardJoystick()) moveForward();
        if (movingBackward() && !movingBackwardsJoystick()) moveBackward();

        if (hmdType == HeadsetType.NoVR)
        {
            if (rotatingLeftNoVR()) rotateLeft(rotationSpeed);
            if (rotatingRightNoVR()) rotateRight(rotationSpeed);
        }
        else
        {
            if (rotatingLeftVR()) rotateLeft(rotationAmountPerPress);
            if (rotatingRightVR()) rotateRight(rotationAmountPerPress);
        }
    }

    void moveForward()
    {
        transform.position += transform.forward * movementSpeed;
    }

    void moveBackward()
    {
        transform.position += -transform.forward * movementSpeed;
    }

    void rotateLeft(float rotationAmount)
    {
        transform.Rotate(0f, -rotationAmount, 0f);
    }

    void rotateRight(float rotationAmount)
    {
        transform.Rotate(0f, rotationAmount, 0f);
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

    public bool rotatingRightNoVR()
    {
        return Input.GetKey(KeyCode.RightArrow) || steamRotateRightAction.state;
    }

    public bool rotatingRightVR()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || steamRotateRightAction.stateDown;
    }

    public bool rotatingLeftNoVR()
    {
        return Input.GetKey(KeyCode.LeftArrow) || steamRotateLeftAction.state;
    }

    public bool rotatingLeftVR()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || steamRotateLeftAction.stateDown;
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
