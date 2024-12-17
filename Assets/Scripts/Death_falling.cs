using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public float fallThreshold = -10f;

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            LoadMainMenu();
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}