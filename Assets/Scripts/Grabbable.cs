using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Sword sword = 
            collision.gameObject.GetComponent<Sword>();

        if ( sword != null )
        {
            StartCoroutine(TempTimeScaleChange(0.1f));
        }
    }
    private IEnumerator TempTimeScaleChange(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }
}
