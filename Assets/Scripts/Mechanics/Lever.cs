using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{


    public Renderer renderer;
    public bool isActive = false;

    


    void Start()
    {

    }


    void Update()
    {
        if (isActive == true)
        {
            renderer.material.color = Color.yellow;
        }
        else
        {
            renderer.material.color = Color.blue;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.tag);
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Player Pressionou o botão");
                
                isActive = true;
            }

        }
     
    }
}
