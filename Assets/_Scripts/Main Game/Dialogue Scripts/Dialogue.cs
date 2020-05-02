using UnityEngine.UI;

// Plays voice clip from narrator AND adds text display for player to read
abstract class Dialogue : Narration {
    Text textContainer;

    // Must be set by children. String array and AudioClip array are expected
    // to be same size and matched one-to-one, so you may need duplicate string
    // script lines.
    protected abstract string[] scripts { get; }

    override protected void Start()
    {
        base.Start();
        textContainer = GetComponentInChildren<Text>();
    }

    override public void StartNarration(OnNarrationCompleteListener caller) {
        textContainer.text = scripts[scriptLine];
        textContainer.gameObject.SetActive(true);

        base.StartNarration(caller);
    }

    public void HideDialogue() {
        textContainer.gameObject.SetActive(false);
    }
}
