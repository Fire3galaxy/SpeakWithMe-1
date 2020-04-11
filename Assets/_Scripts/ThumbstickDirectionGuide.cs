using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The intention of this class is to instantiate an arrow right above the SteamVR
    joystick that indicates which direction is "forward".

    Script is intended to be attached to the left/right controller.
    
    This solves a particular gripe I have with the Windows Mixed Reality 
    controller: it's VERY unclear which direction exactly on the controller is
    pure "forward". As such, it's pretty easy to move diagonally slightly by
    accident. The SteamVR player however doesn't instantiate its controller
    prefab directly; it appears to load the individual components in dynamically
    during runtime only. 
 */
public class ThumbstickDirectionGuide : MonoBehaviour
{
    public GameObject arrowPrefab;

    bool instantiatedArrow = false;
    Vector3 thumbstickOffset = new Vector3(0f, .0221f, .0172f);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!instantiatedArrow)
        {
            Transform joystickTransform = transform.Find("Model/thumbstick/attach");
            if (joystickTransform != null)
            {
                
                GameObject arrow = GameObject.Instantiate(arrowPrefab, 
                                                      joystickTransform, 
                                                      true);
                arrow.transform.localPosition = thumbstickOffset;
                arrow.transform.localRotation = Quaternion.identity;
                instantiatedArrow = true;
            }
        }
    }
}
