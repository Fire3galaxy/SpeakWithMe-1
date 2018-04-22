using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Parent of all specific text classes
public abstract class Dialogue : MonoBehaviour {
    public Text textContainer;
    public int scriptLine = 0;
    public string[] scripts;
    public AudioClip[] clips;
    public AudioSource PlayerAudio;
    public NarratorCallback caller;

    private bool isPlaying = false;
    private bool endOfDialogue = false;

    public virtual void Start()
    {
        PlayerAudio = GameObject.Find("/OVRPlayerController").GetComponent<AudioSource>();
    }

    public void Update()
    {
        if ((isPlaying && !PlayerAudio.isPlaying) || endOfDialogue)
        {
            //NextLine();
            scriptLine++;
            Debug.Log("In here? " + scriptLine);
            isPlaying = false;
            endOfDialogue = false;
            caller.OnClipFinished();
        }
    }

    public void StartDialogue(NarratorCallback caller) {
        if (scriptLine >= scripts.Length)
        {
            EndDialogue();
            Debug.Log("Should not see this");
        }

        Debug.Log("script line for dialogue: " + scriptLine);
        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);

        Debug.Log("Here");

        isPlaying = true;
        PlayerAudio.clip = clips[scriptLine];
        PlayerAudio.Play();

        Debug.Log("Player audio: " + PlayerAudio.isPlaying);

        this.caller = caller;
    }

    //public void NextLine() {
    //    scriptLine++;
    //}

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
    }

    public void EndDialogue() {
        textContainer.gameObject.SetActive(false);
        scriptLine = 0;
        endOfDialogue = true;
    }
}
