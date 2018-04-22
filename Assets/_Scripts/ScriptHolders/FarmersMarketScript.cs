using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmersMarketScript : ScriptHolder {
    void Start()
    {
        script = new int[]{
            Dialogue,
            Player,
            Pause
        };
    }
}
