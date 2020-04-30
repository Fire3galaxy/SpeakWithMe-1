using UnityEngine;

public class BobGently : MonoBehaviour
{
    public float bobDistance = .05f;
    public float bobPeriod = 3f;

    float elapsedTime = 0.0f;
    float originalY;

    void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > bobPeriod) elapsedTime = 0f;
        transform.position = new Vector3(transform.position.x, 
                                         originalY + bobDistance * 
                                            Mathf.Sin(elapsedTime / bobPeriod * 2f * Mathf.PI),
                                         transform.position.z);
    }
}
