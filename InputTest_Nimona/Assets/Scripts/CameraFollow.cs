using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float rateToFollow = 0.5f;
    [SerializeField] private float yOffSet;
    void FixedUpdate()
    {
        Vector3 targetPosition = objectToFollow.position;
        targetPosition.z = transform.position.z;

        Vector3 delta = targetPosition - transform.position;

        transform.position += delta * rateToFollow;

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffSet, transform.position.z);

    }
}
