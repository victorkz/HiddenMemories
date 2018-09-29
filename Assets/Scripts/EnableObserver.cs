using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObserver : MonoBehaviour
{
    public GameObject ObserverEnemy;
    

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            ObserverEnemy.SetActive(true);
        }
        else
        {
            ObserverEnemy.SetActive(false);
        }
    }



}
