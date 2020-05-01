using UnityEngine;

/* RecordingsVolumeController.cs
 * This class is only created to allow audio playback for recordings to be paused.
 * I have no plans at this time to make recordings volume configurable.
 */
class RecordingsVolumeController : VolumeController
{
    // A placeholder setting in PlayerPrefs so VolumeController will have a preference
    // to retrieve
    string placeholderKey = "RecordingsVolume_placeholder";

    override protected void Start()
    {
        base.Start();
        PlayerPrefs.SetFloat(placeholderKey, 1f); // Hardcoded to highest volume
    }

    // Recordings audio is not configurable. Only goes to 1.
    override protected string getPlayerPrefsKey() {
        return placeholderKey;
    }
}