using UnityEngine;
using Valve.VR;

using HeadsetType = PlayerLoader.HeadsetType;

class PlayerMoveControls : PlayPauseBehaviour, PlayerLoader.HeadsetChangeListener {
    [Tooltip("For VR and non-VR")]
    public float movementSpeed = .02f;
    [Tooltip("For VR, Degrees per button press")]
    [Range(0f, 360f)]
    public float rotationAmountPerPressVR = 15f;
    [Tooltip("For non-VR, Degrees per second")]
    [Range(0f, 360f)]
    public float rotationSpeedForDPadNonVR = 1.7f;
    [Tooltip("For non-VR. Degrees per pixel, which is used for rotation speed. For example, " +
             "setting this to .25 would mean 4 pixels of mouse movement equals 1 degree of " +
             "rotation per second.")]
    [Range(0f, 360f)]
    public float degreesPerPixelRotationSpeed = .25f;

    float joystickThreshold = .2f;
    float mouseThreshold = 5f;
    float currentXEulerAngle = 0f;  // used only for non-VR vertical rotation
    SteamVR_Action_Boolean steamForwardAction;
    SteamVR_Action_Boolean steamBackwardAction;
    SteamVR_Action_Boolean steamRotateLeftAction;
    SteamVR_Action_Boolean steamRotateRightAction;
    SteamVR_Action_Vector2 steamMoveJoystick;
    Vector3 lastMousePosition;
    Vector3 mousePositionDelta;

    // Provided by PlayerLoader in callback
    Transform hmdTransform = null;
    HeadsetType hmdType = HeadsetType.ValueNotSet;

    void Awake()
    {
        // PlayerLoader loads the headset at start. We need to be registered before Start().
        PlayerLoader loader = GetComponent<PlayerLoader>();
        loader.addListener(this);
    }

	void Start () {
        steamForwardAction = SteamVR_Actions.demoControls_MoveForward;
        steamBackwardAction = SteamVR_Actions.demoControls_MoveBackward;
        steamRotateLeftAction = SteamVR_Actions.demoControls_RotateLeft;
        steamRotateRightAction = SteamVR_Actions.demoControls_RotateRight;
        steamMoveJoystick = SteamVR_Actions.demoControls_MoveJoystick;
	}

    protected override void UpdatePlay()
    {
        // Camera can be reloaded due to settings change. Avoid action during these rare frames.
        if (hmdTransform == null || hmdType == HeadsetType.ValueNotSet) return;

        // JOYSTICK CONTROLS
        if (movingForwardJoystick() || movingBackwardsJoystick() ||
                movingLeftJoystick() || movingRightJoystick())
            moveWithJoystick(steamMoveJoystick.axis);

        // D-PAD CONTROLS
        // Don't count joystick twice for motion
        if (movingForward() && !movingForwardJoystick()) moveForward();
        if (movingBackward() && !movingBackwardsJoystick()) moveBackward();

        // Non-VR actions and Rotating Camera (Mouse, D-pad, or Keyboard)
        if (hmdType == HeadsetType.NoVR)
        {
            // Keyboard only - Strafe left/right with arrow keys
            if (movingLeftNoVRWithButtons()) moveLeft();
            if (movingRightNoVRWithButtons()) moveRight();
            
            // Calculate the delta before calling rotating(Up/Down/Left/Right)
            // because those functions don't calculate delta (to avoid repeat calculations)
            updateMouseDelta();

            if (rotatingUpNoVRWithMouse() || rotatingDownNoVRWithMouse())
            {
                rotateVerticalNoVR(-mousePositionDelta.y * degreesPerPixelRotationSpeed * Time.deltaTime);
            }

            if (rotatingLeftNoVRWithMouse() || rotatingRightNoVRWithMouse())
            {
                rotateHorizontal(mousePositionDelta.x * degreesPerPixelRotationSpeed * Time.deltaTime);
            }

            float xRotation = (rotatingUpNoVRWithButtons()    ? -rotationSpeedForDPadNonVR * Time.deltaTime : 0) +
                              (rotatingDownNoVRWithButtons()  ?  rotationSpeedForDPadNonVR * Time.deltaTime : 0);
            float yRotation = (rotatingRightNoVRWithButtons() ?  rotationSpeedForDPadNonVR * Time.deltaTime : 0) +
                              (rotatingLeftNoVRWithButtons()  ? -rotationSpeedForDPadNonVR * Time.deltaTime : 0);
            rotateVerticalNoVR(xRotation);
            rotateHorizontal(yRotation);

        }
        else
        {
            float yRotation = (rotatingRightVR() ? rotationAmountPerPressVR : 0) +
                              (rotatingLeftVR() ? -rotationAmountPerPressVR : 0);
            rotateHorizontal(yRotation);
        }
    }

    protected override void UpdatePause()
    {
        // Do nothing
    }

    void moveForward()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void moveBackward()
    {
        transform.position -= transform.forward * movementSpeed * Time.deltaTime;
    }

    void moveLeft()
    {
        transform.position -= transform.right * movementSpeed * Time.deltaTime;
    }

    void moveRight()
    {
        transform.position += transform.right * movementSpeed * Time.deltaTime;
    }

    void rotateHorizontal(float rotationAmount)
    {
        transform.Rotate(0f, rotationAmount, 0f, Space.World);
    }

    void rotateVerticalNoVR(float rotationAmount)
    {
        // Clamp to 90 degrees above and below player (no upside down)
        // There are edge cases where transform.eulerAngles.x is represented in such a
        // way that it obeys this clamp but still puts the user upside down. Avoiding this
        // by tracking x euler angle ourselves.
        float clampedRotationAmount = Mathf.Clamp(rotationAmount,
                                                  -90f - currentXEulerAngle,
                                                  90f - currentXEulerAngle);

        // Rotates the camera rather than the Player object. Don't do this with VR.
        // Benefit is that transform.forward stays aligned on the XZ plane
        hmdTransform.Rotate(clampedRotationAmount, 0f, 0f, Space.Self);
        currentXEulerAngle += clampedRotationAmount;
    }

    void moveWithJoystick(Vector2 axis)
    {
        float playerYRotation = hmdTransform.eulerAngles.y * Mathf.Deg2Rad;
        float cosRot = Mathf.Cos(playerYRotation);
        float sinRot = Mathf.Sin(playerYRotation);

        Vector3 positionDelta = new Vector3(
                movementSpeed * Time.deltaTime * (axis.x * cosRot + axis.y * sinRot),
                0f,
                movementSpeed * Time.deltaTime * (-axis.x * sinRot + axis.y * cosRot)
            );
        transform.position += positionDelta;
    }

    void updateMouseDelta()
    {
        // If player has not clicked, mouse delta is 0
        if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))) {
            mousePositionDelta = Vector3.zero;
            return;
        }

        // First mouse click must be within game window
        if (Input.GetMouseButtonDown(0) && mousePositionIsOnScreen(Input.mousePosition)) {
            lastMousePosition = Input.mousePosition;
        }

        // Note: Delta can include mouse position being outside of window, so long as first click
        // is within window.
        mousePositionDelta = Input.mousePosition - lastMousePosition;
    }

    public bool movingForward()
    {
        // FIXME: Null exceptions happening here with Steam variables.
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

    bool movingLeftNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.LeftArrow);
    }

    bool movingRightNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.RightArrow);
    }

    bool movingForwardJoystick()
    {
        return steamMoveJoystick.axis.y > joystickThreshold;
    }

    bool movingBackwardsJoystick()
    {
        return steamMoveJoystick.axis.y < -joystickThreshold;
    }

    bool movingLeftJoystick()
    {
        return steamMoveJoystick.axis.x < -joystickThreshold;
    }

    bool movingRightJoystick()
    {
        return steamMoveJoystick.axis.x > joystickThreshold;
    }

    bool rotatingLeftVR()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || steamRotateLeftAction.stateDown;
    }

    bool rotatingRightVR()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || steamRotateRightAction.stateDown;
    }

    bool rotatingLeftNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.A) || steamRotateLeftAction.state;
    }

    bool rotatingRightNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.D) || steamRotateRightAction.state;
    }

    bool rotatingUpNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.W);
    }

    bool rotatingDownNoVRWithButtons()
    {
        return Input.GetKey(KeyCode.S);
    }

    bool rotatingRightNoVRWithMouse()
    {
        return mousePositionDelta.x > mouseThreshold;
    }

    bool rotatingLeftNoVRWithMouse()
    {
        return mousePositionDelta.x < -mouseThreshold;
    }

    bool rotatingUpNoVRWithMouse()
    {
        return mousePositionDelta.y > mouseThreshold;
    }

    bool rotatingDownNoVRWithMouse()
    {
        return mousePositionDelta.y < -mouseThreshold;
    }

    bool mousePositionIsOnScreen(Vector3 position)
    {
        return position.x >= 0f && position.x <= Screen.width &&
               position.y >= 0f && position.y <= Screen.height;
    }

    public void onRemoveHeadset(HeadsetType headsetType)
    {
        hmdType = HeadsetType.ValueNotSet;
        hmdTransform = null;
    }

    public void onLoadHeadset(HeadsetType headsetType, GameObject hmdObject)
    {
        hmdType = headsetType;
        switch (headsetType)
        {
            case HeadsetType.NoVR:
                hmdTransform = hmdObject.transform;
                break;
            case HeadsetType.OpenVR:
                hmdTransform = hmdObject.transform.Find("Camera");
                break;
            case HeadsetType.OVR:
                hmdTransform = hmdObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
                break;
            default:
                Debug.LogError("Unsupported headset type " + headsetType);
                break;
        }
    }
}
