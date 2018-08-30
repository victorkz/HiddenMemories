using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMovementRB : MonoBehaviour
{
    //Rigibody
    private Rigidbody rb;

    public float moveSpeed;//Velocidade do personagem

    //Recebera os eixos de movimentação do jogador
    private Vector3 moveInput;
    private Vector3 moveVelocity;//Recebe Velocidade de movimento

    //Pegamos camera principal para temos o movimento do personagem relativo a ela
    private Camera mainCamera;

    //Animação
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;//Atribuimos a camera principal
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Atribuimos os eixos para dois floats
        float lh = Input.GetAxis("Horizontal");
        float lv = Input.GetAxis("Vertical");

        moveInput = new Vector3(lh, 0, lv).normalized;//Recebe eixos

        Vector3 cameraFoward = mainCamera.transform.forward;//Pegamos a direção da frente da camera(Seu eixo Z, o que a camera está olhando)
        cameraFoward.y = 0;//Para não mover a camera para cima

        //Rotação relativa da camera
        Quaternion cameraRelativeRotation = Quaternion.FromToRotation(Vector3.forward, cameraFoward);
        Vector3 lookToward = cameraRelativeRotation * moveInput;

        if (moveInput.sqrMagnitude > 0)
        {
            Ray lookRay = new Ray(transform.position, lookToward);//posição do raio
            transform.LookAt(lookRay.GetPoint(1));//Fazemos a rotação do personagem baseado na posição da camera(entre parentes temos a distância do raio do raycast)
        }

        moveVelocity = transform.forward * moveSpeed * moveInput.magnitude;//Damos a direção de movimento(para frente) e multiplicamos pela velocidade de movimento

        Animating();

    }

    //Este método é usado para fazer o update de tudo relacionado a fisica(Rigibody)
    private void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }

    void Animating()
    {
        animator.SetFloat("speedPercent", rb.velocity.magnitude);
    }
}
