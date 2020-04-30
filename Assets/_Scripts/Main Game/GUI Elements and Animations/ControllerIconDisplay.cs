using UnityEngine;

/*
    The intention of this class is to instantiate a recording icon right above the SteamVR
    controller when the player is recording audio and a play audio icon when the player is
    playing back audio.

    Script is intended to be attached to the left/right controller.
 */
public class ControllerIconDisplay : MonoBehaviour
{
    // public GameObject recordingIconPrefab;
    // public GameObject playingIconPrefab;
    public GameObject iconCardPrefab;

    bool instantiatedIcons = false;
    Vector3 modelPositionOffset = new Vector3(-.0303f, -.0737f, 0f);
    Quaternion iconRotation = Quaternion.Euler(90f, 0f, 0f);
    BlinkIcon recordingIconBlinker;
    BlinkIcon playingIconBlinker;
    PlayerMicControls playerMic;
    AudioSource narratorAudio, playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        Transform player = transform.Find(PlayerLoader.playerPath());
        playerMic = player.GetComponent<PlayerMicControls>();
        narratorAudio = player.Find("Narrator Audio").GetComponent<AudioSource>();
        playerAudio = player.Find("Recordings Audio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!instantiatedIcons)
        {
            // Using trackpad because icons should be parallel to trackpad
            Transform trackpadTransform = transform.Find("Model/trackpad/attach");
            if (trackpadTransform == null)
            {
                return;
            }
                
            GameObject iconCard = GameObject.Instantiate(iconCardPrefab, trackpadTransform, false);
            recordingIconBlinker = iconCard.transform.Find("Intro Recording Icon").GetComponent<BlinkIcon>();
            playingIconBlinker = iconCard.transform.Find("PlayButton").GetComponent<BlinkIcon>();

            instantiatedIcons = true;
            return;
        }

        recordingIconBlinker.enabled = playerMic.isRecording();
        playingIconBlinker.enabled = playerAudio.isPlaying;
    }
}
