using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [field:SerializeField] public AudioClip Shapeshift { get; private set; }
    [field:SerializeField] public AudioClip Ram { get; private set; }
    [field:SerializeField] public AudioClip Charge { get; private set; }
    [field:SerializeField] public AudioClip Flap { get; private set; }
    [field:SerializeField] public AudioClip Glide { get; private set; }
    [field:SerializeField] public AudioClip Jump { get; private set; }
    [field:SerializeField] public AudioClip Walk { get; private set; }
    [field:SerializeField] public AudioClip Climb { get; private set; }
    [field:SerializeField] public AudioClip Pickup { get; private set; }
    [field:SerializeField] public AudioClip Throw { get; private set; }
    [field:SerializeField] public AudioClip Death { get; private set; }
}
