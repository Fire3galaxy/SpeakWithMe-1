public class DirectionPoliceScript : ScriptHolder {
    void Start()
    {
        script = new Speaker[]{
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,

            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Narrator,

            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,

            Speaker.Narrator,

            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,

            Speaker.Narrator,
            Speaker.Dialogue
        };
    }
}
