using UnityEngine;

using Speaker = ScriptHolder.Speaker;

/* ObjectFocus.cs
 * The central logic script for initiating conversation with an NPC. Runs the dialogue scripts.
 */
public class ObjectFocus : MonoBehaviour, NarratorCallback {
    int scriptLine = 0;
    public static bool PLAY_RECORDING = false;

    Dialogue dialogue;
    ScriptHolder scriptHolder;
    Narrator narrator;
    PlayerMicControls playerMicControls;
    PlayerDialogueControls playerDialogueControls;
    AudioSource recordingsAudio;
    bool recordingStarted = false;

	void Start () {
        dialogue = GetComponent<Dialogue>();
        scriptHolder = GetComponent<ScriptHolder>();
        narrator = GetComponent<Narrator>();

        GameObject player = GameObject.Find(PlayerLoader.playerPath());
        playerMicControls = player.GetComponent<PlayerMicControls>();
        playerDialogueControls = player.GetComponent<PlayerDialogueControls>();
        recordingsAudio = player.transform.Find("Recordings Audio").GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering trigger for " + gameObject.name);
        if (scriptLine < scriptHolder.script.Length)
        {
            if (scriptHolder.script[scriptLine] == Speaker.Narrator)
            {
                narrator.StartNarrator(this);
            }
            else if (scriptHolder.script[scriptLine] == Speaker.Dialogue)
            {
                dialogue.StartDialogue(this);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        dialogue.HideDialogue();
        if (playerMicControls.isRecording())
            playerMicControls.StopRecording();
    }

    private void nextScriptLine()
    {
        if (scriptLine++ >= scriptHolder.script.Length) return;

        switch (scriptHolder.script[scriptLine])
        {
            case Speaker.Narrator:
                narrator.StartNarrator(this);
                break;
            case Speaker.Dialogue:
                dialogue.StartDialogue(this);
                break;
            case Speaker.Player:
                if (playerMicControls.isRecording()) playerMicControls.StopRecording();
                break;
            case Speaker.Pause:
                break;
        }
    }

    void Update()
    {
        if (scriptLine >= scriptHolder.script.Length)
            return;

        switch(scriptHolder.script[scriptLine])
        {
            case Speaker.Player:
                if (playerMicControls.recordButtonPressed()) {
                    if (!recordingStarted) 
                    {
                        recordingStarted = true;
                    }
                    else
                    {
                        recordingStarted = false;
                        nextScriptLine();
                    }
                }
                break;
            case Speaker.Pause:
                if (playerDialogueControls.playRecordingButtonPressed())
                {
                    recordingsAudio.clip = playerMicControls.recording.audioClip;
                    PLAY_RECORDING = true;
                    recordingsAudio.Play();
                }
                else if (playerDialogueControls.nextDialogueButtonPressed())
                {
                    if (recordingsAudio.isPlaying)
                        recordingsAudio.Stop();
                    nextScriptLine();
                }
                break;
            default:
                break;
        }
    }

    // When Narrator finishes
    public void OnClipFinished()
    {
        nextScriptLine();
    }
}
