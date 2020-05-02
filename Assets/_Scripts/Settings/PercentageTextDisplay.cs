using UnityEngine;
using UnityEngine.UI;
using TMPro;

class PercentageTextDisplay : MonoBehaviour
{
    TMP_Text textbox;

    void Awake()
    {
        textbox = GetComponent<TMP_Text>();
        float startingPercentage = transform.parent.GetComponentInChildren<Slider>().value;
        textbox.text = string.Format("{0:F0}%", startingPercentage * 100f);
    }

    public void updateText(float newPercentage)
    {
        if (textbox != null) textbox.text = string.Format("{0:F0}%", newPercentage * 100f);
    }
}
