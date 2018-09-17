using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;//Jogador;

    public float smoothSpeed = 2f;//Suavização da camera
    Vector3 smoothedPosition;

    public static float alterarCamera;
    public static bool EntrouScrip;
    public Vector3 offset;
    public float z;


    private void Update()
    {
        if(EntrouScrip)
        {
            z = alterarCamera;

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 desiredPosition = target.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, z);
    }
}
