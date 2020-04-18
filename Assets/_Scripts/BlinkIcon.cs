using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkIcon : MonoBehaviour {
    public float BlinkRate = 2f;
    MeshRenderer meshRenderer;
    float timePassed = 0f;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        if (timePassed >= BlinkRate)
        {
            meshRenderer.enabled = !meshRenderer.enabled;
            timePassed = 0f;
        }
	}

    void OnDisable()
    {
        meshRenderer.enabled = true;
        Debug.Log("OnDisable: BlinkIcon for " + gameObject.name);
    }
}
