using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private KnightSounds knightSounds;
    private SoundsScript audioPlayer;

    private Animator anim;
    private GameObject target;
    private Death deathScript;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<SoundsScript>();
        knightSounds = GetComponent<KnightSounds>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(target);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponentInParent<Movement>();

        if (movement != null)
        {
            target = other.gameObject;
            deathScript = target.GetComponentInParent<Death>();

            audioPlayer.SoundToPlay = knightSounds.Swipe;
            audioPlayer.PlayAudio();

            anim.SetTrigger("Attack");
        }

        Grabbable grabbable = 
            other.gameObject.GetComponent<Grabbable>();

        if ( grabbable != null )
        {
            KnightMovement knightMov = GetComponent<KnightMovement>();
            knightMov.DieSequence();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponentInParent<Movement>();

        if (movement != null)
        {
            target = null;
            deathScript = null;
        }
    }
    public void Kill()
    {
        deathScript.GameOver();
    }
}
