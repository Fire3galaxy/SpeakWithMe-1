using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleDialogue : Dialogue {
    // Use this for initialization
    void Start() {
        textContainer = GetComponentInChildren<Text>();
        scripts = new string[]{
            "Apple - 蘋果 \"Píngguǒ\"",
            "Try saying 蘋果 \"Píngguǒ\"",
            "Good job, you learned to say 蘋果 \"Píngguǒ\" (Apple)"
        };
    }
}
