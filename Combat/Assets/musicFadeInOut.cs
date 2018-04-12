using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicFadeInOut : MonoBehaviour
{

    AudioSource sound;
    bool stopFade = false;
    bool start = false;

    public float fadeInTime;

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopFade == false)
        {
            if (sound.time < fadeInTime)
            {
                sound.volume = ((sound.time) / (fadeInTime));
            }
            else if (sound.time >= fadeInTime)
            {
                sound.volume = 1f;
                stopFade = true;
            }
        }
    }
}
