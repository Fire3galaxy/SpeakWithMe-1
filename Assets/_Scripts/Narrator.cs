using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NarratorCallback
{
    void OnClipFinished();
}

public class Narrator : MonoBehaviour {
    public AudioClip[] narratorClips;
    public int clipNum;
    public AudioSource PlayerAudio;
    public NarratorCallback scriptLogic;

    private float clipLength;
    private float timePassed;
    private bool isPlaying;

	// Use this for initialization
	void Start () {
        PlayerAudio = GameObject.Find("/OVRPlayerController").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlaying)
        {
            timePassed += Time.deltaTime;

            if (timePassed >= clipLength)
            {
                if (!PlayerAudio.isPlaying)
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
        PlayerAudio.clip = narratorClips[clipNum];
        clipLength = narratorClips[clipNum].length;
        PlayerAudio.Play();
        timePassed = 0.0f;
        isPlaying = true;
    }
}
