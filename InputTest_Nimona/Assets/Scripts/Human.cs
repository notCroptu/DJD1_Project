using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private BoxCollider2D airCollider;

    private GameObject player;
    void OnEnable()
    {
        /*
        player = transform.parent.gameObject;

        // updating the ground check collider to accomodate for size change using the other colliders sizes
        BoxCollider2D playerGCC = player.GetComponent<BoxCollider2D>();

        Vector2 newSize;
        Vector2 newOffset;

        newSize = new Vector2(groundCollider.size.x, playerGCC.size.y);
        newOffset = new Vector2(groundCollider.offset.x, playerGCC.offset.y);

        playerGCC.size = newSize;
        playerGCC.offset = newOffset;

        // setting the new ground and air colliders
        CapsuleCollider2D playerGC = player.GetComponent<Movement>().groundCollider;

        playerGC = groundCollider;

        BoxCollider2D playerAC = player.GetComponent<Movement>().airCollider;

        playerAC = airCollider;
        */
    }
}
