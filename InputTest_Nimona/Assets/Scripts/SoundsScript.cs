using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsScript : MonoBehaviour
{
    [SerializeField] private AudioClip soundToPlay;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayAudio()
    {
        audioSource.clip = soundToPlay;
        audioSource.Play();
    }
}
