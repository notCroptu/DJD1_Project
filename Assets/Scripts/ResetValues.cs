using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetValues : MonoBehaviour
{
    void Start()
    {
        Timer timer = FindObjectOfType<Timer>();
        PlayerScore score = FindObjectOfType<PlayerScore>();
        if (timer != null) timer.ResetTimer();
        if (score != null) score.ResetScore();
    }
}
