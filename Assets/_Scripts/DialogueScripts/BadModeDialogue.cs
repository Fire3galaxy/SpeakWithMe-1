using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadModeDialogue : Dialogue
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        textContainer = GetComponentInChildren<Text>();
        scripts = new string[]{
            "I’m screwed 悲劇 (Pei Chu)",
            "I’m so so happy 很爽 (Hen Shuang)",
            "Homebody 宅 (Chai)"
        };
    }
}