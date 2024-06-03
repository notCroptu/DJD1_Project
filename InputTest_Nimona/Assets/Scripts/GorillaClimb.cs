using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaClimb : MonoBehaviour
{
    PlayerActions playerActions;
    [SerializeField] private Collider2D wallCol;
    public bool WallCheck { get; private set; } = false;
    private Movement movScript;
    private ClimbMovement climbScript;

    [SerializeField] private float wallJumpingDuration = 2f;

    public bool Jumped { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the necessary components
        movScript = GetComponentInParent<Movement>();
        climbScript = GetComponentInParent<ClimbMovement>();
        playerActions = GetComponentInParent<PlayerActions>();
        Jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is wall climbing
        if (WallCheck && playerActions.Ability.IsPressed && !Jumped)
        {
            // Disable movement script and enable climbing script
            movScript.enabled = false;
            climbScript.enabled = true;
        }
        else
        {
            // Enable movement script and disable climbing script
            movScript.enabled = true;
            climbScript.enabled = false;
        }
        
    }

    // Coroutine to handle the wall jumping duration
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(wallJumpingDuration);
    }

    // Trigger detection for entering a climbable wall
    private void OnTriggerEnter2D(Collider2D other)
    {
        Climbable climbable = other.gameObject.GetComponent<Climbable>();

        if (climbable != null)
        {
            WallCheck = true;
        }
    }

    // Trigger detection for exiting a climbable wall
    private void OnTriggerExit2D(Collider2D other)
    {
        Climbable climbable = other.gameObject.GetComponent<Climbable>();

        if (climbable != null)
        {
            WallCheck = false;
        }
    }
}
