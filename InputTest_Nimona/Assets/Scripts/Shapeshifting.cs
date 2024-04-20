using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapeshifting : MonoBehaviour
{
    [SerializeField] private GameObject rhino;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject dragon;
    [SerializeField] private GameObject gorilla;

    [SerializeField] private BoxCollider2D groundCheckCollider;

    private GameObject currentShape;
    void Start()
    {
        // Initialize with human shape at the start
        ChangeShape(human);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeShape(rhino);
            /*
            rhino.SetActive(true);

            human.SetActive(false);
            dragon.SetActive(false);
            gorilla.SetActive(false);
            */
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeShape(human);
            /*
            human.SetActive(true);

            rhino.SetActive(false);
            dragon.SetActive(false);
            gorilla.SetActive(false);
            */
        }
        
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeShape(dragon);
            /*
            dragon.SetActive(true);

            rhino.SetActive(false);
            human.SetActive(false);
            gorilla.SetActive(false);
            */
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeShape(gorilla);
            /*
            gorilla.SetActive(true);

            rhino.SetActive(false);
            human.SetActive(false);
            dragon.SetActive(false);
            */
        }
    }
    public void ChangeShape(GameObject newShape)
    {
        if (newShape == currentShape) return; // Don't switch to the same shape

        // reset default values
        Movement movement = GetComponent<Movement>();
        if (movement != null) movement.ResetValues();

        if (currentShape != null)
        {
            currentShape.SetActive(false); // Deactivate current shape
        }

        newShape.SetActive(true); // Activate new shape
        currentShape = newShape; // Update current shape reference

        // Update the ground check collider size and offset
        CapsuleCollider2D newGroundCollider;
        newGroundCollider = newShape.GetComponent<CapsuleCollider2D>();


        if (newGroundCollider != null && groundCheckCollider != null)
        {
            Vector2 newSize;
            Vector2 newOffset;

            newSize = new Vector2(newGroundCollider.size.x, groundCheckCollider.size.y);
            newOffset = new Vector2(newGroundCollider.offset.x, groundCheckCollider.offset.y);

            groundCheckCollider.size = newSize;
            groundCheckCollider.offset = newOffset;
        }

        // Update movement colliders
        if (movement != null)
        {
            movement.groundCollider = newGroundCollider;
            movement.airCollider = newShape.GetComponent<BoxCollider2D>();
        }
    }
}
