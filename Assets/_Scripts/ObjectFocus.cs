using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFocus : MonoBehaviour {
    public Dialogue dialogue;

	// Use this for initialization
	void Start () {
        dialogue = GetComponent<Dialogue>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched this");
        Debug.Log(other.gameObject.GetInstanceID());
        dialogue.StartDialogue();
        Debug.Log("After this");
    }

    private void OnTriggerExit(Collider other) {
        dialogue.HideDialogue();
    }
}
