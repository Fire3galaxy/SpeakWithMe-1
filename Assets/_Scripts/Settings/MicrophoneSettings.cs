using System.Collections.Generic;
using UnityEngine;
using TMPro;

class MicrophoneSettings : MonoBehaviour, SettingsInterface
{
    public static string preferenceKey = "microphone";

    TMP_Dropdown dropdownMenu;
    SortedSet<string> cachedMicDevices = new SortedSet<string>();
    readonly List<string> defaultDropdownValues = new List<string>(new string[]{"No Microphone Detected!"});
    float elapsedTime = 0f;
    float updateMenuFrequency = 1f;

    // Initialize values for dropdown menu with microphone devices or error message
    void Start()
    {
        dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.ClearOptions();

        SortedSet<string> currentSet = new SortedSet<string>(Microphone.devices);
        updateDropdownMenuOptions(currentSet);
    }

    // Once per second, update dropdown menu based on available microphone device list changes
    void Update()
    {
        // Don't check mic array every frame. Check based on set update frequecy.
        elapsedTime += Time.deltaTime;
        if (elapsedTime < updateMenuFrequency) return;

        // Reset time
        elapsedTime = 0f;

        // Determine if latest list of devices matches the previous.
        SortedSet<string> latestSet = new SortedSet<string>(Microphone.devices);
        if (!latestSet.SetEquals(cachedMicDevices)) updateDropdownMenuOptions(latestSet);
    }

    public void onValueChanged(int value)
    {
        if (value == 0) return;
        PlayerPrefs.SetString(preferenceKey, Microphone.devices[value]);
    }

    public void resetSettings()
    {
        PlayerPrefs.DeleteKey(preferenceKey);
        dropdownMenu.value = 0;
    }

    void updateDropdownMenuOptions(SortedSet<string> latestSet)
    {
        // Device list has changed. Update dropdown menu to newest list of microphones.
        dropdownMenu.ClearOptions();
        cachedMicDevices = latestSet;
        
        // The cached list was definitely not empty, but the latest list is. Update and disable dropdown.
        if (latestSet.Count == 0)
        {
            dropdownMenu.AddOptions(defaultDropdownValues);
            dropdownMenu.value = 0;
            dropdownMenu.interactable = false;
            return;
        }

        // The latest list contains ready devices. Update options.
        dropdownMenu.AddOptions(new List<string>(cachedMicDevices));
        dropdownMenu.interactable = true;

        string savedMicName = PlayerPrefs.GetString(preferenceKey, "");
        int micIndex = -1;
        for (int i = 0; i < dropdownMenu.options.Count; i++)
        {
            if (dropdownMenu.options[i].text == savedMicName)
            {
                micIndex = i;
            }
        }

        // Edge failure case: preferred mic isn't available. Go with 0 as default.
        if (micIndex < 0)
        {
            // Note: Changing preference here to follow the philosophy of "Let the
            // player start the game asap". They likely only have one mic plugged in
            // anyway.
            dropdownMenu.value = 0;
            PlayerPrefs.SetString(preferenceKey, dropdownMenu.options[0].text);
            return;
        }

        // Success case: devices available. Preferred mic is available.
        dropdownMenu.value = micIndex;
    }

    // Note: Settings should always be valid if microphone devices are available. Valid means
    // that PlayerPrefs is set to an existing microphone device. If no microphone devices are
    // available, then this setting is invalid and the user can't play the game.
    public bool validateSettings()
    {
        return dropdownMenu.interactable;
    }
}