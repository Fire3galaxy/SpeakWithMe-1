using UnityEngine;

using AudioState = AudioSourceWrapper.AudioState;

// Plays voice clip from us language developers/teachers
class Narration : MonoBehaviour {
    public AudioClip[] audioClips;

    protected int scriptLine = 0;
    OnNarrationCompleteListener scriptLogic;
    AudioSourceWrapper narratorAudioSourceWrapper;
    bool isPlaying = false;

	virtual protected void Start () {
        narratorAudioSourceWrapper = GameObject.Find(PlayerLoader.playerPath() + "/Narrator Audio")
                                        .GetComponent<AudioSourceWrapper>();
	}

    virtual protected void Update()
    {
        // A clip starts playing when StartNarrator() is called. This code waits for
        // the clip to ends, then notifies the callee we're done.
		if (isPlaying && narratorAudioSourceWrapper.currentState == AudioState.NotPlaying)
        {
            isPlaying = false;
            scriptLine++;
            scriptLogic.onNarrationComplete();
        }
    }

    virtual public void StartNarration(OnNarrationCompleteListener caller)
    {
        scriptLogic = caller;
        narratorAudioSourceWrapper.Play(audioClips[scriptLine]);
        isPlaying = true;
    }

    public interface OnNarrationCompleteListener
    {
        void onNarrationComplete();
    }
}
