using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaClimb : MonoBehaviour
{
    PlayerActions playerActions;
    [SerializeField] private Collider2D wallCol;
    [SerializeField] private string climbTag;
    public bool WallCheck { get; private set; } = false;
    private Movement movScript;
    private ClimbMovement climbScript;

    // Start is called before the first frame update
    void Start()
    {
        movScript = GetComponentInParent<Movement>();
        climbScript = GetComponentInParent<ClimbMovement>();

        playerActions = GetComponentInParent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"WALLCHECK:{WallCheck}");

        if (WallCheck && playerActions.Ability.IsPressed )
        {
            Debug.Log("ISCLIMBING");
            movScript.enabled = false;
            climbScript.enabled = true;
        }
        else
        {
            movScript.enabled = true;
            climbScript.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(climbTag))
        {
            WallCheck = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(climbTag))
        {
            WallCheck = false;
        }
    }
}
