using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For the Player/Narrator to know when this recording is done
public interface MicReceiver {
    void onFinishedRecording();
}

// Stores the audio clip for a single object, starts/stops recording
public class PlayerMic : MonoBehaviour {
    public GameObject introRecordingIcon;
    public AudioClip recording;
    public int maxDuration = 10;
    public bool isRecording = false;

    // "Powered on" concept: don't start recording unless mic is turned on
    [HideInInspector]
    public bool isPoweredOn = false;

    public static bool RECORDING = false;

    private string micName;
    private MicReceiver caller;

	// Use this for initialization
	void Start () {
        micName = Microphone.devices[0];
	}

	// Update is called once per frame
	void Update () {
        // Start recording (if desired)
        // FIXME: MONDAY, continue refactoring PlayerMic so input is listened to solely from here.
        if (isPoweredOn)
        {
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Remote) ||
                    Input.GetKeyDown("space"))
            {
                StartRecording();
            }
            if (!isRecording) {
                
            }
        }

        // Let caller know recording is done.
        if (isRecording) {
            if (!Microphone.IsRecording(micName)) {
                isRecording = false;
                RECORDING = false;
                caller.onFinishedRecording();
                caller = null;
            }
        }
	}
    
    public void StartRecording(MicReceiver receiver) {
        isRecording = true;
        RECORDING = true;
        caller = receiver;

        // 10 second recording
        recording = Microphone.Start(micName, true, maxDuration, 44100);
    }

    // Stop recording manually if needed
    public void StopRecording() {
        Microphone.End(micName);
        isRecording = false;
        RECORDING = false;
        caller.onFinishedRecording();
        caller = null;
    }
}
