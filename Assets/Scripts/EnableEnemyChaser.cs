using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemyChaser : MonoBehaviour {

    public GameObject ChaserEnemy;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChaserEnemy.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChaserEnemy.SetActive(false);
        }
    }
    
}
