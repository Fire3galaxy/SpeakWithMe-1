using UnityEngine;
using TMPro;

class PlayStyleSettings : DropdownSettings
{
    TMP_Dropdown dropdownMenu;

    public static string preferenceKey = "playStyle";
    public override string playerPrefsKey => preferenceKey;

    void Start()
    {
        dropdownMenu = GetComponent<TMP_Dropdown>();
        setDropdownToPlayerPref(dropdownMenu);
    }

    protected override void setDropdownToPlayerPref(TMP_Dropdown dropdownMenu)
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey)) return;

        dropdownMenu.value = PlayerPrefs.GetInt(playerPrefsKey, -1) + 1;
    }

    public override void onValueChanged(int value)
    {
        // Index 0 is "Select an option..." option.
        if (value == 0) return;

        // playStyle indices correspond to PlayerLoader.HeadsetType
        PlayerPrefs.SetInt(playerPrefsKey, value - 1);
    }

    public override void resetSettings()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
        dropdownMenu.value = 0;
    }
}