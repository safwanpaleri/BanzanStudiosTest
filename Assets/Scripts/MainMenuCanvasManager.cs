using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasManager : MonoBehaviour
{
    public void onPlayButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void onExitButtonPressed()
    {
        Application.Quit();
    }
}
