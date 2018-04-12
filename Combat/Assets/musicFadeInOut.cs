using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicFadeInOut : MonoBehaviour
{

    AudioSource sound;
    float duration;
    bool stopFade = false;
    bool start = false;

    public float fadeInTime;
    private float maxVolume;


    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
        duration = sound.clip.length;
        //Start volume = max possible volume
        maxVolume = sound.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopFade == false)
        {
            if (sound.time < 1)
            {
                StartCoroutine("FadeIn");
            }
            else if (sound.time >= fadeInTime)
            {
                StopCoroutine("FadeIn");
                Debug.Log("stop fade");
                sound.volume = maxVolume;
                stopFade = true;
            }
        }
    }

    IEnumerator FadeIn()
    {
        float currentTime = sound.time;
        Debug.Log(currentTime);
        if (currentTime < fadeInTime)
        {
            if (currentTime == 0)
            {
                sound.volume = 0;
            } else { 
                sound.volume = maxVolume * (currentTime / fadeInTime);
            }
            Debug.Log(sound.volume);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
