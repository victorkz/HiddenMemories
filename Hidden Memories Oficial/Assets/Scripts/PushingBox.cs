using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingBox : MonoBehaviour
{
    public GameObject characterPlayer;// Objeto do Jogador;
    //public PushingBox pushingBoxScript;// Script do jogador

    Rigidbody rb;

    public bool isPushing;
    public bool isColliding;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // pushingBoxScript = characterPlayer.GetComponent<PushingBox>();
        //boxPosition = this.transform.parent;

    }

    void Update()
    {

        if (isColliding && Input.GetKeyUp(KeyCode.E))
        {
            isPushing = false;
            rb.isKinematic = true;
            this.transform.parent = null;
        }

        if (isColliding && Input.GetKey(KeyCode.E))
        {
            transform.forward = characterPlayer.transform.forward + transform.forward * 4000;
            isPushing = true;
            rb.isKinematic = false;
            this.transform.parent = characterPlayer.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isColliding = true;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb.isKinematic = false;

            isColliding = false;

            isPushing = false;
        }
    }

}

