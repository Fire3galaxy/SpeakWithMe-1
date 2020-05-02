public class DirectionPoliceScript : ScriptHolder {
    void Start()
    {
        script = new Speaker[]{
            Speaker.Dialogue, // 45
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue, // 46
            Speaker.Player,
            Speaker.Pause,

            Speaker.Dialogue, // 47
            Speaker.Player,
            Speaker.Pause,
            Speaker.Narrator, // 49

            Speaker.Dialogue, // 50
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue, // 51
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue, // 52
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue, // 52
            Speaker.Player,
            Speaker.Pause,

            Speaker.Narrator, // 55

            Speaker.Dialogue, // 56
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue, // 57
            Speaker.Player,
            Speaker.Pause,

            Speaker.Narrator, // 58
            Speaker.Dialogue
        };
    }
}
