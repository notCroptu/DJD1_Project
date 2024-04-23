using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    private float duration = 0;
    private float magnitude = 0;
    private bool isShaking;
    void Update()
    {
        if (isShaking)
        {
            if ( duration <=  0 )
            {
                duration = 0;
                magnitude = 0;
                isShaking = false;
                return;
            }
            
            Vector3 newPosition = transform.position;

            float x = Random.Range(-1f, 1f) * magnitude * duration;
            float y = Random.Range(-1f, 1f) * magnitude * duration;

            transform.position += new Vector3(x, y, 0f);

            duration -= Time.deltaTime;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
        isShaking = true;
    }
}
