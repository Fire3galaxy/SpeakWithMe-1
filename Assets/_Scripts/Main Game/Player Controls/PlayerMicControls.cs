using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// Stores the audio clip for a single object, starts/stops recording
// Is not responsible for playing the audio clip, only recording it.
class PlayerMicControls : PlayPauseBehaviour {
    public int maxDuration = 10;

    SteamVR_Action_Boolean steamRecordAction;
    string micName;
    int sampleRate = 44100;
    float elapsedTime = 0f;
    float refreshMicListFrequency = 2f;

    public AudioClipWrapper recording {get; private set;}

	void Start() {
        steamRecordAction = SteamVR_Actions.demoControls_Record;
        // Throws an error if playerPref isn't set. Should be guaranteed by MicrophoneSettings
        micName = PlayerPrefs.GetString(MicrophoneSettings.preferenceKey);
        handleMicChangeUpdate(true);
	}

	override protected void UpdatePlay() {
        handleMicChangeUpdate(false);
        handleRecordButtonUpdate();
	}

    override protected void UpdatePause()
    {
        if (isRecording())
        {
            StopRecording();
        }

        // Keep micName updated while settings is open.
        micName = PlayerPrefs.GetString(MicrophoneSettings.preferenceKey);
    }

    void handleMicChangeUpdate(bool checkImmediately)
    {
        elapsedTime += Time.deltaTime;
        if (!checkImmediately && elapsedTime <= refreshMicListFrequency) return;
        elapsedTime = 0f;

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone available. Sending back to settings.");
            SceneManager.LoadScene(0);
            return;
        }

        // Make sure preferred mic is in current list of microphone devices
        micName = PlayerPrefs.GetString(MicrophoneSettings.preferenceKey);
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            if (Microphone.devices[i] == micName)
            {
                return;
            }
        }

        // Preferred mic isn't available now. Go with first available microphone and tell user.
        // FIXME: GUI way to tell user which mic we're using.
        Debug.LogWarning("Preferred microphone " + micName + " not available. " +
                         "Going with first device " + Microphone.devices[0]);
        micName = Microphone.devices[0];
        PlayerPrefs.SetString(MicrophoneSettings.preferenceKey, micName);
    }

    void handleRecordButtonUpdate()
    {
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
               Input.GetKeyDown(KeyCode.R) ||
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
