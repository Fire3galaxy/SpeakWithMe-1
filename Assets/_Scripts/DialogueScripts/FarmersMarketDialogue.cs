using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmersMarketDialogue : Dialogue
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        textContainer = GetComponentInChildren<Text>();
        scripts = new string[]{
            "One to Ten 一 (Yi) 二 (Erh) 三 (San) 四 (Ssu) 五 (Wu) 六 (Liu) 七 (Chi) 八 (Ba) 九 (Chiu) 十 (Shih)"
        };
    }
}