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
    bool waiting = false;
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
        scriptLine++;
        //narrator.StartNarrator(this);

        if (scriptLine < scriptHolder.script.Length)
        {
            switch (scriptHolder.script[scriptLine])
            {
                case ScriptHolder.Narrator:
                    narrator.StartNarrator(this);
                    break;
                case ScriptHolder.Dialogue:
                    dialogue.StartDialogue(this);
                    break;
                case ScriptHolder.Player:
                    Debug.Log("In Player");
                    waiting = true;
                    break;
                case ScriptHolder.Pause:
                    Debug.Log("In Pause");
                    break;
            }
        }
        Debug.Log("Test");
    }

    private void Update()
    {
        if (waiting)
        {
            Debug.Log("In waiting if statement");
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Remote) ||
                    Input.GetKeyDown("space"))
            {
                if (!playerMic.isRecording)
                {
                    playerMic.StartRecording(this);
                }
                else
                {
                    playerMic.StopRecording();
                }
            }

            else if (Input.GetKeyDown("s"))
            {
                onFinishedRecording();
            }
        }

        if (scriptLine < scriptHolder.script.Length)
        {
            if (scriptHolder.script[scriptLine] == ScriptHolder.Pause)
            {
                Debug.Log("Inside Update Pause");
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
            }
        }
    }

    // When Narrator finishes
    public void OnClipFinished()
    {
        Debug.Log("Finish clip");
        nextScriptLine();
    }

    public void onFinishedRecording()
    {
        Debug.Log("Finish recording");
        waiting = false;
        nextScriptLine();
    }
}
