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
        // Initialize player actions and set grab collider radius
        playerActions = GetComponentInParent<PlayerActions>();
        grabCollider.radius = grabRadius;
    }

    // Called when the script is enabled
    void OnEnable()
    {
        // Initialize shapeshifting component and decrease Gorilla points
        shpshift = GetComponentInParent<Shapeshifting>();
        shpshift.GorillaPoints -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for throw input
        if (playerActions.Throw.WasPressed)
        {
            IsGrabbing = true;
            grabbingObject = grabObject;
            playerObject.GetComponent<Shapeshifting>().enabled = false;
        }
        else if (IsGrabbing && grabObject != null)
        {
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
        Vector3 grabVector;

        Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();

        if (grabObject == null)
        {
            if (grabbable != null)
            {
                grabObject = other.gameObject;
            }
        }
        else
        {
            if (grabbable != null)
            {
                grabVector = grabObject.transform.position;
                Vector3 otherVector = other.transform.position;

                if (Vector3.Distance(otherVector, transform.position) < Vector3.Distance(grabVector, transform.position))
                {
                    grabObject = other.gameObject;
                    grabVector = grabObject.transform.position;
                }
            }
        }
    }

    // Check for exiting grabbable area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == grabObject)
        {
            grabObject = null;
        }
    }

    // Grabbing mechanic logic
    private void GrabbingObject()
    {
        grabbingObject.transform.position = grabPoint.transform.position;
        grabbingObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        float rJoystickX = playerActions.AimX.Value;
        float rJoystickY = playerActions.AimY.Value;

        float zRotation = Mathf.Atan2(rJoystickY, rJoystickX) * Mathf.Rad2Deg;
        grabbingObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);

        if (playerActions.Throw.WasPressed)
        {
            grabbingObject.transform.position += new Vector3(transform.right.x * 10f, 0f, 0f);
            UnGrabObject();
        }
        else if (playerActions.Throw.WasReleased)
        {
            ThrowObject(rJoystickX, rJoystickY);
            UnGrabObject();
            TurnOnShapeshift();
        }
    }

    // Throw the grabbed object
    private void ThrowObject(float Xinput, float Yinput)
    {
        Rigidbody2D rb = grabbingObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 throwDirection = new Vector2(Xinput, Yinput);
            throwDirection *= throwForce;
            if (throwDirection.y > 0) throwDirection.y *= 1.5f;
            rb.AddForce(throwDirection, ForceMode2D.Impulse);
        }
    }

    // Enable shapeshifting
    private void TurnOnShapeshift()
    {
        playerObject.GetComponent<Shapeshifting>().enabled = true;
    }

    // Ungrab the object
    private void UnGrabObject()
    {
        grabbingObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        IsGrabbing = false;
        grabbingObject = null;
        grabObject = null;
    }
}
