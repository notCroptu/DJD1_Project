using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float rateToFollow = 0.5f;
    void Update()
    {
        Vector3 targetPosition = objectToFollow.position;
        targetPosition.z = transform.position.z;

        Vector3 delta = targetPosition - transform.position;

        transform.position += delta * rateToFollow * Time.deltaTime*10;
    }
}
