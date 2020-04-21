using UnityEngine;
using UnityEngine.UI;

// Parent of all specific text classes
public abstract class Dialogue : MonoBehaviour {
    public AudioClip[] clips;
    
    // Must be set by children
    protected abstract string[] scripts { get; }

    private int scriptLine = 0;
    private AudioSource narratorAudio;
    private Text textContainer;
    private NarratorCallback caller;
    private bool isPlaying = false;
    private bool endOfDialogue = false;

    // Intentionally set as private to prevent overriding by children
    private void Start()
    {
        narratorAudio = GameObject.Find(PlayerLoader.playerPath() + "/Narrator Audio")
                                  .GetComponent<AudioSource>();
        textContainer = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if ((isPlaying && !narratorAudio.isPlaying) || endOfDialogue)
        {
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
            Debug.LogError("Should not see this");
        }

        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);

        Debug.Log("Here");

        isPlaying = true;
        narratorAudio.clip = clips[scriptLine];
        narratorAudio.Play();

        Debug.Log("Player audio: " + narratorAudio.isPlaying);

        this.caller = caller;
    }

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
    }

    public void EndDialogue() {
        textContainer.gameObject.SetActive(false);
        scriptLine = 0;
        endOfDialogue = true;
    }
}
