using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Start()
    {
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }
}
