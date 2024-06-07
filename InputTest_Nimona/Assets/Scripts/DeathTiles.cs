using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTiles : MonoBehaviour
{
    private Death deathScript;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other + " has collided with death tiles");

        Movement player = other.gameObject.GetComponentInParent<Movement>();

        if (player != null)
        {
            deathScript = target.GetComponentInParent<Death>();
            deathScript.GameOver();
        }
    }
}
