using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void Play(int index)
    {
        SceneManager.LoadScene(index);        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
