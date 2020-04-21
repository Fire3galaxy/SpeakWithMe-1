using UnityEngine;

// Parent for each specific scriptHolder
public abstract class ScriptHolder : MonoBehaviour {
    // Narrator: Do English audio
    // Dialogue: Do text + Chinese audio
    // Player: Record the player
    // Pause: Wait for user to go to next line, playback recording, or go back.
    public Speaker[] script = { };

    public enum Speaker {
        Narrator,
        Dialogue,
        Player,
        Pause
    }
}
