using UnityEngine;

class PlayerSettingsControls : MonoBehaviour
{
    public static bool paused = false;

    GameObject settingsCanvas;

    void Start()
    {
        settingsCanvas = transform.Find("Settings Canvas").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // // Hack to alternate between Oculus VR (1) and No VR (2)
            // PlayerPrefs.SetInt(PlayStyleSettings.preferenceKey, 
            //     3 - PlayerPrefs.GetInt(PlayStyleSettings.preferenceKey, 1));

            // Testing pausing over entire game
            paused = !paused;
            settingsCanvas.SetActive(paused);
        }
    }
}