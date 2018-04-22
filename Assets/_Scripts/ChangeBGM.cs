using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour {
    public AudioSource BGSource;
    public AudioClip GangstaSong;
    public AudioClip RegularSong;

    private void OnTriggerEnter(Collider other)
    {
        BGSource.Stop();
        BGSource.clip = GangstaSong;
        BGSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        BGSource.Stop();
        BGSource.clip = RegularSong;
        BGSource.Play();
    }
}
