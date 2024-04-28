using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float knockback = 2;

    private Rigidbody2D rbP;
    private GameObject player;
    private Vector2 bufferVelocity;
    private float clamp;
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if ( collision.gameObject.CompareTag("Player") )
        {
            GameObject rhino = collision.gameObject.transform.Find("Rhino").gameObject;
            clamp = rhino.GetComponent<Rhino>().RunClamp;

            float parryForce = Mathf.Sign(bufferVelocity.x) * clamp - bufferVelocity.x;
            parryForce *= knockback;
            Vector2 newVelocity = new Vector2(- parryForce, 0f);

            rbP.AddForce(newVelocity, ForceMode2D.Impulse);
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
