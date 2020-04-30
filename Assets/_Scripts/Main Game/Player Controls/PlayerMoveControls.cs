using UnityEngine;
using Valve.VR;

using HeadsetType = PlayerLoader.HeadsetType;

public class PlayerMoveControls : MonoBehaviour {
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
    SteamVR_Action_Boolean steamForwardAction;
    SteamVR_Action_Boolean steamBackwardAction;
    SteamVR_Action_Boolean steamRotateLeftAction;
    SteamVR_Action_Boolean steamRotateRightAction;
    SteamVR_Action_Vector2 steamMoveJoystick;
    Transform hmdTransform;
    HeadsetType hmdType;
    Vector3 lastMousePosition;
    Vector3 mousePositionDelta;

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

	void Update () {
        // JOYSTICK CONTROLS
        if (movingForwardJoystick() || movingBackwardsJoystick() ||
                movingLeftJoystick() || movingRightJoystick())
            moveWithJoystick(steamMoveJoystick.axis);

        // DPAD CONTROLS
        // Don't count joystick twice for motion
        if (movingForward() && !movingForwardJoystick()) moveForward();
        if (movingBackward() && !movingBackwardsJoystick()) moveBackward();

        if (hmdType == HeadsetType.NoVR)
        {
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

    void moveForward()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void moveBackward()
    {
        transform.position += -transform.forward * movementSpeed * Time.deltaTime;
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
}
