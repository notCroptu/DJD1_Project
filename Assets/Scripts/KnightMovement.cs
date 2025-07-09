using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    private KnightSounds knightSounds;
    private SoundsScript audioPlayer;

    [SerializeField] private LayerMask excludeLayersOnDie;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float stopDistance = 128f;
    [SerializeField] private float sightDistance = 256f;
    [SerializeField] private float enemyScore = 50f;
    [SerializeField] private LayerMask collidables;
    [SerializeField] private PhysicsMaterial2D bouncy;

    private Rigidbody2D rb;
    private Rigidbody2D rbP;
    private Animator anim;
    private Movement player;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 bufferVelocity;
    private bool dead;

    void Start()
    {
        audioPlayer = GetComponent<SoundsScript>();
        knightSounds = GetComponent<KnightSounds>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Movement>();
            if (player != null)
            {
                rbP = player.GetComponent<Rigidbody2D>();
            }
        }
        else if (rbP != null && !dead)
        {
            Vector2 dirToPlayer = (rbP.position - new Vector2(0f, 10f)) - rb.position;
            dirToPlayer = dirToPlayer.normalized;

            // Get the center of the capsule collider
            Vector2 currentPosition = capsuleCollider.bounds.center;
            Vector2 distance = currentPosition + new Vector2(stopDistance * Mathf.Sign(dirToPlayer.x), 0f);

            // Perform the raycast and log the result
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, dirToPlayer, sightDistance, collidables);
            //Debug.Log($"Raycast hit: {hit.collider != null}");

            // Visualize the raycast in the Scene view
            //Debug.DrawRay(currentPosition, dirToPlayer * sightDistance, Color.red);

            if (hit.collider != null)
            {
                Movement hitPlayer = hit.collider.gameObject.GetComponentInParent<Movement>();
                if (hitPlayer != null)
                {
                    //Debug.Log("Player detected!");

                    float distanceToPlayer = Vector2.Distance(rb.position, rbP.position);

                    if ( distanceToPlayer > stopDistance )
                    {
                        // Animation
                        if (dirToPlayer.x < 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else if (dirToPlayer.x > 0)
                        {
                            transform.rotation = Quaternion.identity;
                        }

                        Vector2 newVelocity = rb.velocity;
                        newVelocity.x = Mathf.Lerp(rb.velocity.x, speed * Mathf.Sign(dirToPlayer.x), 0.3f);
                        rb.velocity = newVelocity;    

                        // Log velocity change
                        //Debug.Log($"New velocity: {rb.velocity}");
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                    
                }
            }
            // Animation
            anim.SetFloat("AbsVelocity",Mathf.Abs(rb.velocity.x));
            
            // Update the bufferVelocity
            bufferVelocity = rbP.velocity;
        }
    }

    public void DieSequence()
    {
        dead = true;
        anim.SetTrigger("Die");

        Sword sword = GetComponent<Sword>();
        Shield shield = GetComponent<Shield>();

        if (sword != null) sword.enabled = false;
        if (shield != null) shield.enabled = false;

        StartCoroutine(DieSequenceCR());
    }

    IEnumerator DieSequenceCR()
    {
        audioPlayer.SoundToPlay = knightSounds.Death;
        audioPlayer.PlayAudio();

        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = excludeLayersOnDie;
        rb.sharedMaterial = bouncy;

        // Vector2 bounce = new Vector2(bufferVelocity.x, bufferVelocity.y + 100f * rb.mass);
        Vector2 bounce = new Vector2(bufferVelocity.x, 0f);
        rb.AddForce(bounce, ForceMode2D.Impulse);
        rbP.AddForce(bufferVelocity, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.2f);

        PlayerScore.ChangeScore(enemyScore);
        Destroy(gameObject);
    }
}
