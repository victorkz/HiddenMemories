using UnityEngine.EventSystems;
using UnityEngine;

//https://www.youtube.com/watch?v=qITXjT9s9do

public class ControlePersonagemTeste : MonoBehaviour
{/*
    #region Campo
    public enum CharacterState
    {
        idle,
        walking,
        running,
        crounching,
        pushing,
        dead
    }

    public CharacterState characterState;//Estado do personagem
    public Interactable actualInteractable;//Objeto que interage par ao inventario
    public ForceTeste box;

    public GameObject inventory;
    CharacterController controlePersonagem;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float characterForce;

    private float characterMoveSpeed;
    private float currentSpeed;

    public Vector3 moveInput;
    public Vector3 moveVelocity;
    private Camera mainCamera;

    public float jumpHeight = 1;
    public float gravity = -12;
    float velocityY;

    public GameObject objectInteract = null;

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

    #endregion

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

    void MoveInput()
    {
        float leftMove = Input.GetAxis("Horizontal");
        float rightMove = Input.GetAxis("Vertical");

        moveInput = new Vector3(leftMove, 0, rightMove);
        moveInput = moveInput.normalized;
        // Debug.Log(moveInput);
    }

    void Update()
    {
        MoveInput();

        bool running = Input.GetKey(KeyCode.LeftShift);
        bool crounching = Input.GetKey(KeyCode.C);
        bool interacting = Input.GetKey(KeyCode.E);

        bool action = false;


        //State Walking
        if (moveInput != Vector3.zero)
        {
            characterState = CharacterState.walking;
        }
        //State Running
        if (running)
        {
            characterState = CharacterState.running;
        }
        //State Crounching
        if (crounching)
        {
            characterState = CharacterState.crounching;
        }
        //State Idle
        if (moveInput == Vector3.zero)
        {
            characterState = CharacterState.idle;
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !crounching)
        {
            Jump();
        }

        if (characterState == CharacterState.pushing)
        {
            moveInput = Vector3.zero;

        }


        //void BoxDistance();

        // Debug.DrawLine(mainCamera.transform.position, this.transform.position, Color.green, 1f);
        // Debug.DrawRay(mainCamera.transform.position, cameraForward, Color.green);

        Raycast();

        // print(characterState);
        // print(crounching);
        MoveCharacter(moveInput);
        CameraFoward();
        Animating(crounching, action);
    }

    void Raycast()
    {
        RaycastHit hit;
        Ray ray;
        float rayDistance = 4;

        ray = new Ray(transform.position + new Vector3(0f, characterController.center.y, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < rayDistance)
            {
                //Debug.Log("hit something");
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
            }

            // Debug.Log("Don't hit something");
        }

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

        float dist = Vector3.Distance(this.transform.position, hit.transform.position);
        //print("distance:" + dist);

        //Interação com caixas
        if (hit.gameObject.tag == "Box")
        {
            print("entrou");
            InteractingBox(hit, dist);

        }


        #region inventario
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                IsInteracting(interactable);
            }
        }
        #endregion

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
        if (characterState == CharacterState.pushing)
        {
            animator.SetBool("IsPushing", action);
        }


        // if (characterState.ToString() == "crounching")
        //animator.SetBool("IsCrouch", (characterState.ToString() == "crounching"));
        // animator.SetFloat("TurnPercent", turnAngle);
        //print(characterController.velocity.magnitude);
    }


    void InteractingBox(ControllerColliderHit Box, float distance)
    {

        box = Box.collider.GetComponent<ForceTeste>();
        if (Box.gameObject.tag == "Box" && Input.GetKey(KeyCode.E))
        {

            box.CharacterInteract(this);

            print("mudou estado");

        }
        else
            box.NoCharacterInteract(this);
    }

    #region inventario
    void IsInteracting(Interactable _new)
    {
        if (_new != actualInteractable)
        {
            if (actualInteractable != null)
            {
                actualInteractable.OnDefocused();
            }


            actualInteractable = _new;
        }
        _new.OnFocused(transform);
    }

    void RemoveInteracting()
    {
        if (actualInteractable != null)
            actualInteractable.OnDefocused();

        actualInteractable = null;
    }
    #endregion



    // print("Maior");
    /*
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
        characterState = CharacterState.pushing;*/

}
