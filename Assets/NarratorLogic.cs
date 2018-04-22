using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Starts recordings by calling PlayerMics on each object we care about. 
// Retrieves references to recordings stored in PlayerMics and gives it to AudioSource component.
public class NarratorLogic : MonoBehaviour, MicReceiver {
    public bool waitingForRecording = false;

    // FIXME: In future version of app, this is replaced by dynamically grabbing reference
    // of the object the user is looking at.
    public GameObject[] objects = new GameObject[3];

    private AudioSource audioSource;
    private PlayerMic currMic;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!waitingForRecording) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Debug.Log("Started record for object 1");

                currMic = objects[0].GetComponent<PlayerMic>();
                currMic.StartRecording(this);
                waitingForRecording = true;
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                audioSource.Play();
            }
        }
	}

    public void onFinishedRecording() {
        audioSource.clip = currMic.recording;
        waitingForRecording = false;
        Debug.Log("Recording is ready");
    }
}
