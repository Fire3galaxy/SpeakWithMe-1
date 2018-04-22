using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingIndicator : MonoBehaviour {
    Text text;
    AudioSource PlayerAudio;

    private void Start()
    {
        text = GetComponent<Text>();
        PlayerAudio = GameObject.Find("/OVRPlayerController").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		if (PlayerMic.RECORDING)
        {
            text.color = new Color(1, 0, 0);
            text.text = "REC";
        } 

        //else if (ObjectFocus.PLAY_RECORDING)
        //{
        //    if (PlayerAudio.isPlaying)
        //    {
        //        text.color = new Color(0, 1, 0);
        //        text.text = "Listening";
        //    } else
        //    {
        //        text.text = "";
        //        ObjectFocus.PLAY_RECORDING = false;
        //    }
        //}

        else
        {
            text.text = "";
        }
    }
}
