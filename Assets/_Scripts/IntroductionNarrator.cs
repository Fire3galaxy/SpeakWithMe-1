using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Intro Narrator script (bind to main player)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour {
    public AudioClip[] clips;
    public GameObject IntroRecordingIcon;

    AudioSource playerAudio;
    PlayerMove playerMover;
    PlayerMic playerMic;
    int currClip = 0;
    bool startedPlaying = false;
    bool playedRecording = false;
    float timePassed = 0f;
    delegate void AdvanceFunction();
    SteamVR_Action_Boolean forwardAction, backwardAction, rotateLeft, rotateRight;

    // Use this for initialization
    void Start () 
    {
        playerAudio = GetComponent<AudioSource>();
        playerMover = GetComponent<PlayerMove>();
        playerMic = GetComponent<PlayerMic>();

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

    private void playRecording()
    {
        playerAudio.clip = GetComponent<PlayerMic>().recording;
        playerAudio.Play();
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
                    // Ensure recording isn't in progress already due to button mashing
                    if (!startedPlaying && playerMic.isRecording) playerMic.StopRecording();
                    handleUpdate(playerMic.recordButtonPressed, advanceClip);
                    break;
                // Waiting 3 seconds for recording
                case 6:
                    timePassed += Time.deltaTime;
                    handleUpdate(timePassed >= 3.0f, advanceClip);
                    break;
                // Stopping recording
                case 7:
                    handleUpdate(playerMic.recordButtonPressed || !playerMic.isRecording, 
                                 advanceClip);
                    break;
                // Playing back recording, sending player off
                case 8:
                    // Player's recording isn't in our array, so we treat it as a precondition
                    // to playing the next clip.
                    if (!playedRecording)
                    {
                        playRecording();
                        playedRecording = true;
                    } 
                    else 
                    {
                        handleUpdate(true, advanceClip);
                    }
                    break;
                default:
                    break;
            }
        }
	}
}
