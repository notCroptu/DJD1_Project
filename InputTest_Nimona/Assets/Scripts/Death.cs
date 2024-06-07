using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private Scene currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void GameOver()
    {
        //GameOver code
        Debug.Log("KILELD PLAYER!");
        SceneManager.LoadScene(currentScene.name);
    }
}
