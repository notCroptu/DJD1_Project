using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorilla : MonoBehaviour , IShapeColliders 
{
    [field:SerializeField] public CapsuleCollider2D GroundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D AirCollider { get; set; }
    [field:SerializeField] public BoxCollider2D GroundCheckCollider { get; set; }
    [SerializeField] private float jumpSpeed;

    private Movement movement;

    void Start()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        movement.JumpSpeed = jumpSpeed;
    }
    void OnEnable()
    {
        movement = transform.parent.gameObject.GetComponent<Movement>();
        movement.JumpSpeed = jumpSpeed;
    }
}
