using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float jumpSpeed = 200f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private float maxJumpTime = 0.1f;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D airCollider;
    private Rigidbody2D rb;
    private float defaultGravity;
    private float jumpTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = IsGrounded();

        groundCollider.enabled = isGrounded;
        airCollider.enabled = !isGrounded;

        float deltaX = Input.GetAxis("Horizontal");
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.x = deltaX * moveSpeed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jumping");
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 1.0f;
            jumpTime = Time.time;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - jumpTime) < maxJumpTime))
        {
            rb.gravityScale = 1.0f;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, 0);

    }
    private bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayers);

        return collider != null;
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            if (!IsGrounded())
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
