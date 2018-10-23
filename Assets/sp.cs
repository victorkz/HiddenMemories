using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sp : MonoBehaviour
{


    Rigidbody rigidBody;
    Vector3 move;
    public float velocity;

    public Interactable focus;

    CapsuleCollider capsuleCollider;

    public RaycastHit hit;


    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        move = new Vector3(moveH, 0, moveV);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //Raycast

        Ray ray;
        float rayDistance = 1;

        ray = new Ray(this.transform.position + new Vector3(0f, capsuleCollider.center.y, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < rayDistance)
            {
                // Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
                //Debug.Log("hit something");

                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable.tag == "Box")
                {
                    Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SetFocus(interactable);
                    }

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        RemoveFocus();
                    }
                }
            }

        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(this.transform.position - move * velocity);
    }

    void SetFocus(Interactable newFocus)
    {

        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.onDefocused();
            }
            focus = newFocus;
        }

        newFocus.onFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.onDefocused();

        focus = null;
    }
}
