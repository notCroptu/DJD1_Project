using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private bool ResetMusic;
    [SerializeField] private string nextRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Movement player = other.gameObject.GetComponentInParent<Movement>();

        if (player != null)
        {

            if ( ResetMusic )
            {
                MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
                Destroy(musicPlayer.gameObject);
            }

            SceneManager.LoadScene(nextRoom);
        }
    }
}
