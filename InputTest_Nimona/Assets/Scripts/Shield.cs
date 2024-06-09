using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private KnightSounds knightSounds;
    private SoundsScript audioPlayer;

    [SerializeField] private float knockback = 2;

    private Rigidbody2D rb;
    private Rigidbody2D rbP;
    private Movement player;
    private GameObject rhino;
    private Vector2 bufferVelocity;
    private float clamp;
    void Start()
    {
        audioPlayer = GetComponent<SoundsScript>();
        knightSounds = GetComponent<KnightSounds>();

        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if ( collision.gameObject.CompareTag("Player") )
        {
            if ( rhino == null )
            {
                rhino = collision.gameObject.transform.Find("Rhino").gameObject;
            }

            clamp = rhino.GetComponent<Rhino>().RunClamp;

            Vector2 dirToPlayer = rbP.position - rb.position;
            dirToPlayer = dirToPlayer.normalized;

            float parryForce = Mathf.Sign(bufferVelocity.x) * clamp - bufferVelocity.x;
            parryForce *= knockback;
            Vector2 newVelocity = new Vector2(- parryForce, 0f);

            audioPlayer.SoundToPlay = knightSounds.Parry;
            audioPlayer.PlayAudio();

            player.ImpulsePlayer(newVelocity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( rbP != null )
        {
            bufferVelocity = rbP.velocity;
        }

        if ( player == null )
        {
            player = FindObjectOfType<Movement>();
            rbP = player.GetComponent<Rigidbody2D>();
        }
    }
}
