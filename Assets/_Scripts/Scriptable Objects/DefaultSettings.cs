using UnityEngine;
using HeadsetType = PlayerLoader.HeadsetType;

[CreateAssetMenu(fileName = "SettingsData", menuName = "ScriptableObjects/DefaultSettings", order = 1)]
class DefaultSettings : ScriptableObject
{
    public HeadsetType defaultPlaySetting = HeadsetType.OpenVR;
}