using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private LayerMask excludeLayersOnDie;

    [SerializeField] private GameObject rhino;
    [SerializeField] private float knockback = 2;

    private Rigidbody2D rb;
    private Rigidbody2D rbP;
    private GameObject player;

    private Vector2 bufferVelocity;
    private float clamp;

    private bool dead;
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if ( collision.gameObject.CompareTag("Player") )
        {
            float parryForce = Mathf.Sign(bufferVelocity.x) * clamp - bufferVelocity.x;
            parryForce *= knockback;
            Vector2 newVelocity = new Vector2(- parryForce, 0f);

            Debug.Log("This is bufferVelocity " + bufferVelocity);
            Debug.Log("This is newVelocity " + newVelocity);
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
            clamp = rhino.GetComponent<Rhino>().RunClamp;
        }

        if ( dead && (Mathf.Abs(rb.velocity.x) <  30) )
        {
            Destroy(gameObject);
        }
    }
    public void DieSequence()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = excludeLayersOnDie;

        rb.AddForce(new Vector2(bufferVelocity.x * rb.mass, 0f), ForceMode2D.Impulse);

        dead = true;
    }
}
