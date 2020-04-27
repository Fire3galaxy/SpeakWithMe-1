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
        hmdType = (HeadsetType) PlayerPrefs.GetInt(PlayStyleSettings.preferenceKey);
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
            float xRotation = (rotatingUpNoVR() ? -rotationSpeed : 0) + 
                              (rotatingDownNoVR() ? rotationSpeed : 0);
            float yRotation = (rotatingRightNoVR() ? rotationSpeed : 0) + 
                              (rotatingLeftNoVR() ? -rotationSpeed : 0);
            rotateVerticalNoVR(xRotation);
            rotateHorizontal(yRotation);
        }
        else
        {
            float yRotation = (rotatingRightVR() ? rotationAmountPerPress : 0) + 
                              (rotatingLeftVR() ? -rotationAmountPerPress : 0);
            rotateHorizontal(yRotation);
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

    void rotateHorizontal(float rotationAmount)
    {
        transform.Rotate(0f, rotationAmount, 0f, Space.World);
    }

    void rotateVerticalNoVR(float rotationAmount)
    {
        // localEulerAngles uses [0,360f) instead of negatives, which I need for the formula below.
        float adjustedLocalXRotation = hmdTransform.localEulerAngles.x;
        if (adjustedLocalXRotation > 90f) adjustedLocalXRotation = adjustedLocalXRotation - 360f;

        // Clamp to 90 degrees above and below player (no upside down)
        float clampedRotationAmount = Mathf.Clamp(rotationAmount, 
                                                  -90f - adjustedLocalXRotation, 
                                                  90f - adjustedLocalXRotation);

        // Rotates the camera rather than the Player object. Don't do this with VR.
        // Benefit is that transform.forward stays aligned on the XZ plane
        hmdTransform.Rotate(clampedRotationAmount, 0f, 0f, Space.Self);
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

    public bool rotatingLeftVR()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || steamRotateLeftAction.stateDown;
    }

    public bool rotatingRightVR()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || steamRotateRightAction.stateDown;
    }

    public bool rotatingLeftNoVR()
    {
        return Input.GetKey(KeyCode.A) || steamRotateLeftAction.state;
    }

    public bool rotatingRightNoVR()
    {
        return Input.GetKey(KeyCode.D) || steamRotateRightAction.state;
    }

    public bool rotatingUpNoVR()
    {
        return Input.GetKey(KeyCode.W);
    }

    public bool rotatingDownNoVR()
    {
        return Input.GetKey(KeyCode.S);
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
