using UnityEngine;

class PlayerGUIControls : MonoBehaviour
{
    int selectedId = 0;
    void Start()
    {

    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     selectedId--;
        //     Debug.Log("On button " + selectedId);
        // }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     selectedId++;
        //     Debug.Log("On button " + selectedId);
        // }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse position = " + Input.mousePosition);
            Debug.Log("Mouse position (world) = " + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
        }
    }
}