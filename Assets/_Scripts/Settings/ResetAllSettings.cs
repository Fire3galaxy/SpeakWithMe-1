using UnityEngine;

class ResetAllSettings : MonoBehaviour
{
    Transform canvas;

    void Start()
    {
        canvas = transform.Find("/Canvas");
    }

    public void onClick()
    {
        foreach (Transform child in canvas)
        {
            if (child.name.StartsWith("Settings"))
            {
                child.GetComponentInChildren<SettingsInterface>().resetSettings();
            }
        }
        PlayerPrefs.Save();
    }
}