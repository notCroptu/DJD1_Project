using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShapeColliders {
    BoxCollider2D GroundCheckCollider { get; set; }
    CapsuleCollider2D GroundCollider { get; set; }
    BoxCollider2D AirCollider { get; set; }
}
