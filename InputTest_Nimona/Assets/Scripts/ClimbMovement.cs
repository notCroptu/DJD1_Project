using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbMovement : MonoBehaviour
{
    PlayerActions playerActions;
    [SerializeField] private GorillaClimb climbScript;
    [SerializeField] private Movement movement;
    [SerializeField] private float climbSpeed;

    private Rigidbody2D rb;
    private float initialXVelocity;

    // Start is called before the first frame update
    void Start()
    {
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
        initialXVelocity = rb.velocity.x;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Climbing");
        // Get player input for horizontal and vertical movement
        float horizontalMovement = playerActions.MoveX;
        float verticalMovement = playerActions.MoveY;

        // Disable gravity while climbing
        rb.gravityScale = 0;

        // Adjust vertical velocity based on climb speed
        Vector3 currentVel = rb.velocity;
        currentVel.y = verticalMovement * climbSpeed;

        // Apply a dampening effect to the initial horizontal velocity
        initialXVelocity = Mathf.Sign(initialXVelocity) * (Mathf.Abs(initialXVelocity) * 0.1f);

        // Set the new velocity of the Rigidbody2D
        rb.velocity = new Vector3(initialXVelocity, currentVel.y, 0f);

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
