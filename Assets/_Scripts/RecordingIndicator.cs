using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingIndicator : MonoBehaviour {
    Text text;
    AudioSource playerAudio;
    PlayerMicControls playerMic;

    private void Start()
    {
        text = GetComponent<Text>();
        text.color = new Color(1, 0, 0);
        GameObject player = GameObject.Find("/OpenVR Player");
        playerAudio = player.GetComponent<AudioSource>();
        playerMic = player.GetComponent<PlayerMicControls>();
    }

    // Update is called once per frame
    void Update () {
		if (playerMic.isRecording())
        {
            text.text = "REC";
        }
        else
        {
            text.text = "";
        }
    }
}
