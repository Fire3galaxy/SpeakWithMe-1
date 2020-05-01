class BGMVolumeSettings : VolumeSettings
{
    static public string playerPrefsKey = "bgmVolume";

    override protected string getPlayerPrefsKey() {
        return playerPrefsKey;
    }
}