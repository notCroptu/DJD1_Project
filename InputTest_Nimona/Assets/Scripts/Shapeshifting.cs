using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shapeshifting : MonoBehaviour
{
    PlayerActions playerActions;
    [SerializeField] private GameObject rhino;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject dragon;
    [SerializeField] private GameObject gorilla;

    [SerializeField] private BoxCollider2D groundCheckCollider;

    // GD2 Shapeshift points test mechanic
    [SerializeField] private bool testShapeshiftPoints = true;
    [SerializeField] private Image gorillaBar;
    [SerializeField] private Image dragonBar;
    [SerializeField] private Image rhinoBar;
    [SerializeField] private float maxPoints; 
    public float GorillaPoints { get; set; }
    public float DragonPoints { get; set; }
    public float RhinoPoints { get; set; }

    private GameObject currentShape;
    [SerializeField] private Movement movement;

    [SerializeField] private ParticleSystem shapeParticleSystem;
    [SerializeField] private ParticleSystemForceField forceField;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;
    private SpriteRenderer spriteRenderer;
    private float ParticleEffectArea;
    [SerializeField] private Gradient color;
    void Start()
    {
        // Initialize with human shape at the start
        ChangeShape(human);
        RhinoPoints = maxPoints;
        GorillaPoints = maxPoints;
        DragonPoints = maxPoints;

        playerActions = GetComponent<PlayerActions>();
    }
    void Update()
    {
        float rJoystickX = playerActions.ShapeshiftX;
        float rJoystickY = playerActions.ShapeshiftY;

        Debug.Log(rJoystickX);

        if (testShapeshiftPoints)
        {
            if ((rJoystickX > 0.71f || Input.GetKeyDown(KeyCode.Alpha1)) && RhinoPoints > 0)
            {
                ChangeShape(rhino);
            }
            else if (rJoystickY < -0.71f || Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeShape(human);
            }
            else if ((rJoystickY > 0.71f || Input.GetKeyDown(KeyCode.Alpha2)) && DragonPoints > 0)
            {
                ChangeShape(dragon);
            }
            else if ((rJoystickX < -0.71f || Input.GetKeyDown(KeyCode.Alpha3)) && GorillaPoints > 0)
            {
                ChangeShape(gorilla);
            }
        }
        else
        {
            if (rJoystickX > 0.71f)
            {
                ChangeShape(rhino);
            }
            else if (rJoystickY < -0.71f)
            {
                ChangeShape(human);
            }
            else if (rJoystickY > 0.71f)
            {
                ChangeShape(dragon);
            }
            else if (rJoystickX < -0.71f)
            {
                ChangeShape(gorilla);
            }
        }
        
        UpdateBars(GorillaPoints,gorillaBar);
        UpdateBars(DragonPoints,dragonBar);
        UpdateBars(RhinoPoints,rhinoBar);
    }
    public void ChangeShape(GameObject newShape)
    {
        if (newShape == currentShape) return; // Don't switch to the same shape

        // reset default values
        movement.ResetValues();

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

        // Activate shapeshift particles
        spriteRenderer = newShape.GetComponent<SpriteRenderer>();
        shapeModule = shapeParticleSystem.shape;
        shapeModule.spriteRenderer = spriteRenderer;
        shapeModule.texture = spriteRenderer.sprite.texture;

        // Change the force field according to shape size
        forceField.endRange = newAirCollider.size.y;

        // Emit Particles and flash player
        StartCoroutine(StartAndStopEmission());
    }
    private IEnumerator StartAndStopEmission()
    {

        emissionModule = shapeParticleSystem.emission;

        // Start emission
        emissionModule.enabled = true;
        shapeParticleSystem.Play();

        Color originalColor = spriteRenderer.color;
        float timer = 1f;
        Material originalMaterial = spriteRenderer.material;

        Material newMaterial = new Material(originalMaterial);
        newMaterial.shader = Shader.Find("Shader Graphs/FlashShader");
        spriteRenderer.material = newMaterial;

        // Flash
        do
        {
            float t = 1.0f - timer;
            Color newColor = color.Evaluate(t);

            newMaterial.SetColor("_FlashColor", newColor);

            timer -= Time.deltaTime;
            yield return null;

        } while (timer > 0);

        // Set back to the original color
        spriteRenderer.material = originalMaterial;


        // Stop emission
        emissionModule.enabled = false;
        shapeParticleSystem.Stop();
    }
    public void DecreasePointUse(float shapePoints)
    {
        shapePoints -= 1;
        Debug.Log($"DECREASE VARIABLE TO {shapePoints}");
    }
    public float DecreasePointTime(float shapePoints)
    {
        return shapePoints - Time.deltaTime;
    }
    public void UpdateBars(float shapePoints, Image shapeBar)
    {
        float barValue = Mathf.InverseLerp(0f, maxPoints, shapePoints);
        if (shapeBar != null) shapeBar.fillAmount = barValue;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("AAAAAAAAAAA");
        if (other.CompareTag("Point"))
        {
            Points pts = other.GetComponent<Points>();

            if (pts.ShareType == PointsShape.Rhino)
            {
                RhinoPoints += 1;
            }
            else if (pts.ShareType == PointsShape.Gorilla)
            {
                GorillaPoints += 1;
            }
            else if (pts.ShareType == PointsShape.Dragon)
            {
                DragonPoints += 1;
            }
            Destroy(other.gameObject);
        }
    }

}
