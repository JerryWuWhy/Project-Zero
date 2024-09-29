using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Start()
    {
        // 开始播放背景音乐
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

}
