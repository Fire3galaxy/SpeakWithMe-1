using UnityEngine;
using UnityEngine.UI;

// Parent of all specific text classes
public abstract class Dialogue : MonoBehaviour {
    public AudioClip[] clips;
    
    // Must be set by children
    protected abstract string[] scripts { get; }

    int scriptLine = 0;
    AudioSource narratorAudio;
    VolumeController narratorVolumeController;
    Text textContainer;
    OnDialogueCompleteListener caller;
    bool isPlaying = false;
    bool endOfDialogue = false;

    void Start()
    {
        GameObject narratorAudioObject = GameObject.Find(PlayerLoader.playerPath() + 
                                                         "/Narrator Audio");
        narratorAudio = narratorAudioObject.GetComponent<AudioSource>();
        narratorVolumeController = narratorAudioObject.GetComponent<VolumeController>();
        textContainer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if ((isPlaying && !narratorAudio.isPlaying && !narratorVolumeController.pausedByPlayer) || endOfDialogue)
        {
            scriptLine++;
            isPlaying = false;
            endOfDialogue = false;
            caller.onDialogueClipCompleted();
        }
    }

    public void StartDialogue(OnDialogueCompleteListener caller) {
        if (scriptLine >= scripts.Length)
        {
            EndDialogue();
            Debug.LogError("Should not see this");
        }

        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);

        isPlaying = true;
        narratorAudio.clip = clips[scriptLine];
        narratorAudio.Play();

        this.caller = caller;
    }

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
    }

    public void EndDialogue() {
        HideDialogue();
        scriptLine = 0;
        endOfDialogue = true;
    }

    public interface OnDialogueCompleteListener
    {
        void onDialogueClipCompleted();
    }
}
