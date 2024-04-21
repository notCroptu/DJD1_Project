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
        float rJoystickX = Input.GetAxis("JoyStickR_X");
        float rJoystickY = Input.GetAxis("JoyStickR_Y");

        if (rJoystickX == 1)
        {
            ChangeShape(rhino);
        }

        else if (rJoystickY == -1)
        {
            ChangeShape(human);
        }
        
        else if (rJoystickY == 1)
        {
            ChangeShape(dragon);
        }

        else if (rJoystickX == -1)
        {
            ChangeShape(gorilla);
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
        BoxCollider2D newAirCollider;
        newAirCollider = newShape.GetComponent<BoxCollider2D>();


        if (newAirCollider != null && groundCheckCollider != null)
        {
            Vector2 newSize;
            Vector2 newOffset;

            newSize = new Vector2(newAirCollider.size.x - 5f, groundCheckCollider.size.y);
            newOffset = new Vector2(newAirCollider.offset.x, groundCheckCollider.offset.y);

            groundCheckCollider.size = newSize;
            groundCheckCollider.offset = newOffset;
        }

        // Update movement colliders
        if (movement != null)
        {
            movement.groundCollider = newShape.GetComponent<CapsuleCollider2D>();
            movement.airCollider = newAirCollider;
        }
    }
}
