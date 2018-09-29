using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public Lever lever;
    public GameObject startPos;
    public GameObject endPos;
    public Renderer renderer;
    public bool E = false;

    void Start ()
    {
        transform.position = startPos.transform.position;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (lever.isActive == true)
        {
            renderer.material.color = Color.yellow;

            if (E == true)
            {
                renderer.material.color = Color.green;
            }
        }
        else
        {
            renderer.material.color = Color.red;
        }
	}

    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player") && lever.isActive==true && E==true)
        {
            
            Invoke("Subindo", 2);
        }
        if(Input.GetKey(KeyCode.E))
        {
            
            E = true;
        }
    }

    void Subindo()
    {
        transform.position = Vector3.Lerp(transform.position, endPos.transform.position, Time.deltaTime);
    }

    
}
