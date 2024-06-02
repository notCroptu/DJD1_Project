using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Movement : MonoBehaviour
{
    PlayerActions playerActions;

    // class variables that will be changed throughout shapes
    [SerializeField] private BoxCollider2D groundCheckCollider;
    [field:SerializeField] public CapsuleCollider2D groundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D airCollider { get; set; }

    // other variables that will be changed throughout shapes (the field serialize is purely for inspector viewing of variable changes)
    public float MoveClamp { get; set; }
    public float MoveRate { get; set; }
    public float JumpSpeed { get; set; }
    public float FallingGravity { get; set; }

    // The previous variable's default states and their getters
    [SerializeField] private float defaultMoveClamp = 100f;
    public float DefaultMoveClamp => defaultMoveClamp;
    [SerializeField] private float defaultMoveRate = 0.9f;
    public float DefaultMoveRate => defaultMoveRate;
    [SerializeField] private float defaultJumpSpeed = 200f;
    public float DefaultJumpSpeed => defaultJumpSpeed;
    [SerializeField] private float defaultFallingGravity = 2f;
    public float DefaultFallingGravity => defaultFallingGravity;

    // variables that will be the same throughout shapes
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float knockOutTime = 1f;

    private Rigidbody2D rb;
    private Vector3 moveVector;
    public bool IsGrounded { get; set; }
    private bool canJump;
    private float coyoteTimer;

    public bool Jumped { get; set; }
    private bool isJumping;
    private float deltaX;
    private bool inputEnabled;
    void Start()
    {
        inputEnabled = true;
        rb = GetComponent<Rigidbody2D>();

        playerActions = GetComponent<PlayerActions>();
    }

    void FixedUpdate()
    {
        UpdateGroundState();

        // change from air to ground collider or ground to air collider
        groundCollider.enabled = IsGrounded;
        airCollider.enabled = !IsGrounded;

        if ( inputEnabled)
        {
            moveVector = rb.velocity;

            moveVector.x = Mathf.Lerp(rb.velocity.x, MoveClamp * deltaX, MoveRate);

            if ( Jumped && canJump && IsGrounded )
            {
                rb.gravityScale = FallingGravity;
                moveVector.y = JumpSpeed;
                Jumped = false;
                canJump = false;
            }
            if ( isJumping )
            {
                rb.gravityScale = FallingGravity;
            }
            else
            {
                rb.gravityScale = 4f;
                Jumped = false;
            }

            rb.velocity = moveVector;
        }

    }
    void Update()
    {
        if ( playerActions.Jump ) Jumped = true;

        isJumping = playerActions.Jump.IsPressed;
        deltaX = playerActions.MoveX;

        // Animation

        if (( deltaX < 0 ) && (transform.right.x > 0) )
        {
            transform.rotation = Quaternion.Euler( 0, 180, 0);
        }
        else if (( deltaX > 0 ) && (transform.right.x < 0) )
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
                canJump = true;
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
    public void ImpulsePlayer(Vector2 impulse)
    {
        StartCoroutine(ImpulsePlayerCR(impulse));
    }
    IEnumerator ImpulsePlayerCR(Vector2 impulse)
    {
        rb.velocity = impulse;
        inputEnabled = false;
        yield return new WaitForSeconds(knockOutTime);
        inputEnabled = true;
    }
    public void ResetValues()
    {
        // here the states are reset to their original forms
        MoveClamp = defaultMoveClamp;
        MoveRate = defaultMoveRate;
        JumpSpeed = defaultJumpSpeed;
        FallingGravity = defaultFallingGravity;
    }
}
