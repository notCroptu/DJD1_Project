using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    Scene currentScene;
    int sceneIndex;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneIndex = currentScene.buildIndex;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SceneManager.LoadScene(sceneIndex - 1);
            Debug.Log("NEXT SCENE");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneManager.LoadScene(sceneIndex + 1);
            Debug.Log("PREVIOUS SCENE");
        }
    }
}
