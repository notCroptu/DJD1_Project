using System.Collections;
using System.Collections.Generic;
using InterfaceMovement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string buttonOne;
    [SerializeField] private string buttonTwo;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PlayerActions buttonScript;

    void Update()
    {
        if (buttonScript.PauseBtt.WasPressed)
        {
            Pause();
        }
    }
    public void ButtonOneActive()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buttonOne);
    }
    public void ButtonTwoActive()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buttonTwo);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
