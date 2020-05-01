using UnityEngine;

public class DebugAudioMonitor : MonoBehaviour
{
    // Debug button to quickly pause/unpause.
    public bool pauseAudio = false;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAudio) audioSource.Pause();
        else audioSource.UnPause();
        Debug.Log(audioSource.time);
    }
}
