using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    PauseScript ps;
    FMOD.Studio.Bus SFX;
    float SFXVolume = 0.5f;
    void Awake()
    {
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Environment");
        

    }

    // Update is called once per frame
    void Update()
    {
        SFX.setVolume(SFXVolume);
       
       
    }
    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
       
    }
   
}
