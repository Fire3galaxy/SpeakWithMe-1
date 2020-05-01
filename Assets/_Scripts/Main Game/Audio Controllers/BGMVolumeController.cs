class BGMVolumeController : VolumeController
{
    override protected string getPlayerPrefsKey() {
        return BGMVolumeSettings.playerPrefsKey;
    }
}