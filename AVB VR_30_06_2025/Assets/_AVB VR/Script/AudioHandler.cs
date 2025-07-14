using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;

    public AudioSource BGAudioSource;
    public AudioSource ButtonClickAS;
    public AudioSource ButtonHoverAS;
    public AudioSource CrowdAS;
    public AudioSource BallwooshAS;
    public AudioSource CountDownTickAS;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    public void ButtonHover_AudioPlay()
    {
        if (ButtonHoverAS.isPlaying)
            ButtonHoverAS.Stop();

        ButtonHoverAS.Play();
    }

    public void ButtonClick_AudioPlay()
    {
        if (ButtonClickAS.isPlaying)
            ButtonClickAS.Stop();

        ButtonClickAS.Play();
    }

    public void BallWoosh_AudioPlay()
    {
        if(BallwooshAS.isPlaying)
            BallwooshAS.Stop();

        BallwooshAS.Play();
    }

    public void CountDownTick_AudioPlay()
    {
        if (CountDownTickAS.isPlaying)
            CountDownTickAS.Stop();

        CountDownTickAS.Play();
    }

    public void Crowd_AudioPlay()
    {
        //CrowdAS.Play();
    }

    public void Crowd_AudioStop()
    {
        //CrowdAS.Stop();
    }
}
