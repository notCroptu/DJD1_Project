using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaClimb : MonoBehaviour
{
    [SerializeField] private Collider2D wallCol;
    [SerializeField] private KeyCode climbKey;
    [SerializeField] private GameObject playerObject;
    public bool WallCheck { get; private set; } = false;
    private Movement movScript;
    private ClimbMovement climbScript;

    // Start is called before the first frame update
    void Start()
    {
        movScript = playerObject.GetComponent<Movement>();
        climbScript = playerObject.GetComponent<ClimbMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"WALLCHECK:{WallCheck}");

        if (WallCheck && Input.GetKey(climbKey) )
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
        if (other.CompareTag("Wall"))
        {
            WallCheck = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            WallCheck = false;
        }
    }
}
