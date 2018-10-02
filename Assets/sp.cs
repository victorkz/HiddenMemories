using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sp : MonoBehaviour
{


    Rigidbody rigidBody;
    Vector3 move;
    public float velocity;


    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        move = new Vector3(moveH, 0, moveV);


    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(this.transform.position + move * velocity);
    }
}
