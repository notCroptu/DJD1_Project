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
    private Animator anim;

    public bool Jumped { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the necessary components
        movScript = GetComponentInParent<Movement>();
        climbScript = GetComponentInParent<ClimbMovement>();
        playerActions = GetComponentInParent<PlayerActions>();
        anim = GetComponentInParent<Animator>();
        Jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is wall climbing
        if ( WallCheck && playerActions.Ability.WasPressed && !Jumped )
        {
            // Disable movement script and enable climbing script
            movScript.enabled = false;
            climbScript.enabled = true;
            anim.SetBool("isClimbing",true);
            Debug.Log("Normal Move");
        }
        else if ( !playerActions.Ability.IsPressed || !WallCheck || Jumped )
        {
            // If the player has jumped and is not on the wall anymore, reset Jumped
            if ( Jumped && !WallCheck )
            {
                Jumped = false;
                // StartCoroutine(WaitForSeconds());
            }

            // Enable movement script and disable climbing script
            movScript.enabled = true;
            climbScript.enabled = false;
            anim.SetBool("isClimbing",false);
        }
    }

    // Trigger detection for entering a climbable wall
    private void OnTriggerStay2D(Collider2D other)
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
    void OnDisable()
    {
        // Enable movement script and disable climbing script
        movScript.enabled = true;
        climbScript.enabled = false;
        anim.SetBool("isClimbing",false);
    }
}
