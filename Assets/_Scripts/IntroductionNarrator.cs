using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Intro Narrator script (bind to main player or object with audio source)
// Plays on start of scene
public class IntroductionNarrator : MonoBehaviour, MicReceiver {
    public AudioClip[] clips;
    public GameObject IntroRecordingIcon;

    private AudioSource PlayerAudio;
    int currClip = 0;
    bool playedOnce = false;
    bool playedPlayer8 = false;
    bool finishedRecording = false;
    float timePassed = 0f;

    // Use this for initialization
    void Start () {
        PlayerAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!PlayerAudio.isPlaying)
        {
            if (currClip < clips.Length)
            {
                if (currClip == 2)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                    }
                    else if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || Input.GetKey(KeyCode.UpArrow))
                    {
                        currClip++;
                        playedOnce = false;
                    }
                }
                else if (currClip == 3)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                    }
                    else if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Remote) || Input.GetKey(KeyCode.DownArrow))
                    {
                        currClip++;
                        playedOnce = false;
                    }
                }
                else if (currClip == 4)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                    }
                    else if (OVRInput.Get(OVRInput.Button.DpadDown | OVRInput.Button.DpadUp, OVRInput.Controller.Remote) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
                    {
                        currClip++;
                        playedOnce = false;
                    }
                }
                else if (currClip == 5)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                    }
                    else if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Remote) || Input.GetKey("space"))
                    {
                        IntroRecordingIcon.SetActive(true);
                        GetComponent<PlayerMic>().StartRecording(this);
                        currClip++;
                        playedOnce = false;
                    }
                }
                else if (currClip == 6)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                        timePassed = 0.0f;
                    }
                    else
                    {
                        timePassed += Time.deltaTime;
                        if (timePassed >= 3.0f)
                        {
                            currClip++;
                            playedOnce = false;
                        }
                    }
                }
                // 19
                else if (currClip == 7)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                    }
                    else if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Remote) || Input.GetKey("space"))
                    {
                        IntroRecordingIcon.SetActive(false);
                        GetComponent<PlayerMic>().StopRecording();
                        currClip++;
                        playedOnce = false;
                    }
                }
                else if (currClip == 8)
                {
                    if (!playedOnce)
                    {
                        PlayerAudio.clip = clips[currClip];
                        PlayerAudio.Play();
                        playedOnce = true;
                        playedPlayer8 = false;
                    }
                    else if (!playedPlayer8)
                    {
                        PlayerAudio.clip = GetComponent<PlayerMic>().recording;
                        PlayerAudio.Play();
                        playedPlayer8 = true;
                    }
                    else if (!PlayerAudio.isPlaying)
                    {
                        currClip++;
                        playedOnce = false;
                    }
                }

                else
                {
                    PlayerAudio.clip = clips[currClip];
                    PlayerAudio.Play();
                    currClip++;
                }
            }
        }
	}

    public void onFinishedRecording()
    {
        finishedRecording = true;
    }
}
