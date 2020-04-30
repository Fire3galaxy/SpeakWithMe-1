using UnityEngine;
using Valve.VR;

public class PlayerDialogueControls : MonoBehaviour {
    SteamVR_Action_Boolean steamPlayRecordingButton;
    SteamVR_Action_Boolean steamNextDialogueButton;

    void Start()
    {
        steamPlayRecordingButton = SteamVR_Actions.demoControls_PlayRecording;
        steamNextDialogueButton = SteamVR_Actions.demoControls_NextDialogue;
    }

    public bool playRecordingButtonPressed()
    {
        return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Remote) || 
               Input.GetKeyDown(KeyCode.P) ||
               steamPlayRecordingButton.stateDown;
    }

    public bool nextDialogueButtonPressed()
    {
        return OVRInput.GetDown(OVRInput.Button.DpadRight, OVRInput.Controller.Remote) || 
               Input.GetKeyDown(KeyCode.N) ||
               steamNextDialogueButton.stateDown;
    }
}
