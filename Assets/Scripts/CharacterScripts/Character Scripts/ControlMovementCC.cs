using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMovementCC : MonoBehaviour
{
    public float walkSpeed = 2;//Velocidade de caminhada
    public float runSpeed = 6;//Velocidade de corrida


    Vector2 playerInput;
    bool running = false;
    Animator animator;
    public SphereCollider sphereCollider;



    //Smooth de rotação do personagem
    [Range(0, 3)]//Slider da variavel de baixo
    public float TurnSmoothTime = 0.2f;//Suavização do tempo de rotação do personagem
    float turnSmoothVelocity;

    //Smooth da velocidade do personagem
    [Range(0.0f, 0.5f)]
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    //Character Controller
    CharacterController characterController;

    //Gravidade
    public float gravity = -12;
    float VelocityY;

    //Pulo
    [Range(0, 1)]
    public float jumpHeight = 1f;

    //Controle no ar
    [Range(0, 1)]
    public float airControlPercent;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
       

        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//Pegamos input do jogador//Se eu usar um vetor tridimensional o personagem não rotaciona
        Vector3 inputDir = playerInput.normalized;//Passamos o input do jogador para outro vetor, onde normalizamos ele

        //Personagem pula
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //O personagem sempre retorna para a posição original, então se o vetor não for igual a zero, ele calcula e mantem a posição
        if (inputDir != Vector3.zero)
        {
            //Deixa a rotação do persoangem mais suave
            float playerRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;//Rotação do personagem
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, playerRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(TurnSmoothTime));
            // transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        {
            //running = true;
        }

        //Se o personagem esta correndo igual a runSpeed se não é walkSpeed e se não tiver nada pressionado multiplicado por magnitude será igual a zero
        float PlayerSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        if (PlayerSpeed >= walkSpeed * inputDir.magnitude)


            //print(inputDir.magnitude);
            currentSpeed = Mathf.SmoothDampAngle(currentSpeed, PlayerSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));//Deixamos a transição de velocidade do personagem mais suave;
        //print("PlayerVelocidade: " + PlayerSpeed + " PlayerCurrentSpeed: " + currentSpeed);

        //Velocidade de queda
        VelocityY += Time.deltaTime * gravity;

        //Mover o personagem na direção em que ele está olhando
        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * VelocityY;//Movimento do personagem com o CharacterController, adicionando gravidade junto
        characterController.Move(velocity * Time.deltaTime);//Atribuimos movimento ao CharacterController

        //Para fazer o personagem para de correr quando encostar em algo
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        //Se o personagem estiver no chão sua velocidade Y é igual a zero
        if (characterController.isGrounded)
        {
            VelocityY = 0;
        }

        //Tempo da porcentagem da animação do blendTree
        //float animationSpeedPorcent = ((running) ? 1 : .5f) * inputDir.magnitude;
        float animationSpeedPorcent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPorcent, speedSmoothTime, Time.deltaTime);//Deixamos a blendTree mais suave

    }

    void Jump()
    {

        if (characterController.isGrounded)
        {

            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            VelocityY = jumpVelocity;
        }
    }

    //Criamos um método que retorna um float que modificamos o tempo de suavização
    float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
        {
            return smoothTime;//Retornamos sem nenhuma modificação
        }

        return smoothTime / airControlPercent;//Demoramos o tempo de reação do personagem no ar(se air control for menor que 1)
    }


    public void SomParado()
    {
        if (playerInput == Vector2.zero)
        {
            sphereCollider.radius = 0f;

        }
    }
    public void SomAndando()
    {
        /* if (currentSpeed < 2.1f)
         {
             sphereCollider.radius += 0.05f;
             if (sphereCollider.radius > 2)
             {
                 sphereCollider.radius = 2f;
             }

         }*/


    }


        public void SomCorrendo()
        {
            if (currentSpeed > 2.1f && currentSpeed < 4.1f)
            {
                sphereCollider.radius += 0.05f;
                if (sphereCollider.radius > 4)
                {
                    sphereCollider.radius = 4f;
                }
            }
            if (currentSpeed < 2.1f && sphereCollider.radius != 1)
            {
                sphereCollider.radius -= 0.05f;
                if (sphereCollider.radius < 1)
                {
                    sphereCollider.radius = 1f;
                }
            }
        }
    }

