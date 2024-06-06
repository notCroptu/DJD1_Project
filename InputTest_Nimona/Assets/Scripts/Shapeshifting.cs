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
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Portraits portraitScript;
    [SerializeField] private List<Sprite> portraitList;

    private bool canShapeshift;
    private bool isThrowing = false;
    void Start()
    {
        originalColor = spriteRenderer.color;
        originalMaterial = spriteRenderer.material;

        // Initialize with human shape at the start
        ChangeShape<Human>(human,0,portraitList[0]);
        RhinoPoints = maxPoints;
        GorillaPoints = maxPoints;
        DragonPoints = maxPoints;

        playerActions = GetComponent<PlayerActions>();

        isThrowing = false;
    }
    void OnEnable()
    {
        isThrowing = true;
    }
    void Update()
    {
        float rJoystickX = playerActions.ShapeshiftX.Value;
        float rJoystickY = playerActions.ShapeshiftY.Value;
        Vector2 joystickInput = new Vector2(rJoystickX, rJoystickY);

        canShapeshift = false;
        
        if ( isThrowing )
        {
            // not Aiming
            if ( joystickInput.magnitude < 0.2f )
            {
                isThrowing = false;
                // if ( !isThrowing ) Debug.Log("RELEASED.");
            }
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
                    ChangeShape<Rhino>(rhino,0,portraitList[1]);
                }
                else if (rJoystickY < -0.71f || Input.GetKeyDown(KeyCode.Alpha4))
                {
                    ChangeShape<Human>(human,0,portraitList[0]);
                }
                else if ((rJoystickY > 0.71f || Input.GetKeyDown(KeyCode.Alpha2)) && DragonPoints > 0)
                {
                    ChangeShape<DragonWings>(dragon,0,portraitList[2]);
                }
                else if ((rJoystickX < -0.71f || Input.GetKeyDown(KeyCode.Alpha3)) && GorillaPoints > 0)
                {
                    ChangeShape<Gorilla>(gorilla,0,portraitList[3]);
                }
            }
            else
            {
                if (rJoystickX > 0.71f)
                {
                    ChangeShape<Rhino>(rhino,90,portraitList[1]);
                }
                else if (rJoystickY < -0.71f)
                {
                    ChangeShape<Human>(human,0,portraitList[0]);
                }
                else if (rJoystickY > 0.71f)
                {
                    ChangeShape<DragonWings>(dragon,180,portraitList[2]);
                }
                else if (rJoystickX < -0.71f)
                {
                    ChangeShape<Gorilla>(gorilla,270,portraitList[3]);
                }
            }
        }
        
        UpdateBars(GorillaPoints,gorillaBar);
        UpdateBars(DragonPoints,dragonBar);
        UpdateBars(RhinoPoints,rhinoBar);
    }
    public void ChangeShape<T>(GameObject newShape, int angle, Sprite portrait) where T : MonoBehaviour, IShapeColliders 
    {
        if (newShape == currentShape) return; // Don't switch to the same shape

        portraitScript.ChangeShapeView(angle,portrait);

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

        Material newMaterial = new Material(flashMaterial);

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
