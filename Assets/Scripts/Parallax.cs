using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform cam;
    private Vector3 camStartPos;
    private Material material;
    private float distance;
    [SerializeField] private bool fixateHorizontally = true;
    [SerializeField] [Range(0f,0.5f)] private float speed;
    [SerializeField] private bool moveIdle;
    [SerializeField] [Range(-0.5f, 0.5f)] private float moveIdleSpeed;
    private float moveIdleDistance = 0;
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if ( moveIdle )
        {
            moveIdleDistance += Time.deltaTime * moveIdleSpeed;
        }

        if ( fixateHorizontally )
        {
            distance = (cam.position.x - camStartPos.x) * speed;
            distance += moveIdleDistance;
            transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
            material.SetTextureOffset("_MainTex", new Vector2(distance, 0));
        }
        else
        {
            distance = (cam.position.y - camStartPos.y) * speed;
            transform.position = new Vector3(transform.position.x, cam.position.y, transform.position.z);
            material.SetTextureOffset("_MainTex", new Vector2(moveIdleDistance, distance));
        }
    }
}
