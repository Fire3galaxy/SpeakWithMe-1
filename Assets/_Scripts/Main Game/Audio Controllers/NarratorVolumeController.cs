class NarratorVolumeController : VolumeController
{
    override protected string getPlayerPrefsKey() {
        return NarratorVolumeSettings.playerPrefsKey;
    }
}