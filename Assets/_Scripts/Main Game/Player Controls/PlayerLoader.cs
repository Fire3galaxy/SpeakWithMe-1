using System.Collections.Generic;
using UnityEngine;

// Loads player prefab into the world under common object name "Player"
public class PlayerLoader : MonoBehaviour
{
    public GameObject OVRHeadsetPrefab, OpenVRHeadsetPrefab, NoVRPrefab;

    int cachedHeadsetType;
    GameObject cachedHeadsetInstance;
    List<HeadsetChangeListener> listeners = new List<HeadsetChangeListener>();

    void Start()
    {
        cachedHeadsetType = PlayerPrefs.GetInt(PlayStyleSettings.preferenceKey);
        loadHeadset((HeadsetType) cachedHeadsetType);
    }

    void Update()
    {
        // Note: we're implicitly saying if PlayerPrefs lacks value (which should never happen),
        // then behavior is undefined/will crash.
        int latestValue = PlayerPrefs.GetInt(PlayStyleSettings.preferenceKey, -1);
        if (latestValue == cachedHeadsetType) return;
        
        removeHeadset((HeadsetType) cachedHeadsetType);
        loadHeadset((HeadsetType) latestValue);
        cachedHeadsetType = latestValue;
    }

    public void addListener(HeadsetChangeListener listener)
    {
        listeners.Add(listener);
    }

    void loadHeadset(HeadsetType headsetType)
    {
        GameObject objectToInstantiate = headsetType == HeadsetType.OVR ? OVRHeadsetPrefab :
                                         headsetType == HeadsetType.OpenVR ? OpenVRHeadsetPrefab :
                                         NoVRPrefab;
        cachedHeadsetInstance = Instantiate(objectToInstantiate, transform, true);
        cachedHeadsetInstance.name = objectToInstantiate.name; // Removes "(Clone)" from name.

        foreach (HeadsetChangeListener listener in listeners) 
            listener.onLoadHeadset(headsetType, cachedHeadsetInstance);
    }

    void removeHeadset(HeadsetType headsetType)
    {
        Destroy(cachedHeadsetInstance);
        foreach (HeadsetChangeListener listener in listeners) 
            listener.onRemoveHeadset(headsetType);
    }

    public static string playerPath()
    {
        return "/Player";
    }
    
    public enum HeadsetType {
        OpenVR, OVR, NoVR, ValueNotSet // Last one should never be visible to player. Only used in code.
    }

    public interface HeadsetChangeListener {
        void onRemoveHeadset(HeadsetType headsetType);
        void onLoadHeadset(HeadsetType headsetType, GameObject headsetInstance);
    }
}
