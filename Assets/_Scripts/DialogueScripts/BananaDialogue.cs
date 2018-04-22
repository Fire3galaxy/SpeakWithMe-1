using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaDialogue : Dialogue
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        textContainer = GetComponentInChildren<Text>();
        scripts = new string[]{
            "Banana 香蕉 (Hsiang Chiao)"
        };
    }
}
