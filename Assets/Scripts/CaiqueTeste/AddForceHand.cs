using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceHand : MonoBehaviour
{
    public GameObject objectInteract;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool isPushing = false;

        if (hit.gameObject.tag == "Box")
        {
            isPushing = true;
            objectInteract = hit.gameObject;
            objectInteract.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 15);
        }

        else if (!isPushing)
        {
            objectInteract = null;
            isPushing = false;
        }
    }

}
