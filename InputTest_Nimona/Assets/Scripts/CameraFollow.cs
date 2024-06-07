using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float rateToFollow = 0.5f;
    [SerializeField] private float yOffSetAmount;
    private float yOffSet;
    private Movement movement;
    private bool isGrounded;
    void FixedUpdate()
    {
        if ( movement == null )
        {
            movement = objectToFollow.gameObject.GetComponent<Movement>();
        }
        else
        {
            isGrounded = movement.IsGrounded;
        }
        Debug.Log(isGrounded);

        Vector3 targetPosition = objectToFollow.position;
        targetPosition.z = transform.position.z;

        Vector3 delta = targetPosition - transform.position;

        transform.position += delta * rateToFollow;

        if ( isGrounded )
        {
            yOffSet = Mathf.Lerp(yOffSet, yOffSetAmount, 0.01f);
        }
        else
        {
            yOffSet = Mathf.Lerp(yOffSet, 0, 0.01f);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffSet, transform.position.z);
    }
}
