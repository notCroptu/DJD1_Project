using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rhino : MonoBehaviour , IShapeColliders 
{
    private PlayerActions playerActions;

    private PlayerSounds playerSounds;
    private SoundsScript audioPlayer;

    // reference variables to change in player
    [field:SerializeField] public CapsuleCollider2D GroundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D AirCollider { get; set; }
    [field:SerializeField] public BoxCollider2D GroundCheckCollider { get; set; }

    // value variables to change in player
    [SerializeField] private float runClamp = 100f;
    public float RunClamp => runClamp;
    // private float runRate;
    private float walkClamp;
    // private float walkRate;

    // variables for breaking
    private Vector3 bufferVelocity;
    private bool collided;

    // variables for breaking destructible tilemap
    [SerializeField] private float desBreakPoint;
    [SerializeField] private float shakeForce = 10f;
    [SerializeField] private float breakAngle = 60f;
    private Tilemap desTilemap;

    // player variables
    private GameObject player;
    private Movement movement;
    private Rigidbody2D rb;

    // Shapeshift points mechanic
    private Shapeshifting shpshift;

    void Start()
    {
        audioPlayer = GetComponentInParent<SoundsScript>();
        playerSounds = GetComponentInParent<PlayerSounds>();

        player = transform.parent.gameObject;
        movement = player.GetComponentInParent<Movement>();
        rb = player.GetComponent<Rigidbody2D>();
        walkClamp = movement.DefaultMaxSpeed;
        // walkRate = movement.DefaultMoveRate;

        // runRate = (walkClamp / runClamp) * walkRate;

        playerActions = GetComponentInParent<PlayerActions>();
    }
    void OnEnable()
    {
        shpshift = GetComponentInParent<Shapeshifting>();
        Debug.Log($"GET SHAPESHIFTING : {shpshift}");
        shpshift.RhinoPoints -= 1;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bufferVelocity.magnitude >= desBreakPoint)
        {
            float shake = bufferVelocity.magnitude * shakeForce / RunClamp;
            Camera camera = Camera.main;

            KnightMovement knightMovement =
                collision.gameObject.GetComponent<KnightMovement>();

            Destructibles destructibles =
                collision.gameObject.GetComponent<Destructibles>();

            if (destructibles != null)
            {
                // StartCoroutine(HandleCollision(collision, camera, shake, true, null));
                DestroyTilemap(collision);
                camera.GetComponent<Shaker>().Shake(0.7f, shake);
                collided = true;
            }
            else if (knightMovement != null)
            {
                // StartCoroutine(HandleCollision(collision, camera, shake, false, knightMovement));
                knightMovement.DieSequence();
                camera.GetComponent<Shaker>().Shake(0.4f, shake);
                collided = true;
            }
        }
    }

    private IEnumerator HandleCollision(Collision2D collision, Camera camera, float shake, bool isDestructible, KnightMovement knightMovement)
    {
        yield return StartCoroutine(TempTimeScaleChange(0.1f));

        audioPlayer.SoundToPlay = playerSounds.Ram;

        if (isDestructible)
        {
            audioPlayer.PlayAudio();

            DestroyTilemap(collision);
            camera.GetComponent<Shaker>().Shake(0.8f, shake);
        }
        else if (knightMovement != null)
        {
            audioPlayer.PlayAudio();
            
            camera.GetComponent<Shaker>().Shake(0.6f, shake);
            knightMovement.DieSequence();
        }

        collided = true;
    }

    private IEnumerator TempTimeScaleChange(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }


    void DestroyTilemap(Collision2D collision)
    {
        foreach(ContactPoint2D hit in collision.contacts)
        {
            float velocityAngle = Mathf.Atan2(bufferVelocity.y, bufferVelocity.x) * Mathf.Rad2Deg;
            float hitAngle = Mathf.Atan2(-hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
            float angleDifference = Mathf.DeltaAngle(velocityAngle, hitAngle);

            Debug.Log($"Velocity Angle: {velocityAngle}");
            Debug.Log($"Hit Angle: {hitAngle}");
            Debug.Log($"Angle Difference: {angleDifference}");

            if ( Mathf.Abs(angleDifference) <= breakAngle )
            {
                float shake = bufferVelocity.magnitude * shakeForce / RunClamp;
                Camera camera = Camera.main;
                camera.GetComponent<Shaker>().Shake(0.7f, shake);

                Destroy(collision.gameObject);
                collided = true;
                break;
            }
        }
    }
    /*
    void DestroyTilemap(Collision2D collision)
    {
        desTilemap = collision.gameObject.GetComponent<Tilemap>();
        Vector3 hitPosition;

        foreach(ContactPoint2D hit in collision.contacts)
        {
            hitPosition = hit.point;

            float velocityAngle = Mathf.Atan2(bufferVelocity.y, bufferVelocity.x) * Mathf.Rad2Deg;
            float hitAngle = Mathf.Atan2(hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
            float angleDifference = Mathf.DeltaAngle(velocityAngle, hitAngle);

            if ( Mathf.Abs(angleDifference) <= breakAngle )
            {
                hitPosition.x -= 1f * hit.normal.x;
                hitPosition.y -= 1f * hit.normal.y;
            }

            if (desTilemap.GetTile(desTilemap.WorldToCell(hitPosition)) != null)
            {
                float shake = bufferVelocity.magnitude * shakeForce / RunClamp;
                Camera camera = Camera.main;
                camera.GetComponent<Shaker>().Shake(0.7f, shake);

                desTilemap.SetTile(desTilemap.WorldToCell(hitPosition), null);
                collided = true;
            }
        }
    }*/
    void FixedUpdate()
    {
        // this checks if it has had a breaking collision and if true it sets the velocity to the previous velocity
        if ( collided )
        {
            rb.velocity = bufferVelocity;
            collided = false;
        }
        else
        {
            bufferVelocity = rb.velocity;
        }
    }
    void Update()
    {
        if  ( playerActions.Ability.IsPressed && ( movement.IsGrounded ) )
        {
            if ( (movement.MaxSpeed != runClamp) /*|| (movement.MoveRate != runRate)*/ )
            {
                movement.MaxSpeed = runClamp;
                movement.CurrentRun = playerSounds.Charge;
                // movement.MoveRate = runRate;
            }
        }
        else if ( movement.IsGrounded )
        {
            if ( (movement.MaxSpeed != walkClamp) /* || (movement.MoveRate != walkRate)*/ )
            {
                movement.MaxSpeed = walkClamp;
                movement.CurrentRun = playerSounds.Walk;
                // movement.MoveRate = walkRate;
            }
        }
    }
    void OnDisable()
    {
        movement.CurrentRun = playerSounds.Walk;
    }
}
