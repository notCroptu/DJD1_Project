using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Movement : MonoBehaviour
{
    private PlayerActions playerActions;

    private PlayerSounds playerSounds;
    private SoundsScript audioPlayer;
    private AudioSource audioSource;
    public AudioClip CurrentRun { get; set; }

    // class variables that will be changed throughout shapes
    [field:SerializeField] public BoxCollider2D GroundCheckCollider { get; set; }
    [field:SerializeField] public CapsuleCollider2D GroundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D AirCollider { get; set; }

    // other variables that will be changed throughout shapes (the field serialize is purely for inspector viewing of variable changes)
    public float MaxSpeed { get; set; }
    // public float MoveRate { get; set; }
    public float JumpSpeed { get; set; }
    public float FallingGravity { get; set; }

    // The previous variable's default states and their getters
    [SerializeField] private float defaultMaxSpeed = 100f;
    public float DefaultMaxSpeed => defaultMaxSpeed;
    /*[SerializeField] private float defaultMoveRate = 0.9f;
    public float DefaultMoveRate => defaultMoveRate;*/
    [SerializeField] private float defaultJumpSpeed = 200f;
    public float DefaultJumpSpeed => defaultJumpSpeed;
    [SerializeField] private float defaultFallingGravity = 2f;
    public float DefaultFallingGravity => defaultFallingGravity;
    [SerializeField] private float defaultWalkingGravity = 4f;
    public float DefaultWalkingGravity => defaultWalkingGravity;
    [SerializeField] private float acceleration = 8f;
    public float Acceleration => acceleration;


    // variables that will be the same throughout shapes
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float knockOutTime = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 moveVector;
    public bool IsGrounded { get; set; }
    public bool GroundScore { get; set; }
    private bool canJump;
    private float coyoteTimer;

    public bool Jumped { get; set; }
    private bool isJumping;
    private float deltaX;
    private bool inputEnabled;

    public bool IsGliding { get; set; } = false;
    void Start()
    {
        inputEnabled = true;
        rb = GetComponent<Rigidbody2D>();

        playerActions = GetComponent<PlayerActions>();

        audioPlayer = GetComponent<SoundsScript>();
        playerSounds = GetComponent<PlayerSounds>();
        audioSource = GetComponent<AudioSource>();
        CurrentRun = playerSounds.Walk;
    }
    void OnEnable()
    {
        GroundScore = true;
    }

    void FixedUpdate()
    {
        UpdateGroundState();

        // change from air to ground collider or ground to air collider
        GroundCollider.enabled = IsGrounded;
        AirCollider.enabled = !IsGrounded;

        anim.SetBool("isGrounded",IsGrounded);

        if ( IsGrounded ) GroundScore = true;

        if ( inputEnabled )
        {
            moveVector = rb.velocity;

            // moveVector.x = Mathf.Lerp(rb.velocity.x, MaxSpeed * deltaX, MoveRate);
            moveVector.x += deltaX * acceleration;

            if ( Mathf.Abs(moveVector.x) > MaxSpeed )
            {
                moveVector.x -= Mathf.Sign(rb.velocity.x) * acceleration * 1.2f;
            }

            //moveVector.x = Mathf.Clamp(moveVector.x, -MaxSpeed, MaxSpeed);

            if ( Jumped && canJump && IsGrounded )
            {
                audioPlayer.SoundToPlay = playerSounds.Jump;
                audioPlayer.PlayAudio();

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
                rb.gravityScale = DefaultWalkingGravity;
                Jumped = false;
            }

            rb.velocity = moveVector;

            // Animator
            anim.SetFloat("AbsVelocity",Mathf.Abs(moveVector.x));
            anim.SetFloat("VelocityY",moveVector.y);

            if ( (Mathf.Abs(rb.velocity.x) > 10 && IsGrounded) || IsGliding)
            {
                audioSource.clip = CurrentRun;
                audioSource.enabled = true;
                Debug.Log("is moving");
            }
            else
            {
                audioSource.enabled = false;
            }
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
        if (GroundCheckCollider)
        {
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useLayerMask = true;
            contactFilter.layerMask = groundLayerMask;

            Collider2D[] results = new Collider2D[128];

            int n = Physics2D.OverlapCollider(GroundCheckCollider, contactFilter, results);
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
    public void ChangeAnimator(Animator newAnim)
    {
        anim = newAnim;
    }
    public void ResetValues()
    {
        // here the states are reset to their original forms
        MaxSpeed = defaultMaxSpeed;
        // MoveRate = defaultMoveRate;
        JumpSpeed = defaultJumpSpeed;
        FallingGravity = defaultFallingGravity;
    }
}
