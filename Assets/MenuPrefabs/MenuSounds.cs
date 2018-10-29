using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MenuSounds : MonoBehaviour
{

    public AudioSource click;
    public AudioSource thunderClick;
    //public AudioSource rainIntro;
   // public AudioSource rainEnd;

    public Animation fadeSoundRain;
   // public Animation fadeOutSoundRain;
    public Animation fadesoundThunder;

    public void Awake()
    {
        fadeSoundRain.Play("RainSoundFadeIn");
        
        
    }
    public void clickPlay()
    {
        click.Play();
    }
    public void thunderPlay()
    {
        thunderClick.Play();
        fadesoundThunder.Play("ThunderSoundFadeOut");
        fadeSoundRain.Stop("RainSoundFadeIn");
        fadeSoundRain.Play("RainSoundFadeOut");

    }
}
