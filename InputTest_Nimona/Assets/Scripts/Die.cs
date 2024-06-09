using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Grabbable grabbable = 
            collision.gameObject.GetComponent<Grabbable>();

        if ( grabbable != null )
        {
            Destroy(gameObject);
            StartCoroutine(TempTimeScaleChange(0.3f, 0.1f));
        }
    }
    private IEnumerator TempTimeScaleChange(float duration, float newTimeScale)
    {
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }
}
