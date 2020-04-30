class BGMVolumeSettings : VolumeSettings
{
    static public string playerPrefsKey = "bgmVolume";

    override internal string getPlayerPrefsKey() {
        return playerPrefsKey;
    }
}