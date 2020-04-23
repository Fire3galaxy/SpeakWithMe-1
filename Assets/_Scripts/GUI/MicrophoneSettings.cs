using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicrophoneSettings : MonoBehaviour
{
    void Start()
    {
        TMP_Dropdown dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.AddOptions(new List<string>(Microphone.devices));
        dropdownMenu.value = PlayerPrefs.GetInt("microphone", 0);
    }

    public void onMicrohphoneSettingChanged(int setValue)
    {
        // Index 0 is "Select an option"
        if (setValue != 0) 
        {
            Debug.Log(Microphone.devices[setValue - 1]);
            PlayerPrefs.SetInt("microphone", setValue - 1);
        }
    }
}
