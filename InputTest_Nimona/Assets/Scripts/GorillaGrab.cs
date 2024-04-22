using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GorillaGrab : MonoBehaviour
{
    [SerializeField] private Collider2D grabCollider;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private KeyCode useKey;
    private GameObject grabObject;
    private GameObject grabingObject;
    private bool isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing)
        {
            grabingObject.transform.position = grabPoint.transform.position;
            if (Input.GetKeyDown(useKey))
            {
                grabingObject.transform.position += new Vector3(transform.right.x*10f, 0f, 0f);
                isGrabbing = false;
                grabingObject = null;
                grabObject = null;
            }
        }
        if (grabObject != null)
        {
            if (Input.GetKeyDown(useKey) && !isGrabbing)
            {
                isGrabbing = true;
                grabingObject = grabObject;
            }
        }
       
    }
    private void OnTriggerStay2D (Collider2D other)
    {
        Debug.Log("START");
        Debug.Log($"other: {other}");

        Vector3 grabVector;

        if (grabObject == null)
        {
            Debug.Log("STEP 1");
            if (other.CompareTag("Grabbable"))
            {
                Debug.Log("STEP 2");
                grabObject = other.gameObject;
            }
        }
        else
        {
            if (other.CompareTag("Grabbable"))
            {
                grabVector = grabObject.transform.position;
                Debug.Log("STEP 1");
                Vector3 otherVector = other.transform.position;

                Debug.Log($"{Vector3.Distance(otherVector, transform.position)} < {Vector3.Distance(grabVector, transform.position)} ? ");
                if (Vector3.Distance(otherVector, transform.position) < Vector3.Distance(grabVector, transform.position))
                {
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

}
