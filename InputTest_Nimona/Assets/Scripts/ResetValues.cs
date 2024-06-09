using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetValues : MonoBehaviour
{
    void Start()
    {
        Timer timer = FindObjectOfType<Timer>();
        PlayerScore score = FindObjectOfType<PlayerScore>();
        timer.ResetTimer();
        score.ResetScore();
    }
}
