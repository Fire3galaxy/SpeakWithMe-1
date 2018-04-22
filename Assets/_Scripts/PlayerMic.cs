using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For the Player/Narrator to know when this recording is done
public interface MicReceiver {
    void onFinishedRecording();
}

// Stores the audio clip for a single object, starts/stops recording
public class PlayerMic : MonoBehaviour {
    public string micName;
    public AudioClip recording;
    public int maxDuration = 10;
    public bool isRecording = false;

    private MicReceiver caller;

	// Use this for initialization
	void Start () {
        micName = Microphone.devices[0];
	}

	// Update is called once per frame
	void Update () {
        // Let caller know recording is done.
        if (isRecording) {
            if (!Microphone.IsRecording(micName)) {
                isRecording = false;
                caller.onFinishedRecording();
                caller = null;
            }
        }
	}
    
    public void StartRecording(MicReceiver receiver) {
        isRecording = true;
        caller = receiver;

        // 10 second recording
        recording = Microphone.Start(micName, true, maxDuration, 44100);
    }

    // Stop recording manually if needed
    public void StopRecording() {
        Microphone.End(micName);
        isRecording = false;
        caller.onFinishedRecording();
        caller = null;
    }
}
