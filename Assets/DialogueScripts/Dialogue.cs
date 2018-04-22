using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Parent of all specific text classes
public abstract class Dialogue : MonoBehaviour {
    public Text textContainer;
    public int scriptLine = 0;
    public string[] scripts;
    public bool isActive = false;

    public void StartDialogue() {
        Debug.Log("HERE");
        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);
        isActive = true;
    }

    public void NextLine() {
        scriptLine++;
        if (scriptLine < scripts.Length)
            textContainer.text = scripts[scriptLine];
        else
            EndDialogue();
    }

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
        isActive = false;
    }

    public void EndDialogue() {
        textContainer.gameObject.SetActive(false);
        scriptLine = 0;
        isActive = false;
    }

    private void Update() {
        if (isActive && 
                (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Remote) || 
                Input.GetKeyDown("space"))) {
            NextLine();
        }
    }
}
