using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadModeScript : ScriptHolder
{
    void Start()
    {
        script = new int[]{
            Narrator,
            Dialogue,
            Player,
            Pause,
            Dialogue,
            Player,
            Pause,
            Dialogue,
            Player,
            Pause
        };
    }
}

