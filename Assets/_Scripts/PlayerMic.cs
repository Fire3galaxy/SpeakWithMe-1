using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores the audio clip for a single object, starts/stops recording
// Is not responsible for playing the audio clip, only recording it.
public class PlayerMic : MonoBehaviour {
    public GameObject introRecordingIcon;
    public int maxDuration = 10;

    public AudioClip recording {get; private set;}
    public bool isRecording {
        get {
            return Microphone.IsRecording(micName);
        }
    }

    public bool recordButtonPressed {
        get {
            return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Remote) ||
                   Input.GetKeyDown("space");
        }
    }

    string micName;

	// Use this for initialization
	void Start () {
        micName = Microphone.devices[0];
	}

	// Update is called once per frame
	void Update () {
        if (recordButtonPressed)
        {
            Microphone.IsRecording(micName) ? StopRecording : StartRecording();
            introRecordingIcon.SetActive(Microphone.IsRecording(micName));
        }
	}
    
    public void StartRecording() {
        recording = Microphone.Start(micName, true, maxDuration, 44100);
    }

    // Recordings have maximum durations, but we can end them early too.
    public void StopRecording() {
        Microphone.End(micName);
    }
}
