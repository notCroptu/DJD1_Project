using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    [SerializeField] private LayerMask excludeLayersOnDie;

    [SerializeField] private float speed = 100f;
    [SerializeField] private float stopDistance = 128f;
    [SerializeField] private float sightDistance = 256f;
    [SerializeField] private LayerMask collidables;

    private Rigidbody2D rb;
    private Rigidbody2D rbP;
    private GameObject player;

    private Vector2 bufferVelocity;
    private bool dead;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbP = player.GetComponent<Rigidbody2D>();

        if ( rbP != null )
        {
            Vector2 dirToPlayer = rbP.position - rb.position;
            dirToPlayer = dirToPlayer.normalized;

            // make sure there are no obstacles like abscesses or walls and that the player is in sight
            Vector3 distance = transform.position;
            distance += new Vector3(stopDistance * Mathf.Sign(dirToPlayer.x), 0f, 0f);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(distance, dirToPlayer, sightDistance, collidables);

            if (( hit.collider != null ))
            {
                Debug.Log(hit.transform.tag);
                if ( hit.transform.tag == "Player" )
                {
                    rb.velocity = new Vector2(speed * Mathf.Sign(dirToPlayer.x), 0f);
                }
                
            }

            //update the buffervelocity
            bufferVelocity = rbP.velocity;
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
