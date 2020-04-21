public class GreetingGuyScript : ScriptHolder {
    void Start()
    {
        script = new int[]{
            Narrator,
            Dialogue,
            Dialogue,
            Dialogue,
            Player,
            Pause,
            Dialogue,
            Dialogue,
            Player,
            Pause, // end of one
            Narrator,
            Dialogue,
            Dialogue,
            Dialogue,
            Player,
            Pause,
            Dialogue,
            Dialogue,
            Player,
            Pause,      // end of second lesson
            Narrator, // 42
            Dialogue,
            Player,
            Pause,
            Dialogue,
            Player,
            Pause, // end of 3
            Narrator
        };
    }
}
