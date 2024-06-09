using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private Scene currentScene;
    private PlayerSounds playerSounds;
    private SoundsScript audioPlayer;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        audioPlayer = GetComponent<SoundsScript>();
        playerSounds = GetComponent<PlayerSounds>();
    }
    public void GameOver()
    {
        audioPlayer.SoundToPlay = playerSounds.Death;
        audioPlayer.PlayAudio();

        //GameOver code
        Debug.Log("KILELD PLAYER!");
        SceneManager.LoadScene(currentScene.name);
    }
}
