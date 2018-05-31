using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    private bool IsPaused;
    
    public void Play()
    {
        SceneManager.LoadScene(3);
    }

    public void playSurv()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
