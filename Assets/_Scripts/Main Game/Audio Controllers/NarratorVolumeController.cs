using UnityEngine;

class NarratorVolumeController : VolumeController
{
    override internal string getPlayerPrefsKey() {
        return NarratorVolumeSettings.playerPrefsKey;
    }
}