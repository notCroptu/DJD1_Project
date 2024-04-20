using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private BoxCollider2D airCollider;

    [SerializeField] public float runSpeed = 100f;
    [SerializeField] public float runRate = 0.9f;
    [SerializeField] public float walkSpeed = 100f;
    [SerializeField] public float walkRate = 0.9f;

    private GameObject player;
    private Movement movement;

    void Start()
    {
        player = transform.parent.gameObject;
        movement = player.GetComponent<Movement>();
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
            if ( (movement.moveClamp != walkSpeed) || (movement.moveRate != walkRate) )
            {
                movement.moveClamp = walkSpeed;
                movement.moveRate = walkRate;
            }
        }
    }
}
