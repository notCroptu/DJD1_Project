using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [field:SerializeField] public PointsShape ShareType { get; private set; }
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        if (ShareType == PointsShape.Rhino)
        {
            spr.color = new Color(0f,0f,100f);
        }
        else if (ShareType == PointsShape.Gorilla)
        {
            spr.color = new Color(0f,100f,0f);
        }
        else if (ShareType == PointsShape.Dragon)
        {
            spr.color = new Color(100f,0f,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum PointsShape
{
    Rhino,
    Gorilla,
    Dragon
}
