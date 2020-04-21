using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaDialogue : Dialogue
{
    string[] _scripts = new string[] {
                            "Banana 香蕉 (Hsiang Chiao)"
                        };

    protected override string[] scripts
    {
        get {
            return _scripts;
        }
    }
}
