using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFocus : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched this");
        text.text = "蘋果 Píngguǒ";
        Debug.Log(other.gameObject.GetInstanceID());
        if (other.gameObject.GetInstanceID() == 13804)
            Debug.Log("me");
    }
}
