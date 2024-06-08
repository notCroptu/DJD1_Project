using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator anim;
    private GameObject target;
    private Death deathScript;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(target);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponentInParent<Movement>();

        if (movement != null)
        {
            target = other.gameObject;
            deathScript = target.GetComponentInParent<Death>();

            anim.SetTrigger("Attack");
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponent<Movement>();

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
