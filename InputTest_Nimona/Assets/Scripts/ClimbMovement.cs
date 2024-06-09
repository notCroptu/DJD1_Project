using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbMovement : MonoBehaviour
{
    private PlayerActions playerActions;
    private SoundsScript audioPlayer;
    private AudioSource audioSource;
    private PlayerSounds playerSounds;

    [SerializeField] private GorillaClimb climbScript;
    [SerializeField] private Movement movement;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float climbGravity;

    private Rigidbody2D rb;
    private Vector2 initialXVelocity;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<SoundsScript>();
        playerSounds = GetComponent<PlayerSounds>();
        audioSource = GetComponent<AudioSource>();

        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Get the PlayerActions component from the parent object
        playerActions = GetComponentInParent<PlayerActions>();
    }

    // Called when the script is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        // Store the initial horizontal velocity
        initialXVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input for horizontal and vertical movement
        float horizontalMovement = playerActions.MoveX;
        float verticalMovement = playerActions.MoveY;

        // Disable gravity while climbing
        rb.gravityScale = climbGravity;

        initialXVelocity.x = initialXVelocity.x * 0.95f;
        initialXVelocity.y = initialXVelocity.y * 0.9f;

        // Adjust vertical velocity based on climb speed
        Vector3 currentVel = rb.velocity;
        currentVel.y = verticalMovement * climbSpeed + initialXVelocity.y;

        // Set the new velocity of the Rigidbody2D
        rb.velocity = new Vector3(initialXVelocity.x, currentVel.y, 0f);

        if ( Mathf.Abs(currentVel.y) > 10 )
        {
            audioSource.clip = playerSounds.Climb;
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }

        // Check for wall jump input
        if (playerActions.Jump.WasPressed)
        {
            WallJump();
        }

        // Rotate the player to face the direction of movement
        if ((horizontalMovement < 0) && (transform.right.x > 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if ((horizontalMovement > 0) && (transform.right.x < 0))
        {
            transform.rotation = Quaternion.identity;
        }
    }

    // Handles the wall jump action
    private void WallJump()
    {
        audioPlayer.SoundToPlay = playerSounds.Jump;
        audioPlayer.PlayAudio();

        // Calculate the new velocity for the wall jump
        Vector2 moveVector = rb.velocity;

        rb.gravityScale = movement.FallingGravity;
        moveVector.x = Mathf.Sign(playerActions.MoveX) * movement.MaxSpeed;
        moveVector.y = movement.JumpSpeed * 1.3f;

        // Apply the new velocity to the Rigidbody2D
        rb.velocity = moveVector;
        Debug.Log("velocity " + rb.velocity);

        // Set the jump state in the climb script
        climbScript.Jumped = true;
        Debug.Log("Jumped");
    }
}
