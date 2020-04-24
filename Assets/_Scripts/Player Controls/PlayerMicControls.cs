using UnityEngine;
using Valve.VR;

// Stores the audio clip for a single object, starts/stops recording
// Is not responsible for playing the audio clip, only recording it.
public class PlayerMicControls : MonoBehaviour {
    public int maxDuration = 10;
    
    SteamVR_Action_Boolean steamRecordAction;
    string micName;
    int sampleRate = 44100;

    public AudioClipWrapper recording {get; private set;}

	void Start () {
        steamRecordAction = SteamVR_Actions.demoControls_Record;

        int micIndex = MicrophoneSettings.getPreferredDeviceIndex();
        if (micIndex == -1) return; // FIXME: User error on screen later for this case. Shouldn't happen though.

        micName = Microphone.devices[micIndex];
	}

	// Update is called once per frame
	void Update () {
        // Below code handles pressing the record button
        if (!recordButtonPressed()) return;

        if (isRecording())
        {
            StopRecording(); 
        }
        else
        {
            StartRecording();
        }
	}
    
    public void StartRecording() {
        recording = new AudioClipWrapper(Microphone.Start(micName, true, maxDuration, sampleRate));
        Debug.Log("Recording started!");
    }

    // Recordings have maximum durations, but we can end them early too.
    public void StopRecording() {
        // Position is in number of samples so we convert to seconds.
        recording.length = Microphone.GetPosition(micName) / (float) sampleRate;
        Microphone.End(micName);
        Debug.Log("Recording ended! Length of recording was " + recording.length);
    }

    public bool isRecording()
    {
        return Microphone.IsRecording(micName);
    }

    public bool recordButtonPressed() {
        return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Remote) ||
               Input.GetKeyDown("space") ||
               steamRecordAction.stateDown;
    }

    public class AudioClipWrapper
    {
        public AudioClip audioClip;
        public float length;
        public AudioClipWrapper(AudioClip audioClip)
        {
            this.audioClip = audioClip;
        }
    }
}
