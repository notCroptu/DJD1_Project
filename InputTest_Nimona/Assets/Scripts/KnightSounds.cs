using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSounds : MonoBehaviour
{
    [field:SerializeField] public AudioClip Parry { get; private set; }
    [field:SerializeField] public AudioClip Swipe { get; private set; }
    [field:SerializeField] public AudioClip Death { get; private set; }
}
