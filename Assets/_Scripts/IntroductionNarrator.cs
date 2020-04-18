using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Intro Narrator script (bind to main player)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour {
    public AudioClip[] clips;
    public GameObject IntroRecordingIcon;

    AudioSource narratorAudio, playerAudio;
    PlayerMoveControls playerMover;
    PlayerMicControls playerMic;
    PlayerDialogueControls playerDialogueControls;
    int currClip = 5; // 0; DEBUGGING
    bool startedPlaying = false;
    bool playedRecording = false;
    float timePassed = 0f;
    delegate void AdvanceFunction();

    // Use this for initialization
    void Start () 
    {
        narratorAudio = transform.Find("Narrator Audio").GetComponent<AudioSource>();
        playerAudio = transform.Find("Recordings Audio").GetComponent<AudioSource>();
        playerMover = GetComponent<PlayerMoveControls>();
        playerMic = GetComponent<PlayerMicControls>();
        playerDialogueControls = GetComponent<PlayerDialogueControls>();
    }

    void handleClipsUpdate(bool advanceConditionMet, params AdvanceFunction[] functions)
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

    void playCurrClip()
    {
        Debug.Log("Playing clip named " + clips[currClip].name);
        playerAudio.Stop();
        narratorAudio.clip = clips[currClip];
        narratorAudio.Play();
        startedPlaying = true;
    }

    void advanceClip()
    {
        currClip++;
        startedPlaying = false;
    }

    void playRecording()
    {
        Debug.Log("Playing recording of length " + playerMic.recording.length);
        narratorAudio.Stop();
        playerAudio.clip = playerMic.recording.audioClip;
        playerAudio.PlayScheduled(AudioSettings.dspTime);
        playerAudio.SetScheduledEndTime(AudioSettings.dspTime + playerMic.recording.length);
    }

	// Update is called once per frame
	void Update () 
    {
        if (!narratorAudio.isPlaying && !playerAudio.isPlaying)
        {
            switch (currClip)
            {
                // Just go to next clip once clip has played
                case 0:
                case 1:
                case 8:
                    handleClipsUpdate(true, advanceClip);
                    break;
                // Teaching moving forward
                case 2:
                    handleClipsUpdate(playerMover.movingForward(), advanceClip);
                    break;
                // Teaching moving backwards
                case 3:
                    handleClipsUpdate(playerMover.movingBackward(), 
                                advanceClip);
                    break;
                // Teaching moving in arbitrary direction
                case 4:
                    handleClipsUpdate(playerMover.movingBackward() || playerMover.movingForward(), 
                                 advanceClip);
                    break;
                // Getting first recording
                case 5:
                    // Ensure recording isn't in progress already due to button mashing
                    if (!startedPlaying && playerMic.isRecording()) playerMic.StopRecording();
                    handleClipsUpdate(playerMic.recordButtonPressed(), advanceClip);
                    break;
                // Waiting 3 seconds for recording
                case 6:
                    timePassed += Time.deltaTime;
                    handleClipsUpdate(timePassed >= 3.0f, advanceClip);
                    break;
                // Stopping recording
                case 7:
                    handleClipsUpdate(playerMic.recordButtonPressed() || !playerMic.isRecording(), 
                                 advanceClip);
                    break;
                // Playing back recording, sending player off
                case 9:
                    // Player's recording isn't in our array, so we treat it as a precondition
                    // to playing the next clip.
                    if (!playedRecording)
                    {
                        if (playerDialogueControls.playRecordingButtonPressed())
                        {
                            playRecording();
                            playedRecording = true;
                        }
                    }
                    else 
                    {
                        handleClipsUpdate(true, advanceClip);
                    }
                    break;
                default:
                    break;
            }
        }
	}
}
