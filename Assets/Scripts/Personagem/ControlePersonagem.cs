using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=qITXjT9s9do
public enum CharacterState
{
    idle,
    walking,
    running,
    crounching,
    pushing,
    dead
}
public class ControlePersonagem : MonoBehaviour
{
   

    public CharacterState characterState;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float characterForce;

    private float characterMoveSpeed;
    private float currentSpeed;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Camera mainCamera;

    public float jumpHeight = 1;
    public float gravity = -12;
    float velocityY;

    private bool inv = false;

    public GameObject objectInteract = null;
    public IA ia;

    #region Controle do ar
    [Range(0, 1)]
    public float airControlPercent;
    #endregion

    CharacterController characterController;
    Animator animator;

    #region //Smooth rotação de personagem
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    #endregion

    #region //Smooth velocidade 
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    #endregion

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        mainCamera = Camera.main;

        characterMoveSpeed = 0;

        characterState = CharacterState.idle;
    }

    void CameraFoward()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;

        Quaternion cameraRelativeRotation = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        Vector3 lookToward = cameraRelativeRotation * moveInput;

        if (moveInput.sqrMagnitude > 0)
        {
            Ray lookRay = new Ray(transform.position, lookToward);
            transform.LookAt(lookRay.GetPoint(1));
        }
    }

    void Update()
    {
        float leftMove = Input.GetAxis("Horizontal");
        float rightMove = Input.GetAxis("Vertical");

        moveInput = new Vector3(leftMove, 0, rightMove);
        moveInput = moveInput.normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        bool crounching = Input.GetKey(KeyCode.C);
        bool pushing = Input.GetKey(KeyCode.E);

        bool action = false;

        if (moveInput != Vector3.zero)
        {
            characterState = CharacterState.walking;
        }

        if (running)
        {
            characterState = CharacterState.running;
        }

        if (crounching)
        {
            characterState = CharacterState.crounching;
        }

        if (moveInput == Vector3.zero && !crounching)
        {
            characterState = CharacterState.idle;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !crounching)
        {
            Jump();
        }
        /*
        if (objectInteract != null)
        {
            characterState = CharacterState.pushing;
            action = pushing;
        }*/

        //void BoxDistance();

        // Debug.DrawLine(mainCamera.transform.position, this.transform.position, Color.green, 1f);
        // Debug.DrawRay(mainCamera.transform.position, cameraForward, Color.green);

        print(characterState);
        print(crounching);
        MoveCharacter(moveInput);
        CameraFoward();
        Animating(crounching, action);
        Hide();
        UnHide();
    }

    void MoveCharacter(Vector3 moveInput)
    {
        if (moveInput != Vector3.zero)
        {
            //Problema
            float targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifieldSmoothTime(turnSmoothTime));

            //transform.eulerAngles = Vector3.up * Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        }

        #region Condição Caminhada

        if (characterState.ToString() == "walking")
        {
            characterMoveSpeed = walkSpeed * moveInput.magnitude;

            //ChangeCollider
            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);

            //print("Entrou no estado:" + characterState.ToString());

            // print(characterState.ToString() + ": " + characterMoveSpeed);
        }

        else if (characterState.ToString() == "running")
        {
            characterMoveSpeed = runSpeed * moveInput.magnitude;

            //ChangeCollider
            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);

            // print("Entrou no estado:" + characterState.ToString());

            // print(characterState.ToString() + ": " + characterMoveSpeed);
        }

        else if (characterState.ToString() == "crounching")
        {
            //print("Entrou agachado");
            characterMoveSpeed = crouchSpeed * moveInput.magnitude;

            //ChangeCollider
            characterController.height = 1.3f;
            characterController.center = new Vector3(0, 0.70f, 0);

            // print("Entrou no estado:" + characterState.ToString());

            // print(characterState.ToString() + ": " + characterMoveSpeed);
        }

        else if ((characterState.ToString() == "idle"))
        {
            characterMoveSpeed = walkSpeed * moveInput.magnitude;

            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);

            // print("Entrou no estado:" + characterState.ToString());
        }



        #endregion

        //moveSpeed = ((characterState == CharacterState.running) ? runSpeed : walkSpeed) * moveInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, characterMoveSpeed, ref speedSmoothVelocity, GetModifieldSmoothTime(speedSmoothTime));

        //Gravidade
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;// Pular
        //moveVelocity = transform.forward * moveSpeed * currentSpeed + Vector3.up * velocityY;

        //Velocidade personagem
        moveVelocity = velocity;
        characterController.Move(moveVelocity * Time.deltaTime);//Atualiza personagem

        //Evitar colisão
        if (objectInteract == null)
        {
            currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        }

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }

        // print("estado atual fora do laço:" + characterState);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool isPushing = false;

        float dist = Vector3.Distance(this.transform.position, hit.transform.position);
        //print("distance:" + dist);

        if (hit.gameObject.tag == "Box" && dist >= 0.8f)
        {
            // print("Maior");

            if (Input.GetKey(KeyCode.E))
            {
                characterState = CharacterState.pushing;
                objectInteract = hit.gameObject;
                // print("Entrou");
                objectInteract.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 100);


                isPushing = true;

                objectInteract.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * characterForce);

                //print("dentro do colisor:" + characterState);
            }
            else
                characterState = CharacterState.pushing;

        }

        else if (dist > 2)
        {
            //objectInteract = null;
            objectInteract = null;
            isPushing = false;
        }
    }

    void Jump()
    {
        if (characterController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifieldSmoothTime(float smoothTime)
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

    void Animating(bool crounching, bool action)
    {
        float animationSpeedPercent = ((characterState == CharacterState.running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        animator.SetBool("IsCrouch", crounching);
        animator.SetBool("IsPushing", action);



        // if (characterState.ToString() == "crounching")
        //animator.SetBool("IsCrouch", (characterState.ToString() == "crounching"));
        // animator.SetFloat("TurnPercent", turnAngle);
        //print(characterController.velocity.magnitude);
    }

    public void Hide()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ia.enabled = false;
            ia.isAware = false; 
            inv = false;
            ia.renderer.material.color = Color.blue;
            transform.gameObject.tag = "Inv";
        }

    }

    public void UnHide()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
           
            ia.enabled = true;
            inv = true;
            
            transform.gameObject.tag = "Player";
        }

    }
}
