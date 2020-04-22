using UnityEngine;

// Loads player prefab into the world under common object name "Player"
public class PlayerLoader : MonoBehaviour
{
    public HeadsetType headsetType;
    public GameObject OVRHeadsetPrefab, OpenVRHeadsetPrefab, NoVRPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objectToInstantiate = headsetType == HeadsetType.OVR ? OVRHeadsetPrefab :
                                         headsetType == HeadsetType.OpenVR ? OpenVRHeadsetPrefab :
                                         NoVRPrefab;
        GameObject playerObject = Instantiate(objectToInstantiate, transform, true);
        playerObject.name = objectToInstantiate.name; // Removes "(Clone)" from name.
    }

    public static string playerPath()
    {
        return "/Player";
    }
    

    public enum HeadsetType {
        OVR, OpenVR, NoVR
    }
}
