using UnityEngine;
using UnityEngine.UI;

/* FadeInOut.cs
 * For Image textures in GUI. Adjusts alpha between max and min.
 */
public class FadeInOut : MonoBehaviour
{
    [Range(0f, 1f)]
    public float maxAlpha = 1f;
    [Range(0f, 1f)]
    public float minAlpha = 0f;
    [Range(0f, 10f)]
    public float period = 3f;

    Image image;
    float elapsedTime = 0f;
    float centerAlpha;
    float halfAlphaRange;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        centerAlpha = (maxAlpha + minAlpha) / 2f;
        halfAlphaRange = (maxAlpha - minAlpha) / 2f;
        
        Color startColor = image.color;
        startColor.a = maxAlpha;
        image.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = (elapsedTime + Time.deltaTime) % period;

        Color newColor = image.color;
        newColor.a = centerAlpha + halfAlphaRange * Mathf.Cos(2f * Mathf.PI * elapsedTime / period);
        // Debug.Log(newColor.a);
        image.color = newColor;
    }
}
