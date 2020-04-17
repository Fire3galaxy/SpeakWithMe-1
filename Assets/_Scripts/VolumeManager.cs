using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public enum AudioType {
        BGM, Narrator, Recording
    }

    public AudioType audioType;
    
    [Range(0.0f, 1.0f)]
    public float volume = .5f;

    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.volume != volume) audioSource.volume = volume;
    }
}
