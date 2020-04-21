public class FarmersMarketScript : ScriptHolder {
    void Start()
    {
        script = new Speaker[]{
            Speaker.Dialogue,
            Speaker.Player,
            Speaker.Pause
        };
    }
}
