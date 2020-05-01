using UnityEngine;
using UnityEngine.UI;

using AudioState = AudioSourceWrapper.AudioState;

// Parent of all specific text classes
public abstract class Dialogue : MonoBehaviour {
    public AudioClip[] clips;
    
    int scriptLine = 0;
    AudioSourceWrapper narratorAudioSourceWrapper;
    Text textContainer;
    OnDialogueCompleteListener caller;
    bool isPlaying = false;

    // Must be set by children
    protected abstract string[] scripts { get; }

    void Start()
    {
        narratorAudioSourceWrapper = GameObject.Find(PlayerLoader.playerPath() + "/Narrator Audio")
                            .GetComponent<AudioSourceWrapper>();
        textContainer = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (isPlaying && narratorAudioSourceWrapper.currentState == AudioState.NotPlaying)
        {
            scriptLine++;
            isPlaying = false;
            caller.onDialogueClipCompleted();
        }
    }

    public void StartDialogue(OnDialogueCompleteListener caller) {
        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);

        isPlaying = true;
        narratorAudioSourceWrapper.Play(clips[scriptLine]);
        this.caller = caller;
    }

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
    }

    public interface OnDialogueCompleteListener
    {
        void onDialogueClipCompleted();
    }
}
