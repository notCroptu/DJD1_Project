using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlace : MonoBehaviour
{
    [SerializeField] private GameObject whereToGo;
    [SerializeField] private GameObject goObject;

    void OnCollisionEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            goObject.transform.position = whereToGo.transform.position;
        }
    }
}
