public class BadModeScript : ScriptHolder
{
    void Start()
    {
        script = new Speaker[]{
            Speaker.Narrator,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause,
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause
        };
    }
}

