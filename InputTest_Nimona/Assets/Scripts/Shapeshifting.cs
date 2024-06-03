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
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float ParticleEffectArea;
    [SerializeField] private Gradient color;
    private Color originalColor;
    private Material originalMaterial;

    private bool canShapeshift;
    private bool isThrowing = false;
    private bool ThrowingActivated = false;
    void Start()
    {
        originalColor = spriteRenderer.color;
        originalMaterial = spriteRenderer.material;

        // Initialize with human shape at the start
        ChangeShape<Human>(human);
        RhinoPoints = maxPoints;
        GorillaPoints = maxPoints;
        DragonPoints = maxPoints;

        playerActions = GetComponent<PlayerActions>();
    }
    void Update()
    {
        float rJoystickX = playerActions.ShapeshiftX.Value;
        float rJoystickY = playerActions.ShapeshiftY.Value;

        // Check if gorilla is throwing
        if ( playerActions.Throw.WasPressed )
        {
            isThrowing = true;
            Debug.Log("Started throwing.");
        }

        ThrowingActivated = Mathf.Abs(rJoystickX) >= 0.2f && Mathf.Abs(rJoystickY) >= 0.2f;

        if ( (currentShape == gorilla) && isThrowing && ThrowingActivated )
        {
            // Allow shapeshifting if joystick is neutral
            canShapeshift = Mathf.Abs(rJoystickX) < 0.2f && Mathf.Abs(rJoystickY) < 0.2f;
            isThrowing = !canShapeshift;
            if ( canShapeshift ) ThrowingActivated = false;
            if ( !isThrowing ) Debug.Log("RELEASED.");
        }
        else
        {
            canShapeshift = true;
        }

        if ( canShapeshift )
        {
            if (testShapeshiftPoints)
            {
                if ((rJoystickX > 0.71f || Input.GetKeyDown(KeyCode.Alpha1)) && RhinoPoints > 0)
                {
                    ChangeShape<Rhino>(rhino);
                }
                else if (rJoystickY < -0.71f || Input.GetKeyDown(KeyCode.Alpha4))
                {
                    ChangeShape<Human>(human);
                }
                else if ((rJoystickY > 0.71f || Input.GetKeyDown(KeyCode.Alpha2)) && DragonPoints > 0)
                {
                    ChangeShape<DragonWings>(dragon);
                }
                else if ((rJoystickX < -0.71f || Input.GetKeyDown(KeyCode.Alpha3)) && GorillaPoints > 0)
                {
                    ChangeShape<Gorilla>(gorilla);
                }
            }
            else
            {
                if (rJoystickX > 0.71f)
                {
                    ChangeShape<Rhino>(rhino);
                }
                else if (rJoystickY < -0.71f)
                {
                    ChangeShape<Human>(human);
                }
                else if (rJoystickY > 0.71f)
                {
                    ChangeShape<DragonWings>(dragon);
                }
                else if (rJoystickX < -0.71f)
                {
                    ChangeShape<Gorilla>(gorilla);
                }
            }
        }
        
        UpdateBars(GorillaPoints,gorillaBar);
        UpdateBars(DragonPoints,dragonBar);
        UpdateBars(RhinoPoints,rhinoBar);
    }
    public void ChangeShape<T>(GameObject newShape) where T : MonoBehaviour, IShapeColliders 
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

        T ShapeColliders = newShape.GetComponent<T>();

        // Update movement colliders
        movement.GroundCheckCollider = ShapeColliders.GroundCheckCollider;
        movement.GroundCollider = ShapeColliders.GroundCollider;
        movement.AirCollider = ShapeColliders.AirCollider;

        // Activate shapeshift particles
        spriteRenderer = newShape.GetComponent<SpriteRenderer>();
        shapeModule = shapeParticleSystem.shape;
        shapeModule.spriteRenderer = spriteRenderer;
        shapeModule.texture = spriteRenderer.sprite.texture;

        // Change the force field according to shape size
        forceField.endRange = ShapeColliders.AirCollider.size.y;

        // Emit Particles and flash player
        StartCoroutine(StartAndStopEmission());

        canShapeshift = false;
    }
    private IEnumerator StartAndStopEmission()
    {

        emissionModule = shapeParticleSystem.emission;

        // Start emission
        emissionModule.enabled = true;
        shapeParticleSystem.Play();

        float timer = 1f;

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
        spriteRenderer.color = originalColor;


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

    // For GDII
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
