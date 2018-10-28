using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MenuSounds : MonoBehaviour {

    public AudioSource click;
    public AudioSource thunderClick;
    
    public Animation fadesoundRain;
    public Animation fadesoundThunder;

    public void clickPlay()
    {
        click.Play();
    }
    public void thunderPlay()
    {
        thunderClick.Play();
       
        fadesoundRain.Play();
        fadesoundThunder.Play();
        
        
    }
}
