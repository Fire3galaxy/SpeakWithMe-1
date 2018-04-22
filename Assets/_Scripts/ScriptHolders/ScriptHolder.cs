using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent for each specific scriptHolder
public abstract class ScriptHolder : MonoBehaviour {
    // Narrator: Do audio clip
    // Dialogue: Do text+audio
    // Player: Record the player
    // Pause: Wait for user to go to next line, playback recording, or go back.
    public const int Narrator = 0, Dialogue = 1, Player = 2, Pause = 3;
    public int[] script = { };
}
