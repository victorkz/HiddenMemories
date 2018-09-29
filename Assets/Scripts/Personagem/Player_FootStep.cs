using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FootStep : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string InputFootstep;
    FMOD.Studio.EventInstance FootstepsEvent;
    FMOD.Studio.ParameterInstance Walk_MaterialParameter;


    public ControlePersonagem player;
    public float walkingSpeed = 0.01f;
    private float Walk_Material_Value;


    

    void Start()
    {
        FootstepsEvent = FMODUnity.RuntimeManager.CreateInstance(InputFootstep);
        FootstepsEvent.getParameter("Walk_Material", out Walk_MaterialParameter);


        InvokeRepeating("CallFootsteps", 0, 0.5f);
        InvokeRepeating("CallFootstepsRunning", 0, 0.4f);
    }


    void Update()
    {
        
        Walk_MaterialParameter.setValue(Walk_Material_Value);

    }

    void CallFootsteps()
    {
       
            if (player.characterState == CharacterState.walking || player.characterState == CharacterState.crounching)
            {
                FootstepsEvent.start();

            }
        
            else if (player.characterState == CharacterState.idle)
            {

            }
        
    }
    void CallFootstepsRunning()
    {
        if (player.characterState == CharacterState.running)
        {
            FootstepsEvent.start();

        }
    }
    void OnDisable()
    {
        player.characterState = CharacterState.idle;
    }

     void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Concrete_Terrain"))
        {
            Walk_Material_Value = 2f;

        }
        else if (other.CompareTag("Dirty_Terrain"))
        {
            Walk_Material_Value = 3f;

        }
        else if (other.CompareTag("Grass_Terrain"))
        {
            Walk_Material_Value = 4f;
        }
        else if (other.CompareTag("Wood_Terrain"))
        {
            Walk_Material_Value = 5f;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        Walk_Material_Value = 1f;
    }

}
/*
 1- Silence;
 2- Concrete;
 3- Dirty_Road;
 4- Grass;
 5- Wood;
  
  
  
 */
