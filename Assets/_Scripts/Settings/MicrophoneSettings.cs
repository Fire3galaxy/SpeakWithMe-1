using System.Collections.Generic;
using UnityEngine;
using TMPro;

class MicrophoneSettings : MonoBehaviour, SettingsInterface
{
    public static string preferenceKey = "microphone";

    TMP_Dropdown dropdownMenu;
    HashSet<string> cachedMicDevices;
    readonly List<string> defaultDropdownValues = new List<string>(new string[]{"No Microphone Detected!"});
    float elapsedTime = 0f;
    float updateMenuFrequency = 1f;

    // Initialize values for dropdown menu with microphone devices or error message
    void Start()
    {
        dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.ClearOptions();

        // Failure case: no microphone available
        if (Microphone.devices.Length == 0) 
        {
            // Note: explicitly not clearing PlayerPrefs in this case. The player
            // may just have forgotten to plug his mic in.
            dropdownMenu.AddOptions(defaultDropdownValues);
            dropdownMenu.value = 0;
            dropdownMenu.interactable = false;
            return;
        }

        cachedMicDevices = new HashSet<string>(Microphone.devices);
        dropdownMenu.AddOptions(new List<string>(cachedMicDevices));
        dropdownMenu.interactable = true;

        int micIndex = playerPrefsMicrophoneNameToMicrophoneDevicesIndex();

        // Edge failure case: preferred mic isn't available. Go with 0 as default.
        if (micIndex < 0)
        {
            // Note: Changing preference here to follow the philosophy of "Let the
            // player start the game asap". They likely only have one mic plugged in
            // anyway.
            dropdownMenu.value = 0;
            PlayerPrefs.SetString(preferenceKey, dropdownMenu.options[0].text);
        }
        // Success case: devices available. Preferred mic is available.
        else
        {
            dropdownMenu.value = micIndex;
        }
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
        HashSet<string> latestSet = new HashSet<string>(Microphone.devices);
        if (latestSet.SetEquals(cachedMicDevices)) return; // No change.

        // Device list has changed. Update dropdown menu to newest list of microphones.
        string currentMic = dropdownMenu.options[dropdownMenu.value].text;
        dropdownMenu.ClearOptions();
        
        // The cached list was definitely not empty, but the latest list is. Update and disable dropdown.
        if (latestSet.Count == 0)
        {
            dropdownMenu.AddOptions(defaultDropdownValues);
            dropdownMenu.value = 0;
            dropdownMenu.interactable = false;
            return;
        }

        // The latest list contains ready devices. Update options.
        dropdownMenu.AddOptions(new List<string>(latestSet));
        dropdownMenu.interactable = true;
        cachedMicDevices = latestSet;

        // Find index for current microphone
        for (int i = 0; i < dropdownMenu.options.Count; i++)
        {
            if (dropdownMenu.options[i].text == currentMic)
            {
                dropdownMenu.value = i;
                break;
            }
        }
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

    // Name is horrifically long, but has a simple function and is a discrete task. Oh well.
    public static int playerPrefsMicrophoneNameToMicrophoneDevicesIndex()
    {
        // No preference
        if (!PlayerPrefs.HasKey(preferenceKey)) return -1;

        string micName = PlayerPrefs.GetString(preferenceKey);
        string[] devices = Microphone.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].Equals(micName))
            {
                return i;
            }
        }

        // Microphone not in current set of devices. The differentiating of error code
        // is admittedly entirely unnecessary, but it's no harm either.
        return -2;
    }

    public bool validateSettings()
    {
        return true;
    }
}