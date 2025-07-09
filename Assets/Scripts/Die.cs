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
        }
    }
}
