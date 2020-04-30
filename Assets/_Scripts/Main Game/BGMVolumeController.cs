using UnityEngine;

class BGMVolumeController : VolumeController
{
    override internal string getPlayerPrefsKey() {
        return BGMVolumeSettings.playerPrefsKey;
    }
}