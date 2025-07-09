using UnityEngine;

public class GorillaGrab : MonoBehaviour
{
    private PlayerActions playerActions;

    private PlayerSounds playerSounds;
    private SoundsScript audioPlayer;

    [SerializeField] private float throwForce;
    [SerializeField] private float grabRadius = 30;
    [SerializeField] private CircleCollider2D grabCollider;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private GameObject playerObject;
    private GameObject grabObject;
    private GameObject grabbingObject;
    private Animator anim;
    public bool IsGrabbing { get; private set; } = false;
    public bool IsThrowing { get; private set; } = false;

    // Shapeshift points mechanic
    private Shapeshifting shpshift;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponentInParent<SoundsScript>();
        playerSounds = GetComponentInParent<PlayerSounds>();
        anim = GetComponentInParent<Animator>();

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
        if ( playerActions.Throw.WasPressed && grabObject != null )
        {
            audioPlayer.SoundToPlay = playerSounds.Pickup;
            audioPlayer.PlayAudio();

            anim.SetTrigger("Grab");
            Debug.Log("Grabbed");
            Debug.Log(IsGrabbing = true);
            Debug.Log(grabObject);
            IsGrabbing = true;
            grabbingObject = grabObject;
            grabObject = null;
            playerObject.GetComponent<Shapeshifting>().enabled = false;
        }
        else if (IsGrabbing && grabbingObject != null)
        {
            GrabbingObject();
        }
    }

    // Check for grabbables in a specified area
    private void OnTriggerEnter2D(Collider2D other)
    {
        Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();

        if ( grabObject == null && grabbable != null )
        {
            Debug.Log("Grabbed");
            grabObject = other.gameObject;
        }
    }

    // Check for exiting grabbable area
    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.gameObject == grabObject )
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

        grabbingObject.transform.position += new Vector3(transform.right.x * 10f, 0f, 0f);
        if (playerActions.Throw.WasReleased)
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
        if ( rb != null )
        {
            Vector2 throwDirection = new Vector2(Xinput, Yinput);
            throwDirection *= throwForce;
            rb.velocity = throwDirection;

            audioPlayer.SoundToPlay = playerSounds.Throw;
            audioPlayer.PlayAudio();
            anim.SetTrigger("Throw");
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
        Debug.Log("Ungrabbed");
    }
}
