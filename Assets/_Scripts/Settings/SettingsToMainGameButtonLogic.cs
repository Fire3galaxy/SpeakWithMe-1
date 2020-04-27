using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class SettingsToMainGameButtonLogic : MonoBehaviour
{
    Button startButton;
    List<Transform> settingsTransforms;
    float elapsedTime = 0f;
    float interactivityRefreshRate = 1f;

    void Start()
    {
        startButton = GetComponent<Button>();
        settingsTransforms = new List<Transform>();

        Transform canvas = transform.Find("/Canvas");
        foreach (Transform child in canvas)
        {
            if (child.name.StartsWith("Settings")) settingsTransforms.Add(child);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < interactivityRefreshRate) return;

        elapsedTime = 0f;
        startButton.interactable = areSettingsValid();
    }

    public void onClick()
    {
        bool allSettingsValid = areSettingsValid();

        if (!allSettingsValid) {
            /* FIXME: Fill in error GUI message */
            Debug.LogWarning("Error: Not all settings are valid");
            return;
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    bool areSettingsValid()
    {
        bool allSettingsValid = true;

        foreach (Transform transform in settingsTransforms)
        {
            bool valid = transform.GetComponentInChildren<SettingsInterface>().validateSettings();
            allSettingsValid &= valid;

            // We could return as soon as we find the first invlaid setting, but this log is more valuable.
            if (!valid) Debug.LogWarning(transform.name + " is not valid");
        }

        return allSettingsValid;
    }
}