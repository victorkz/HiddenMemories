using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IALook : MonoBehaviour
{
    public ControlePersonagem player;

    public IALook iaL;
    public GameObject cabeca;
    public GameObject cabecaAbaixada;
    public Transform target;
    public Transform targetPlayer;
    public bool isSpoted = false;
    public Renderer renderer;
    public float viewDistance = 10f;
    public float fov;
    public Light spot;
    public LayerMask layer;

    void Update()
    {
        Vector3.Distance(player.transform.position, this.transform.position);
        fov = spot.spotAngle;
        viewDistance = spot.range;
        if (isSpoted && Vector3.Distance(player.transform.position, this.transform.position) < viewDistance)
        {
            renderer.material.color = Color.red;
            transform.LookAt(targetPlayer);
            player.runSpeed = 1;
            player.walkSpeed = 0.5f;

        }
        else
        {
            isSpoted = false;
            player.runSpeed = 4;
            player.walkSpeed = 2f;
            SearchForPlayerWithoutChase();
            renderer.material.color = Color.blue;
            transform.LookAt(target);

        }


    }

    public void SearchForPlayerWithoutChase()
    {
        if (player.characterState != CharacterState.crounching)
        {
            Debug.DrawLine(transform.position, cabeca.transform.position);
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(cabeca.transform.position)) < fov / 2f)
            {
                if (Vector3.Distance(cabeca.transform.position, transform.position) < viewDistance)
                {
                    Debug.DrawLine(transform.position, cabeca.transform.position, Color.red);
                    RaycastHit hit;
                    if (Physics.Linecast(transform.position, cabeca.transform.position, out hit, layer.value))
                    {

                        Debug.Log(hit.transform.tag);

                        if (hit.transform.tag == "Player")
                        {
                            OnSpoted();

                        }
                    }
                }
            }
            else
            {
                isSpoted = false;
            }
        }
        if (player.characterState == CharacterState.crounching)
        {
            Debug.DrawLine(transform.position, cabecaAbaixada.transform.position);
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(cabecaAbaixada.transform.position)) < fov / 2f)
            {
                if (Vector3.Distance(cabecaAbaixada.transform.position, transform.position) < viewDistance)
                {
                    Debug.DrawLine(transform.position, cabecaAbaixada.transform.position, Color.red);
                    RaycastHit hit;
                    if (Physics.Linecast(transform.position, cabecaAbaixada.transform.position, out hit, layer.value))
                    {
                        Debug.Log(hit.transform.tag);

                        if (hit.transform.tag == "Player")
                        {
                            OnSpoted();
                        }
                    }
                }
                else
                {
                    isSpoted = false;
                }
            }
        }


    }
    public void OnSpoted()
    {
        isSpoted = true;
    }


}
