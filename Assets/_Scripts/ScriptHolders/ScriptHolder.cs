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
    public enum Line {
        Narrator,
        Dialogue,
        Player,
        Pause
    }

    // FIXME: In Unity, write down the order of lines for each script holder BEFORE
    // I change this to a Line enum array and delete the const ints.
    public int[] script = { };
}
