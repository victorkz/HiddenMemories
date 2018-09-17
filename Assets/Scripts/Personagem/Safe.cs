﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    
    public ControlePersonagem player;
    public bool onTrigger = false;
    public void Start()
    {
        
        onTrigger = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Inv"))
        {
            onTrigger = true;
            player.Hide();
            player.UnHide();
            
        }
        
    }
   
}