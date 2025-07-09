using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWings : MonoBehaviour , IShapeColliders 
{
    private PlayerActions playerActions;

    private PlayerSounds playerSounds;
    private SoundsScript audioPlayer;

    // reference variables to change in player
    [field:SerializeField] public CapsuleCollider2D GroundCollider { get; set; }
    [field:SerializeField] public BoxCollider2D AirCollider { get; set; }
    [field:SerializeField] public BoxCollider2D GroundCheckCollider { get; set; }

    [SerializeField] private float jumpSpeed = 200f;
    [SerializeField] private float glideClamp = 400f;
    // private float glideRate;
    [SerializeField] private int jumpsAllowed = 3;
    [SerializeField] private float maxGlideTime = 5;
    [SerializeField] private float maxFallSpeed = 20f;

    
    private float glideTimer;
    private int jumpsExecuted;
    private Vector3 newJump;
    private Vector3 currentVelocity;

    // player variables
    private GameObject player;
    private Movement movement;
    private Rigidbody2D rb;
    private Animator anim;

    // Shapeshift points mechanic
    private Shapeshifting shpshift;

    void Start()
    {
        audioPlayer = GetComponentInParent<SoundsScript>();
        playerSounds = GetComponentInParent<PlayerSounds>();

        player = transform.parent.gameObject;
        movement = player.GetComponentInParent<Movement>();
        rb = player.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpsExecuted = 0;
        glideTimer = maxGlideTime;

        // glideRate = (movement.DefaultMaxSpeed / glideClamp) * movement.DefaultMoveRate;
        
        movement.JumpSpeed = jumpSpeed;

        playerActions = GetComponentInParent<PlayerActions>();
    }
    void OnEnable()
    {
        shpshift = GetComponentInParent<Shapeshifting>();
        Debug.Log($"GET SHAPESHIFTING : {shpshift}");
        shpshift.DragonPoints -= 1;
    }
    void Update()
    {
        // If not jumping change the gravity scale
        if ( ! playerActions.Jump.IsPressed )
        {
            rb.gravityScale = movement.DefaultWalkingGravity;
        }

        if (!movement.IsGrounded)
        {
            if ( jumpsExecuted < jumpsAllowed && playerActions.Jump.WasPressed )
            {
                Debug.Log("has flapped");

                anim.SetTrigger("Flap");

                rb.gravityScale = movement.FallingGravity;

                newJump = rb.velocity;
                newJump.y = jumpSpeed;

                rb.velocity = newJump;

                audioPlayer.SoundToPlay = playerSounds.Flap;
                audioPlayer.PlayAudio();

                jumpsExecuted++;
                Debug.Log(jumpsExecuted + " jumps");
            }
            else if ( playerActions.Ability.IsPressed && (glideTimer > 0) )
            {
                if ( (movement.MaxSpeed != glideClamp) /*|| (movement.MoveRate != glideRate)*/ )
                {
                    movement.MaxSpeed = glideClamp;
                    //movement.MoveRate = glideRate;
                }
                
                currentVelocity = rb.velocity;
                if (rb.velocity.y < -maxFallSpeed)
                {
                    currentVelocity.y = -maxFallSpeed;
                    rb.velocity = currentVelocity;
                }

                movement.CurrentRun = playerSounds.Glide;
                movement.IsGliding = true;

                glideTimer -= Time.deltaTime;
            }

            if ( !playerActions.Ability.IsPressed )
            {
                if ( (movement.MaxSpeed == glideClamp) /*|| (movement.MoveRate == glideRate)*/ )
                {
                    movement.MaxSpeed = movement.DefaultMaxSpeed;
                    movement.IsGliding = false;
                    movement.CurrentRun = playerSounds.Walk;
                    // movement.MoveRate = movement.DefaultMoveRate;
                }
            }

            movement.Jumped = false;
        }
        else if ( movement.MaxSpeed == glideClamp )
        {
            movement.MaxSpeed = movement.DefaultMaxSpeed;
            movement.IsGliding = false;
            movement.CurrentRun = playerSounds.Walk;
        }
        
        if ( movement.GroundScore )
        {
            Debug.Log("reset ground scroer");
            jumpsExecuted = 0;
            glideTimer = maxGlideTime;
            movement.GroundScore = false;
        }

    }
    void OnDisable()
    {
        movement.CurrentRun = playerSounds.Walk;
        movement.IsGliding = false;
    }
}
