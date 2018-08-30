using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundSphere : MonoBehaviour
{
    AI ai;
    SphereCollider SP;
    bool perseguir = false;
    // Use this for initialization
    void Start()
    {
        SP = GetComponent<SphereCollider>();
        ai = GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {

        // SP.radius+=0.01f;
        if (Input.GetButton("Fire1"))
        {
            SP.radius -= 0.01f;
        }
        if (Input.GetButton("Fire2"))
        {
            SP.radius += 0.01f;
        }

       
    }
    


    }

