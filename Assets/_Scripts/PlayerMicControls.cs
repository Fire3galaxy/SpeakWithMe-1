using UnityEngine;
using Valve.VR;

// Stores the audio clip for a single object, starts/stops recording
// Is not responsible for playing the audio clip, only recording it.
public class PlayerMicControls : MonoBehaviour {
    public GameObject introRecordingIcon;
    public int maxDuration = 10;
    
    // FIXME: Probably should turn this into something in a settings menu
    public int debugSelectMic = 0;

    SteamVR_Action_Boolean steamRecordAction;
    string micName;
    int sampleRate = 44100;

    public AudioClipWrapper recording {get; private set;}

	// Use this for initialization
	void Start () {
        micName = Microphone.devices[debugSelectMic];
        Debug.Log("Microphone is " + micName);
        steamRecordAction = SteamVR_Actions.demoControls_Record;
	}

	// Update is called once per frame
	void Update () {
        // Recording ends due to max duration rather than explicit button press
        if (introRecordingIcon.activeSelf && !isRecording())
        {
            introRecordingIcon.SetActive(false);
        }

        // Below code handles pressing the record button
        if (!recordButtonPressed()) return;

        if (isRecording())
        {
            StopRecording(); 
            introRecordingIcon.SetActive(false);
        }
        else
        {
            StartRecording();
            introRecordingIcon.SetActive(true);
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
