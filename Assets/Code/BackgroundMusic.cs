using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip backgroundMusic;

    // Use this for initialization
    void Start()
    {
        // Audio Source responsavel por emitir os sons
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

}
