using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static float Score { get; private set; }
    
    public static void ChangeScore(float points)
    {
        Score += points;
    }
}
