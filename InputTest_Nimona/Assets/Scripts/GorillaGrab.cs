using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GorillaGrab : MonoBehaviour
{
    [SerializeField] private float throwForce;
    [SerializeField] private Collider2D grabCollider;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private KeyCode grabKey;
    [SerializeField] private KeyCode throwKey;
    [SerializeField] private GameObject playerObject;
    private GameObject grabObject;
    private GameObject grabingObject;
    public bool IsGrabbing { get; private set;  } = false;
    public bool IsThrowing { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if isGrabbing is true
        if (IsGrabbing)
        {
            // If true execute GrabingObject()
            GrabingObject();
            ThrowObject();
        }
        // Check if grabObject is not null
        if (grabObject != null)
        {
            // Check for grabKey input and if isGrabbing is false
            if (Input.GetKeyDown(grabKey) && !IsGrabbing)
            {
                // If true set isGrabbing as true
                IsGrabbing = true;
                // Set grabbingObject as grabObject
                grabingObject = grabObject;
            }
        }

    }
    // Check for grabbables in a specified area
    private void OnTriggerStay2D (Collider2D other)
    {
        Debug.Log("START");
        Debug.Log($"other: {other}");
        // Initialize grab vector
        Vector3 grabVector;

        // Check if grabObject is null
        if (grabObject == null)
        {
            Debug.Log("STEP 1");
            // If true check if other is a grabbable
            if (other.CompareTag("Grabbable"))
            {
                Debug.Log("STEP 2");
                // If true set other as grabObject
                grabObject = other.gameObject;
            }
        }
        else
        {
            // If false check if other is a grabbable
            if (other.CompareTag("Grabbable"))
            {
                // If true do as follows
                grabVector = grabObject.transform.position; // Get the current grabObject position
                Debug.Log("STEP 1");
                Vector3 otherVector = other.transform.position; // Get other object position

                Debug.Log($"{Vector3.Distance(otherVector, transform.position)} < {Vector3.Distance(grabVector, transform.position)} ? ");

                // Check if other is closer to the player then the current grabObject
                if (Vector3.Distance(otherVector, transform.position) < Vector3.Distance(grabVector, transform.position))
                {
                    // If true set other as grabObject and update grabVector
                    Debug.Log("TRUE");
                    Debug.Log("STEP 2");
                    grabObject = other.gameObject;
                    grabVector = grabObject.transform.position;
                }
            }
        }
        Debug.Log("DONE");
        Debug.Log("grab: " + grabObject);

    }
    // Grabbing mechanic logic
    private void GrabingObject()
    {
        // Set grabbingObject position as teh same as grabPoint
        grabingObject.transform.position = grabPoint.transform.position;
        // Freeze grabbingObject rotation
        grabingObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        // Check for input in grabKey
        if (Input.GetKeyDown(grabKey))
        {
            // Apply a forward vector to the grabbingObject position
            grabingObject.transform.position += new Vector3(transform.right.x * 10f, 0f, 0f);
            UnGrabObject();
        }
    }
    private void ThrowObject()
    {
        float rJoystickX = Input.GetAxis("RotationX");
        float rJoystickY = Input.GetAxis("RotationY");

        if (Input.GetKeyDown(throwKey))
        {
            playerObject.GetComponent<Shapeshifting>().enabled = false;
        }
        if (Input.GetKey(throwKey))
        {
            Debug.Log("THROWWW");
            
            float zRotation = Mathf.Atan2(rJoystickY, -rJoystickX)*Mathf.Rad2Deg;

            Debug.Log($"Angle: {zRotation}");

            if (zRotation > 10f && zRotation < 170f)
            {
                grabingObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            }
        }
        if (Input.GetKeyUp(throwKey))
        {
            grabingObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-rJoystickX * throwForce, rJoystickY * throwForce),ForceMode2D.Impulse);
            UnGrabObject();
            Invoke("TurnOnShapeshift", 0.5f);
        }
    }
    private void TurnOnShapeshift()
    {
        playerObject.GetComponent<Shapeshifting>().enabled = true;
    }
    private void UnGrabObject()
    {
        // Unfreeze grabbingObject rotation
        grabingObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        // Set isGrabbing to false
        IsGrabbing = false;
        // Set grabingObject and grabObject as null
        grabingObject = null;
        grabObject = null;
    }
}
