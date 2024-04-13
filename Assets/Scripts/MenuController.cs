using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        AudioController.AudioInstance.Click.Play();
    }

    public void ExitGame()
    {
        AudioController.AudioInstance.Click.Play();
        Application.Quit();
    }
}
