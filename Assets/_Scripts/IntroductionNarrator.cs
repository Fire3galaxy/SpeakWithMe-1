using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Intro Narrator script (bind to main player or object with audio source)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour, MicReceiver {
    public AudioClip[] clips;
    public GameObject IntroRecordingIcon;

    AudioSource PlayerAudio;
    int currClip = 0;
    bool startedPlaying = false;
    bool playedPlayer8 = false;
    bool finishedRecording = false;
    float timePassed = 0f;
    delegate void AdvanceFunction();
    SteamVR_Action_Boolean forwardAction, backwardAction, rotateLeft, rotateRight;

    // Use this for initialization
    void Start () 
    {
        PlayerAudio = GetComponent<AudioSource>();
        forwardAction = SteamVR_Actions.demoControls_MoveForward;
        backwardAction = SteamVR_Actions.demoControls_MoveBackward;
        rotateLeft = SteamVR_Actions.demoControls_RotateLeft;
        rotateRight = SteamVR_Actions.demoControls_RotateRight;
    }

    private void handleUpdate(bool advanceConditionMet, params AdvanceFunction[] functions)
    {
        if (!startedPlaying) 
        {
            playCurrClip();
        }
        else if (advanceConditionMet) 
        {
            foreach (AdvanceFunction func in functions)
            {
                func();
            }
        }
    }

    private void playCurrClip()
    {
        PlayerAudio.clip = clips[currClip];
        PlayerAudio.Play();
        startedPlaying = true;
    }

    private void advanceClip()
    {
        currClip++;
        startedPlaying = false;
    }

    private void startRecording()
    {
        IntroRecordingIcon.SetActive(true);
        GetComponent<PlayerMic>().StartRecording(this);
    }

    private void stopRecording()
    {
        IntroRecordingIcon.SetActive(false);
        GetComponent<PlayerMic>().StopRecording();
    }

    private void playRecording()
    {
        PlayerAudio.clip = GetComponent<PlayerMic>().recording;
        PlayerAudio.Play();
    }
	
    public void onFinishedRecording()
    {
        finishedRecording = true;
    }

	// Update is called once per frame
	void Update () 
    {
        if (!PlayerAudio.isPlaying)
        {
            switch (currClip)
            {
                case 0:
                case 1:
                case 9:
                    handleUpdate(true, advanceClip);
                    break;
                // Teaching moving forward
                case 2:
                    handleUpdate(OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || 
                                    Input.GetKey(KeyCode.UpArrow) || 
                                    forwardAction.state, 
                                advanceClip);
                    break;
                // Teaching moving backwards
                case 3:
                    handleUpdate(OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || 
                                    Input.GetKey(KeyCode.DownArrow) || 
                                    backwardAction.state, 
                                advanceClip);
                    break;
                case 4:
                    handleUpdate(OVRInput.Get(OVRInput.Button.DpadDown | OVRInput.Button.DpadUp, 
                                            OVRInput.Controller.Remote) || 
                                    Input.GetKey(KeyCode.DownArrow) || 
                                    Input.GetKey(KeyCode.UpArrow) || 
                                    forwardAction.state || 
                                    backwardAction.state, 
                                advanceClip);
                    break;
                // Getting first recording
                case 5:
                    handleUpdate(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Remote) || 
                                    Input.GetKey("space"), 
                                startRecording,
                                advanceClip);
                    break;
                // Waiting 3 seconds for recording
                case 6:
                    timePassed += Time.deltaTime;
                    handleUpdate(timePassed >= 3.0f, advanceClip);
                    break;
                // Stopping recording
                case 7:
                    handleUpdate(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Remote) || 
                                    Input.GetKey("space"), 
                                stopRecording, 
                                advanceClip);
                    break;
                // Playing back recording
                case 8:
                    if (!playedPlayer8) {
                        handleUpdate(true, playRecording);
                        playedPlayer8 = true;
                    } else {
                        handleUpdate(true, advanceClip);
                    }
                    break;
                default:
                    break;
            }
        }
	}
}
