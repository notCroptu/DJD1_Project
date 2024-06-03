using UnityEngine;

public class GorillaGrab : MonoBehaviour
{
    PlayerActions playerActions;
    [SerializeField] private float throwForce;
    [SerializeField] private float grabRadius = 30;
    [SerializeField] private CircleCollider2D grabCollider;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private GameObject playerObject;
    private GameObject grabObject;
    private GameObject grabbingObject;
    public bool IsGrabbing { get; private set; } = false;
    public bool IsThrowing { get; private set; } = false;

    // Shapeshift points mechanic
    private Shapeshifting shpshift;

    // Start is called before the first frame update
    void Start()
    {
        playerActions = GetComponentInParent<PlayerActions>();
        grabCollider.radius = grabRadius;
    }

    void OnEnable()
    {
        shpshift = GetComponentInParent<Shapeshifting>();
        //Debug.Log($"GET SHAPESHIFTING : {shpshift}");
        shpshift.GorillaPoints -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActions.Throw.WasPressed)
        {
            // If true set isGrabbing as true
            IsGrabbing = true;
            // Set grabbingObject as grabObject
            grabbingObject = grabObject;
            // Disable shapeshifting
            playerObject.GetComponent<Shapeshifting>().enabled = false;
        }
        else if (IsGrabbing && grabObject != null)
        {
            // If true execute GrabbingObject()
            GrabbingObject();
        }
        else
        {
            grabbingObject = null;
        }
    }

    // Check for grabbables in a specified area
    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("START");
        //Debug.Log($"other: {other}");
        // Initialize grab vector
        Vector3 grabVector;

        Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();

        // Check if grabObject is null
        if (grabObject == null)
        {
            Debug.Log("STEP 1");
            // If true check if other is a grabbable
            if (grabbable != null)
            {
                Debug.Log("STEP 2");
                // If true set other as grabObject
                grabObject = other.gameObject;
            }
        }
        else
        {
            // If false check if other is a grabbable
            if (grabbable != null)
            {
                // If true do as follows
                grabVector = grabObject.transform.position; // Get the current grabObject position
                Debug.Log("STEP 1");
                Vector3 otherVector = other.transform.position; // Get other object position

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

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("OBJECT LEFT");
        if (other == grabObject)
        {
            Debug.Log("IS GRAB");
            grabObject = null;
        }
    }

    // Grabbing mechanic logic
    private void GrabbingObject()
    {
        // Set grabbingObject position as the same as grabPoint
        grabbingObject.transform.position = grabPoint.transform.position;
        // Freeze grabbingObject rotation
        grabbingObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        float rJoystickX = playerActions.AimX.Value;
        float rJoystickY = playerActions.AimY.Value;

        //Debug.Log("x: " + rJoystickX + " y: " + rJoystickY);

        
        float zRotation = Mathf.Atan2(rJoystickY, rJoystickX) * Mathf.Rad2Deg;

        //Debug.Log($"Angle: {zRotation}");

        grabbingObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);

        if (playerActions.Throw.WasPressed) // playerActions.Ability.WasPressed 
        {
            // Apply a forward vector to the grabbingObject position
            grabbingObject.transform.position += new Vector3(transform.right.x * 10f, 0f, 0f);
            UnGrabObject();
        }
        else if (playerActions.Throw.WasReleased)
        {
            // Throw and release the object
            ThrowObject(rJoystickX, rJoystickY);
            UnGrabObject();
            TurnOnShapeshift();
        }
    }

    private void ThrowObject(float Xinput, float Yinput)
    {
        Rigidbody2D rb = grabbingObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 throwDirection = new Vector2(Xinput, Yinput);
            throwDirection *= throwForce;
            if ( throwDirection.y > 0) throwDirection.y *= 2;
            rb.AddForce(throwDirection, ForceMode2D.Impulse);
        }
    }

    private void TurnOnShapeshift()
    {
        playerObject.GetComponent<Shapeshifting>().enabled = true;
    }

    private void UnGrabObject()
    {
        // Unfreeze grabbingObject rotation
        grabbingObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        // Set IsGrabbing to false
        IsGrabbing = false;
        // Set grabbingObject and grabObject as null
        grabbingObject = null;
        grabObject = null;
    }
}
