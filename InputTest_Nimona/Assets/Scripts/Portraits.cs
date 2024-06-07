using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portraits : MonoBehaviour
{
    [SerializeField] private Image shapePortrait;
    [SerializeField] private Image shapePadTriangle;

    public void ChangeShapeView(int angle, Sprite portrait)
    {
        shapePadTriangle.transform.eulerAngles = new Vector3(0f,0f,angle);
        shapePortrait.sprite = portrait;
    }
}
