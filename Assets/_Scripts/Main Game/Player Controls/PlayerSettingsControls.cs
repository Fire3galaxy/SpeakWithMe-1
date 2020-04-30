using UnityEngine;

class PlayerSettingsControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Hack to alternate between Oculus VR (1) and No VR (2)
            PlayerPrefs.SetInt(PlayStyleSettings.preferenceKey, 
                3 - PlayerPrefs.GetInt(PlayStyleSettings.preferenceKey, 1));
        }
    }
}