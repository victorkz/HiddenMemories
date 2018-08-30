using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    
    public CharacterControlRB player;
    public bool onTrigger = false;
    public void Start()
    {
        
        onTrigger = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Inv"))
        {
            onTrigger = true;
            player.Hide();
            player.UnHide();
            
        }
        
    }
   
}
