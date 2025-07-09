using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float jumpSpeed = 200f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private float maxJumpTime = 0.1f;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D airCollider;
    private int currentHealth;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private float defaultGravity;
    private float jumpTime;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        defaultGravity = rb.gravityScale;
        
        Debug.Log($"Initializig {name}...");
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded)
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = new Color(1f,0f,0f,1f);
        }

        groundCollider.enabled = isGrounded;
        airCollider.enabled = !isGrounded;

        float deltaX = Input.GetAxis("Horizontal");

        // Vector3 moveVector = new Vector3(deltaX * moveSpeed * Time.deltaTime,0f,0f);
        // transform.position += moveVector;

        Vector3 currentVelocity = rb.velocity;

        currentVelocity.x = deltaX * moveSpeed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
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

        rb.velocity = new Vector3(currentVelocity.x,currentVelocity.y,0);

        //Animation
        //anim.SetFloat("AbsVelocityX", MathF.Abs(currentVelocity.x));

        //if ((currentVelocity.x < 0) && (transform.right.x > 0)) transform.rotation = Quaternion.Euler(0, 180, 0);
        //else if ((currentVelocity.x > 0) && (transform.right.x < 0)) transform.rotation = Quaternion.identity;
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
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position,groundCheckRadius);
        }
    }
}
