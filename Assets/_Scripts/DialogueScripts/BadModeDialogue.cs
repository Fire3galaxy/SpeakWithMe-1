using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadModeDialogue : Dialogue
{
    string[] _scripts = new string[] {
                            "I’m screwed 悲劇 (Pei Chu)",
                            "I’m so so happy 很爽 (Hen Shuang)",
                            "Homebody 宅 (Chai)"
                        };

    protected override string[] scripts
    {
        get {
            return _scripts;
        }
    }
}