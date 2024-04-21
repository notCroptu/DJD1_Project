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
    private bool isGrounded;
    private bool actualIsGrounded;
    private float coyoteTimer;
    private float defaultGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
        UpdateGroundState();

        // change from air to ground collider or ground to air collider
        groundCollider.enabled = isGrounded;
        airCollider.enabled = !isGrounded;

        float deltaX = Input.GetAxis("Horizontal");

        moveVector = rb.velocity;

        // sets the horizontal move to be gradual(optional by changing moveAccel to 1)
        if ( (deltaX < 0 && rb.velocity.x <= 0) || (deltaX > 0 && rb.velocity.x >= 0) )
        {
            moveVector.x = Mathf.Lerp(rb.velocity.x, moveClamp * deltaX, moveRate);
            Debug.Log($"moveRate before: {moveVector.x}");
        }
        // including when switching direction and velocity is not 0 or not moving
        else
        {
            moveVector.x = Mathf.Lerp(rb.velocity.x, 0, moveRate);
        }
        
        // relative movement since the last frame, as we are calculating physics in Update
        // (as opposed to Fixed Update)
        //moveVector.x *= Time.deltaTime;

        if ( Input.GetButton("Jump") && (isGrounded) )
        {
            moveVector.y = jumpSpeed;
            rb.gravityScale = 1f;
        }
        if ( Input.GetButton("Jump") )
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        rb.velocity = moveVector;
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
                actualIsGrounded = true;
                isGrounded = true;
                return;
            }
            else
            {
                actualIsGrounded = false;
                if (rb.velocity.y != 0)
                {
                    coyoteTimer -= Time.deltaTime;
                }
            }
        }

        if (actualIsGrounded)
        {
            // rb.velocity = new Vector2(rb.velocity.x, 0f);
            coyoteTimer = coyoteTime;
        }

        actualIsGrounded = false;

        if (coyoteTimer > 0)
        {
            isGrounded = true;
            return;
        }

        isGrounded = false;
    }
    public void ResetValues()
    {
        // here the states are reset to their original forms
        moveClamp = defaultMoveClamp;
        moveRate = defaultMoveRate;
        jumpSpeed = defaultJumpSpeed;
    }
}
