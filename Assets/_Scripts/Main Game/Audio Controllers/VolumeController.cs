using UnityEngine;

/* VolumeController.cs
 * Keeps volume for audioSource updated to set value in PlayerPrefs. Player prefs
 * key defined by child.
 */
abstract class VolumeController : MonoBehaviour
{
    AudioSource audioSource;
    bool loggedErrorOnce = false;

    virtual protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Update volume to latest preference
        if (!PlayerPrefs.HasKey(getPlayerPrefsKey()))
        {
            if (!loggedErrorOnce) 
            {
                Debug.LogError("No volume saved for " + getPlayerPrefsKey());
                loggedErrorOnce = true;
            }
            return;
        }

        audioSource.volume = PlayerPrefs.GetFloat(getPlayerPrefsKey());
    }

    abstract protected string getPlayerPrefsKey();
}