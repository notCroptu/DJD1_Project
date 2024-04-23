using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // class variables that will be changed throughout shapes
    [SerializeField] private BoxCollider2D groundCheckCollider;
    [field:SerializeField] public CapsuleCollider2D groundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D airCollider { get; set; }

    // other variables that will be changed throughout shapes (the field serialize is purely for inspector viewing of variable changes)
    [field:SerializeField] public float moveClamp { get; set; }
    [field:SerializeField] public float moveRate { get; set; }
    [field:SerializeField] public float jumpSpeed { get; set; }

    // The previous variable's default states and their getters
    [SerializeField] private float defaultMoveClamp = 100f;
    public float DefaultMoveClamp => defaultMoveClamp;
    [SerializeField] private float defaultMoveRate = 0.9f;
    public float DefaultMoveRate => defaultMoveRate;
    [SerializeField] private float defaultJumpSpeed = 200f;
    public float DefaultJumpSpeed => defaultJumpSpeed;

    // variables that will be the same throughout shapes
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float coyoteTime;

    private Rigidbody2D rb;
    private Vector3 moveVector;
    public bool IsGrounded { get; set; }
    private bool actualIsGrounded;
    private float coyoteTimer;
    private float defaultGravity;

    private bool jumped;
    private bool isJumping;
    private float deltaX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
        UpdateGroundState();

        // change from air to ground collider or ground to air collider
        groundCollider.enabled = IsGrounded;
        airCollider.enabled = !IsGrounded;

        moveVector = rb.velocity;

        /*
        // sets the horizontal move to be gradual(optional by changing moveRate to 1)
        if ( (deltaX < 0 && rb.velocity.x <= 0) || (deltaX > 0 && rb.velocity.x >= 0) || !IsGrounded)
        {
            moveVector.x = Mathf.Lerp(rb.velocity.x, moveClamp * deltaX, moveRate);
        }
        // including when switching direction and velocity is not 0
        else
        {
            moveVector.x = Mathf.Lerp(rb.velocity.x, 0, moveRate);
        }*/

        moveVector.x = Mathf.Lerp(rb.velocity.x, moveClamp * deltaX, moveRate);

        
        // relative movement since the last frame, as we are calculating physics in Update
        // (as opposed to Fixed Update)
        //moveVector.x *= Time.deltaTime;

        if ( jumped && IsGrounded )
        {
            rb.gravityScale = 2f;
            moveVector.y = jumpSpeed;
            jumped = false;
        }
        if ( isJumping )
        {
            rb.gravityScale = 2f;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        rb.velocity = moveVector;
    }
    void Update()
    {
        if ( Input.GetButtonDown("Jump") ) jumped = true;

        isJumping = Input.GetButton("Jump");
        deltaX = Input.GetAxis("Horizontal");

        // Animation

        if (( deltaX < 0 ))
        {
            transform.rotation = Quaternion.Euler( 0, 180, 0);
        }
        else if (( deltaX > 0 ))
        {
            transform.rotation = Quaternion.identity;
        }
    }
    void UpdateGroundState()
    {
        if (groundCheckCollider)
        {
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useLayerMask = true;
            contactFilter.layerMask = groundLayerMask;

            Collider2D[] results = new Collider2D[128];

            int n = Physics2D.OverlapCollider(groundCheckCollider, contactFilter, results);
            if (n > 0)
            {
                coyoteTimer = coyoteTime;
                IsGrounded = true;
                return;
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
            }
        }

        if (coyoteTimer > 0)
        {
            IsGrounded = true;
            return;
        }

        IsGrounded = false;
    }
    public void ResetValues()
    {
        // here the states are reset to their original forms
        moveClamp = defaultMoveClamp;
        moveRate = defaultMoveRate;
        jumpSpeed = defaultJumpSpeed;
    }
}
