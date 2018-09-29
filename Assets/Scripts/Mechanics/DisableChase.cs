using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableChase : MonoBehaviour
{
    public GameObject ia;
     void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.CompareTag("Enemy"))
        {
            
            ia.GetComponent<IA>().agent.speed = 0;
        }
    }
}
