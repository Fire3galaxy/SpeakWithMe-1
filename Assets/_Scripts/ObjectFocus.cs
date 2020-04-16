using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFocus : MonoBehaviour, NarratorCallback, MicReceiver {
    int scriptLine = 0;
    public static bool PLAY_RECORDING = false;

    Dialogue dialogue;
    ScriptHolder scriptHolder;
    Narrator narrator;
    PlayerMic playerMic;
    AudioSource PlayerAudio;
    bool recordingStarted = false;
    bool inTrigger = false;

	// Use this for initialization
	void Start () {
        dialogue = GetComponent<Dialogue>();
        scriptHolder = GetComponent<ScriptHolder>();
        narrator = GetComponent<Narrator>();
        playerMic = GetComponent<PlayerMic>();
        PlayerAudio = GameObject.Find("/OVRPlayerController").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        if (scriptLine < scriptHolder.script.Length)
        {
            if (scriptHolder.script[scriptLine] == ScriptHolder.Narrator)
            {
                narrator.StartNarrator(this);
            }
            else if (scriptHolder.script[scriptLine] == ScriptHolder.Dialogue)
            {
                dialogue.StartDialogue(this);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        dialogue.HideDialogue();
        inTrigger = false;
        if (playerMic.isRecording)
            playerMic.StopRecording();
    }

    private void nextScriptLine()
    {
        if (scriptLine++ >= scriptHolder.script.Length) return;

        switch (scriptHolder.script[scriptLine])
        {
            case ScriptHolder.Narrator:
                narrator.StartNarrator(this);
                break;
            case ScriptHolder.Dialogue:
                dialogue.StartDialogue(this);
                break;
            case ScriptHolder.Player:
                if (playerMic.isRecording) playerMic.StopRecording();
                break;
            case ScriptHolder.Pause:
                break;
        }
    }

    private void Update()
    {
        if (scriptLine >= scriptHolder.script.Length)
            return;

        switch(scriptHolder.script[scriptLine])
        {
            case ScriptHolder.Player:
                if (playerMic.recordButtonPressed) {
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
            case ScriptHolder.Pause:
                if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Remote) || Input.GetKeyDown("l"))
                {
                    PlayerAudio.clip = playerMic.recording;
                    PLAY_RECORDING = true;
                    PlayerAudio.Play();
                }
                else if (OVRInput.GetDown(OVRInput.Button.DpadRight, OVRInput.Controller.Remote) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (PlayerAudio.isPlaying)
                        PlayerAudio.Stop();
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
        Debug.Log("Finish clip");
        nextScriptLine();
    }
}
