using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Intro Narrator script (bind to main player)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour, MicReceiver {
    public AudioClip[] clips;
    public GameObject IntroRecordingIcon;

    AudioSource playerAudio;
    PlayerMove playerMover;
    int currClip = 0;
    bool startedPlaying = false;
    bool playedPlayer8 = false;
    float timePassed = 0f;
    delegate void AdvanceFunction();
    SteamVR_Action_Boolean forwardAction, backwardAction, rotateLeft, rotateRight;

    // Use this for initialization
    void Start () 
    {
        playerAudio = GetComponent<AudioSource>();
        playerMover = GetComponent<PlayerMove>();

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
        playerAudio.clip = clips[currClip];
        playerAudio.Play();
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
        playerAudio.clip = GetComponent<PlayerMic>().recording;
        playerAudio.Play();
    }
	
    public void onFinishedRecording()
    {
    }

	// Update is called once per frame
	void Update () 
    {
        if (!playerAudio.isPlaying)
        {
            switch (currClip)
            {
                // Just go to next clip once clip has played
                case 0:
                case 1:
                case 9:
                    handleUpdate(true, advanceClip);
                    break;
                // Teaching moving forward
                case 2:
                    handleUpdate(playerMover.movingForward, advanceClip);
                    break;
                // Teaching moving backwards
                case 3:
                    handleUpdate(playerMover.movingBackward, 
                                advanceClip);
                    break;
                // Teaching moving in arbitrary direction
                case 4:
                    handleUpdate(playerMover.movingBackward || playerMover.movingForward, 
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
