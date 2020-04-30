using UnityEngine;

public interface NarratorCallback
{
    void OnClipFinished();
}

public class Narrator : MonoBehaviour {
    public AudioClip[] narratorClips;

    private int clipNum;
    private NarratorCallback scriptLogic;
    private AudioSource narratorAudio;
    private float clipLength;
    private float timePassed;
    private bool isPlaying;

	// Use this for initialization
	void Start () {
        narratorAudio = GameObject.Find(PlayerLoader.playerPath() + "/Narrator Audio")
                                  .GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlaying)
        {
            timePassed += Time.deltaTime;

            if (timePassed >= clipLength)
            {
                if (!narratorAudio.isPlaying)
                {
                    isPlaying = false;
                    clipNum++;
                    Debug.Log("clipNum" + clipNum);
                    scriptLogic.OnClipFinished();
                }
            }

        }
	}

    public void StartNarrator(NarratorCallback caller)
    {
        scriptLogic = caller;
        narratorAudio.clip = narratorClips[clipNum];
        clipLength = narratorClips[clipNum].length;
        narratorAudio.Play();
        timePassed = 0.0f;
        isPlaying = true;
    }
}
