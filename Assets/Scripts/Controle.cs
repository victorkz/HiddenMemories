using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;
    float crouchSpeed = 1;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float CurrentSpeed;
    float velocityY;

    Animator animatorController;
    CharacterController characterController;


    public SphereCollider sp;
    public GameObject objectInteract;

    AI ai;



    public float somDoPersonagemParado = 0.5f;
    public float somDoPersonagemAndandoAbaixado = 1f;
    public float somDoPersonagemAndando = 2f;
    public float somDoPersonagemPulando = 3f;
    public float somDoPersonagemCorrendo = 4f;
    public float somDoPersonagemInteragindoComACaixa = 6f;

    public float somDoPersonagem;

    Vector3 novaPosicao;
    Vector3 novaRotacao;
    Vector2 input;

    float targetSpeed;

    bool entrou = false;
    public bool isPushing = false;
    public bool isCrounching = false;
    public bool isRunning = false;
    public bool isWalking = false;

    bool rightArrow;
    bool leftArrow;
    bool upArrow;
    bool downArow;


    // Use this for initialization
    void Start()
    {
        animatorController = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        sp = GetComponent<SphereCollider>();
        ai = GetComponent<AI>();
        somDoPersonagem = somDoPersonagemParado;
        isWalking = false;
    }

    void WalkController()
    {
        #region Verificar caixa empurrando
        if (!isPushing)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        if (objectInteract != null)
        {
            if (isPushing && objectInteract != null)
            {
                print("foi?");
                //if (input != Vector2.zero)
                //input = PushingController(input);
            }
        }
        #endregion
        //print(input);
        // input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {


            isWalking = true;
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }
        else
        {
            isWalking = false;
        }

        // float targetSpeed = walkSpeed * inputDir.magnitude;
        #region CONDIÇÃO CAMINHADA
        if (isCrounching && !isRunning || isRunning && isCrounching)
        {
            targetSpeed = crouchSpeed * inputDir.magnitude;
            // print("Crounching: " + targetSpeed);
            characterController.height = 1.23f;
            characterController.center = new Vector3(0, 0.60f, 0);

        }



        if (isRunning && !isCrounching)
        {
            targetSpeed = runSpeed * inputDir.magnitude;
            // print("Running: " + targetSpeed);
            characterController.height = 1.72f;
            characterController.center = new Vector3(0, 0.9f, 0);
        }
        if (!isCrounching && !isRunning)
        {

            targetSpeed = walkSpeed * inputDir.magnitude;
            //print("Walking: " + targetSpeed);
            characterController.height = 1.72f;
            characterController.center = new Vector3(0, 0.9f, 0);
        }

        CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        #endregion

        //transform.Translate(transform.forward * CurrentSpeed * Time.deltaTime, Space.World);
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * CurrentSpeed + Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }

        #region SETANDO ANIMAÇÃO
        float animationSpeedPercente = ((isRunning) ? 1 : .5f) * inputDir.magnitude;
        animatorController.SetFloat("SpeedPercent", animationSpeedPercente, speedSmoothTime, Time.deltaTime);

        if (isCrounching)
        {
            animatorController.SetBool("IsCrouch", isCrounching);
        }
        else
            animatorController.SetBool("IsCrouch", isCrounching);
        #endregion
    }
    /*
    Vector2 PushingController(Vector2 input)
    {
        print("puxador" + objectInteract.gameObject.name);

        if (objectInteract.transform.position.x > this.transform.position.x)
        {
            //print(input);

            rightArrow = true;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, objectInteract.transform.position.z);


            //input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            print("X: Caixa é maior que personagem ");
        }

        if (objectInteract.transform.position.x < this.transform.position.x)
        {
            //print(input);

            leftArrow = true;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, objectInteract.transform.position.z);


            //input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            print("X: Caixa é maior que personagem ");
        }

        if (objectInteract.transform.position.z > this.transform.position.z)
        {
            //print(input);

            upArrow = true;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, objectInteract.transform.position.z);


            //input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            print("X: Caixa é maior que personagem ");
        }

        if (objectInteract.transform.position.x < this.transform.position.x)
        {
            //print(input);

            downArow = true;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, objectInteract.transform.position.z);


            //input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            print("X: Caixa é maior que personagem ");
        }



        /*
        if (objectInteract.transform.position.x > this.transform.position.x)
        {
            //print(input);

            rightArrow = true;

    }*/

    void JumpController()
    {
        if (characterController.isGrounded)
        {
            somDoPersonagem = somDoPersonagemPulando;
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region PlayerSoundSphere
        /*
        if (!isWalking && !isRunning && !isPushing && !isCrounching)
        {
            if (sp.radius > 0.5f)
            {
                sp.radius -= 0.01f;
            }
        }
        if (isWalking && characterController.height == 1.23f)
        {
            sp.radius = 1f;

        }
        if (isWalking && characterController.height == 1.72f)
        {

            sp.radius = 2f;
        }


        if (isRunning && characterController.height == 1.72f)
        {

            sp.radius = 4f;
        }
        */
        #endregion


        if (!isWalking && !isRunning && !isPushing && !isCrounching)
        {
            somDoPersonagem = somDoPersonagemParado;
        }
        if (isWalking && characterController.height == 1.23f)
        {
            somDoPersonagem = somDoPersonagemAndandoAbaixado;

        }
        if (isWalking && characterController.height == 1.72f)
        {
            somDoPersonagem = somDoPersonagemAndando;

        }


        if (isWalking && isRunning && characterController.height == 1.72f)
        {

            somDoPersonagem = somDoPersonagemCorrendo;
        }

        WalkController();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpController();
        }

        isPushing = Input.GetKey(KeyCode.E);

        isCrounching = Input.GetKey(KeyCode.C);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        VerificaoSeta();

    }

    void VerificaoSeta()
    {

        rightArrow = Input.GetKey(KeyCode.RightArrow);
        //print(rightArrow);
        leftArrow = Input.GetKey(KeyCode.LeftArrow);
        //print(leftArrow);
        upArrow = Input.GetKey(KeyCode.UpArrow);
        //print(upArrow);
        downArow = Input.GetKey(KeyCode.DownArrow);
        //print(downArow);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        #region Verificando Colisão Caixa
        if (hit.gameObject.tag == "Box")
        {
            //print("Encostando no objeto");

            if (isPushing && (rightArrow || leftArrow || upArrow || downArow))
            {
                somDoPersonagem = somDoPersonagemInteragindoComACaixa;
                //print("Caixa esta sendo puxada");
                objectInteract = hit.gameObject;
                // print(hit.gameObject.name);
                // Rigidbody otherRB = objectInteract.GetComponent<Rigidbody>();
                objectInteract.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 15);
            }

            else if (!isPushing)
            {
                objectInteract = null;
                //print("Caixa ñ esta sendo puxada");
            }
        }
        #endregion
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }


}




