using UnityEngine.UI;

class AppleDialogue : Dialogue {
    string[] _scripts = new string[] {
                            "Apple - 蘋果 \"Píngguǒ\"",
                            "Try saying 蘋果 \"Píngguǒ\"",
                            "Good job, you learned to say 蘋果 \"Píngguǒ\" (Apple)"
                        };

    protected override string[] scripts
    {
        get {
            return _scripts;
        }
    }
}
