using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private bool ResetValues;
    [SerializeField] private bool ResetMusic;
    [SerializeField] private string nextRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Movement player = other.gameObject.GetComponentInParent<Movement>();

        if (player != null)
        {
            if ( ResetValues )
            {
                Timer timer = FindObjectOfType<Timer>();
                PlayerScore score = FindObjectOfType<PlayerScore>();
                timer.Reset();
                score.Reset();
            }
            if ( ResetMusic )
            {
                MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
                Destroy(musicPlayer.gameObject);
            }
            
            SceneManager.LoadScene(nextRoom);
        }
    }
}
