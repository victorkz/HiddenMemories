using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    bool isFocus = false;//personagem interagiu com o objeto

    bool hasInteract = false;

    Transform player;

    public virtual void Interact()
    {
        //método vai ser sobreescrito
    }

    private void Update()
    {
        if (isFocus && !hasInteract)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                Debug.Log("Interact");
                Interact();
                hasInteract = true;
            }

        }
    }

    //Foca interação do personagem
    public void onFocused(Transform playerTransform)
    {
        isFocus = true;
        print("focou");
        player = playerTransform;
        hasInteract = false;
    }

    //Desfoca personagem
    public void onDefocused()
    {
        print("desfocou");
        isFocus = false;
        hasInteract = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
