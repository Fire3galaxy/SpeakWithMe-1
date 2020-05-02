using UnityEngine;

using AudioState = AudioSourceWrapper.AudioState;

// Intro Narrator script (bind to main player)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour {
    public AudioClip[] clips;
    public int debugStartingIndex = 9;

    AudioSourceWrapper narratorAudioSourceWrapper, playerAudioSourceWrapper;
    PlayerMoveControls playerMover;
    PlayerMicControls playerMic;
    PlayerDialogueControls playerDialogueControls;
    int currClip = 0;
    bool startedPlaying = false;
    bool playedRecording = false;
    float timePassed = 0f;
    delegate void AdvanceFunction();

    // Use this for initialization
    void Start () 
    {
        narratorAudioSourceWrapper = transform.Find("Narrator Audio").GetComponent<AudioSourceWrapper>();
        playerAudioSourceWrapper = transform.Find("Recordings Audio").GetComponent<AudioSourceWrapper>();
        playerMover = GetComponent<PlayerMoveControls>();
        playerMic = GetComponent<PlayerMicControls>();
        playerDialogueControls = GetComponent<PlayerDialogueControls>();

        // DEBUG CODE
        currClip = debugStartingIndex;
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
        playerAudioSourceWrapper.Stop();
        narratorAudioSourceWrapper.Play(clips[currClip]);
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
        narratorAudioSourceWrapper.Stop();
        playerAudioSourceWrapper.PlayScheduled(playerMic.recording.audioClip, 
                                               AudioSettings.dspTime, 
                                               playerMic.recording.length);
    }

	// Update is called once per frame
	void Update() 
    {
        // If audio is playing or paused, we don't advance in the script.
        if (narratorAudioSourceWrapper.currentState != AudioState.NotPlaying || 
                playerAudioSourceWrapper.currentState != AudioState.NotPlaying) 
            return;

        // Note: As you can see, expected order of array elements is VERY hard-coded. Keep in
        // mind when modifying.
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
