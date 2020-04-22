using UnityEngine;
using Valve.VR;

public class DebugMic : MonoBehaviour
{
    public int maxDuration = 10;
    string micName;

    public AudioClip recording {get; private set;}
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        micName = Microphone.devices[0];
        audioSource = GetComponent<AudioSource>();

        foreach (string device in Microphone.devices)
        {
            Debug.Log("device name = " + device);
        }
        Debug.Log("Microphone is " + micName);
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            if (Microphone.IsRecording(micName))
            {
                StopRecording(); 
            }
            else
            {
                StartRecording();
            }
        }

        if (Input.GetKeyDown("p"))
        {
            audioSource.clip = recording;
            audioSource.Play();           
        }
	}

    public void StartRecording() {
        recording = Microphone.Start(micName, true, maxDuration, 44100);
        Debug.Log("Recording started!");
    }

    // Recordings have maximum durations, but we can end them early too.
    public void StopRecording() {
        Microphone.End(micName);
        Debug.Log("Recording ended! Length of recording was " + recording.length);
    }
}