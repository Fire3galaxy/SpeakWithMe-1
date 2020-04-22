using UnityEngine.UI;

public class GreetingGuyDialogue : Dialogue {
    string[] _scripts = new string[] {
                             "Hello 你好 (Ni Hao)",
                            "Hello 你好 (Ni Hao)",
                            "Hello 你好 (Ni Hao)",
                            "Hello 你好 (Ni Hao)",
                            "Hello 你好 (Ni Hao)",
                            "My name is 我的名字是 (Wo Te Ming Tzu Shih)",
                            "My name is 我的名字是 (Wo Te Ming Tzu Shih)",
                            "My name is 我的名字是 (Wo Te Ming Tzu Shih)",
                            "My name is 我的名字是 (Wo Te Ming Tzu Shih)",
                            "My name is 我的名字是 (Wo Te Ming Tzu Shih)",
                            "Nice to meet you 很高興認識你 (Hen Kao Hsiang Jen Shih Ni)",
                            "Nice to meet you 很高興認識你 (Hen Kao Hsiang Jen Shih Ni)"
                        };

    protected override string[] scripts
    {
        get {
            return _scripts;
        }
    }
}
