public class GreetingGuyScript : ScriptHolder {
    void Start()
    {
        script = new Speaker[]{
            Speaker.Narrator,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause, // end of one
            Speaker.Narrator,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,      // end of second lesson
            Speaker.Narrator, // 42
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause, // end of 3
            Speaker.Narrator
        };
    }
}
