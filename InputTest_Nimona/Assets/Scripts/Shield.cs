using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Rigidbody2D rbP;
    private GameObject player;

    private Vector2 bufferVelocity;
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if ( collision.gameObject.CompareTag("Player") )
        {
            float clamp = player.GetComponent<Movement>().moveClamp;


            float parryForce = Mathf.Sign(bufferVelocity.x) * clamp - bufferVelocity.x;
            float newXvelocity = - parryForce * 2;

            rbP.velocity = new Vector2(newXvelocity, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbP = player.GetComponent<Rigidbody2D>();

        if ( rbP != null )
        {
            bufferVelocity = rbP.velocity;
        }
    }
}
