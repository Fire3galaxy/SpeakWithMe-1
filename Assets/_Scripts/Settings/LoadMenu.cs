using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    }
}
