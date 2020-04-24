using UnityEngine;
using UnityEngine.SceneManagement;

class SettingsToMainGame : MonoBehaviour
{
    Transform canvas;

    void Start()
    {
        canvas = transform.Find("/Canvas");
    }

    public void onClick()
    {
        bool allSettingsValid = true;

        foreach (Transform child in canvas)
        {
            if (child.name.StartsWith("Settings"))
            {
                bool valid = child.GetComponentInChildren<SettingsInterface>().validateSettings();
                allSettingsValid &= valid;

                if (!valid) Debug.Log(child.name + " is not valid");
            }
        }

        if (!allSettingsValid) {
            /* FIXME: Fill in error GUI message */
            Debug.LogWarning("Error: Not all settings are valid");
            return;
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}