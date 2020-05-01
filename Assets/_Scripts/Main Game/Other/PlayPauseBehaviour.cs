using UnityEngine;

/* PlayPauseBehaviour:
 * A base class that enforces certain practices for pausing and playing
 */
abstract class PlayPauseBehaviour : MonoBehaviour
{
    abstract protected void UpdatePlay();
    abstract protected void UpdatePause();

    // Children classes should not override Update. I can't enforce this with sealed
    // because Update isn't virtual in MonoBehaviour. Actually, it's not in MonoBehaviour's
    // metadata code. Not sure why.
    void Update()
    {
        if (PlayerSettingsControls.paused) UpdatePause();
        else UpdatePlay();
    }
}