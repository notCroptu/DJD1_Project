using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rhino : MonoBehaviour
{
    // reference variables to change in player
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private BoxCollider2D airCollider;

    // value variables to change in player
    [SerializeField] private float runSpeed = 100f;
    [SerializeField] private float runRate = 0.9f;
    private float walkClamp;
    private float walkRate;

    // variables for breaking
    private Vector2 previousVelocity;
    private bool collided;

    // variables for breaking destructible tilemap
    [SerializeField] private float desBreakPoint;
    [SerializeField] private float RatioToBreak = 0.3f;
    private Tilemap desTilemap;

    // player variables
    private GameObject player;
    private Movement movement;
    private Rigidbody2D rb;
    void Start()
    {
        player = transform.parent.gameObject;
        movement = player.GetComponent<Movement>();
        rb = player.GetComponent<Rigidbody2D>();
        walkClamp = movement.DefaultMoveClamp;
        walkRate = movement.DefaultMoveRate;

        //GameObject des = GameObject.FindGameObjectWithTag("Destructibles");
        //desTiles = des.GetComponent<Tilemap>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        previousVelocity = rb.velocity;

        if (collision.gameObject.CompareTag("Destructibles")
        && (rb.velocity.magnitude > desBreakPoint))
        {
            desTilemap = collision.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition;

            foreach(ContactPoint2D hit in collision.contacts)
            {
                hitPosition = hit.point;

                // these ifs check if the normals of the player's velocity are strong enough to actually break a tile
                if ( Mathf.Abs(rb.velocity.normalized.y) > RatioToBreak )
                {
                    hitPosition.x += 1f * hit.normal.x;
                }

                if ( Mathf.Abs(rb.velocity.normalized.y) > RatioToBreak )
                {
                    hitPosition.y += 1f * hit.normal.y;
                }

                if (desTilemap.GetTile(desTilemap.WorldToCell(hitPosition)) != null)
                {
                    desTilemap.SetTile(desTilemap.WorldToCell(hitPosition), null);
                    collided = true;
                }
            }
        }
    }
    void FixedUpdate()
    {
        // this checks if it has had a breaking collision and if true it sets the velocity to the previous velocity
        if (collided)
        {
            rb.velocity = previousVelocity;
            collided = false;
        }
}
    void Update()
    {
        if  ( Input.GetKey(KeyCode.JoystickButton5) )
        {
            if ( (movement.moveClamp != runSpeed) || (movement.moveRate != runRate) )
            {
                movement.moveClamp = runSpeed;
                movement.moveRate = runRate;
            }
        }
        else
        {
            if ( (movement.moveClamp != walkClamp) || (movement.moveRate != walkRate) )
            {
                movement.moveClamp = walkClamp;
                movement.moveRate = walkRate;
            }
        }
    }
}
