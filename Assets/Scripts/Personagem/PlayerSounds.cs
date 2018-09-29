using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    public ControlePersonagem player;
    public GameObject Walk;

    void Update()
    {
        if(player.characterState==CharacterState.walking)
        {
            
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }



}
