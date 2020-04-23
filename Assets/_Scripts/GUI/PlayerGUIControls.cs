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
            Vector3 normalizedPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("Over button");
    }

    void OnMouseOver()
    {
        Debug.Log("Over button");
    }
}