using System.Collections;
using System.Collections.Generic;
using UnityEngine;

https://www.youtube.com/watch?v=9tePzyL6dgc&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=3

public class CharacterCheckItem : MonoBehaviour
{

    RaycastHit hit;

    CharacterController characterController;

    Interactable focus;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        Ray ray;
        float rayDistance = 4;

        ray = new Ray(this.transform.position + new Vector3(0f, characterController.center.y, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Input.GetKey(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
                    SetFocus(interactable);
                }

            }
        }

    }

    void SetFocus(Interactable newFocus)
    {
        focus = newFocus
    }

    void RemoveFocus()
    {
        focus = null;
    }
}
