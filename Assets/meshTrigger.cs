using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshTrigger : MonoBehaviour
{
    public bool playerTriggered = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTriggered = true;
        }
        else
        {
            playerTriggered = false;
        }
    }
}
