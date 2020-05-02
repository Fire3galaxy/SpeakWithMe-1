using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DirectionPoliceDialogue : Dialogue
{
    string[] _scripts = new string[] {
                            "Police Officer 警察 (Jing Cha)",
                            "Police Officer 警察 (Jing Cha)",
                            "Hello! 你好 (Ni Hao)",
                            "How do I get to the Market? 怎麼去市場？(Tse Ma Chu Shih Chang)",
                            "Go straight. 直走 (Chih Tsou)",
                            "Turn to the left 左轉 (Tso Chuan)",
                            "Turn left 左轉 (Tso Chuan)",
                            "Thank you! 謝謝 (Hsieh Hsieh)",
                            "Goodbye! 再見 (Tsai Chien)",
                            "Now proceed to the market (walk to the market or click your heels three " + 
                                "times to teleport!)"
                        };

    protected override string[] scripts
    {
        get {
            return _scripts;
        }
    }
}
