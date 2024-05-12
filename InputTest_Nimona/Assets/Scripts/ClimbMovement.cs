using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbMovement : MonoBehaviour
{
    [SerializeField] private GorillaClimb climbScript;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private Rigidbody2D rb;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        rb.gravityScale = 0;

        Vector3 currentVel = rb.velocity;

        currentVel.y = verticalMovement * climbSpeed;

        rb.velocity = new Vector3 (currentVel.x, currentVel.y, 0f);
        WallJump();

        if (!isWallJumping)
        {
            if ((horizontalMovement < 0) && (transform.right.x > 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if ((horizontalMovement > 0) && (transform.right.x < 0))
            {
                transform.rotation = Quaternion.identity;
            }
        }
        
    }
    private void WallJump()
    {
        if (climbScript.WallCheck)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.right.x;
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButton("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector3(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y, 0f);
            wallJumpingCounter = 0f;

            //if (transform.right.x != wallJumpingDirection)
            //{
            //    if ((transform.right.x > 0))
            //    {
            //        transform.rotation = Quaternion.Euler(0, 180, 0);
            //    }
            //    else if ((transform.right.x < 0))
            //    {
            //        transform.rotation = Quaternion.identity;
            //    }
            //}

            Invoke("StopWallJumping", wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
