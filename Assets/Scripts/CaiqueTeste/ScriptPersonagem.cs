using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    #region Campo

    public enum _CharacterState
    {
        idle,//okay
        walking,//okay
        running,//okay
        crouch,//okay
        crouchWalking,//okay
        push,//okay
        pushing,//teste
        climb,
        hanging,
        hangingWalking,
        jump,//Okay
        dead
    }

    public _CharacterState _characterState;

    CharacterController characterController;
    Animator anim;
    Camera mainCamera;
    //IKCharacter ikPersonagem;

    Vector3 moveInput;
    Vector3 moveVelocity;

    RaycastHit hit;

    public GameObject box;

    #region Botões
    public bool run;
    public bool crouch;
    public bool jump;
    public bool push;
    #endregion

    #region Personagem Velocidades
    public float characterMoveSpeed;
    float currentSpeed;
    float walkSpeed = 2;
    float runSpeed = 4;
    float crouchSpeed = 1;
    #endregion

    #region Salto
    float jumpHeight = 1;
    float gravity = -12;
    float velocityY;
    #endregion

    #region Controle do ar
    [Range(0, 1)]
    public float airControlPercent;
    #endregion

    #region //Smooth rotação de personagem
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    #endregion

    #region //Smooth velocidade 
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    #endregion

    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        //ikPersonagem = GetComponent<IKCharacter>();

        mainCamera = Camera.main;

        _characterState = _CharacterState.idle;
    }

    void Raycast()
    {
        Ray ray;
        float rayDistance = 4;

        ray = new Ray(this.transform.position + new Vector3(0f, characterController.center.y, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < rayDistance)
            {
                //Debug.Log("hit something");

                if (hit.collider.tag == "Box")
                {
                    Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
                    box = hit.collider.gameObject;
                    //box.GetComponent<Rigidbody>().AddForce(this.transform.forward * 50);
                }
                else if (hit.collider.tag != "Box")
                {
                    Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

                }
            }

            // Debug.Log("Don't hit something");
        }

    }

    void MoveInput()
    {
        float leftMove = Input.GetAxis("Horizontal");
        float rightMove = Input.GetAxis("Vertical");

        moveInput = new Vector3(leftMove, 0, rightMove);
        moveInput = moveInput.normalized;
    }

    void CameraFoward()
    {
        Vector3 cameraFoward = mainCamera.transform.forward;
        cameraFoward.y = 0;

        Quaternion cameraRelativeRotation = Quaternion.FromToRotation(Vector3.forward, cameraFoward);
        Vector3 lookToward = cameraRelativeRotation * moveInput;

        if (moveInput.sqrMagnitude > 0)
        {
            Ray lookRay = new Ray(transform.position, lookToward);
            transform.LookAt(lookRay.GetPoint(1));
        }
    }

    private void FixedUpdate()
    {
        Raycast();
    }

    private void Update()
    {
        UpdateButtons();
        MoveInput();
        MoveCharacter(moveInput);
        UpdateCharacterState();
        CharacterStateSet();

        Animating(crouch);
    }

    private void LateUpdate()
    {
        CameraFoward();
    }

    void UpdateButtons()
    {
        run = Input.GetKey(KeyCode.LeftShift);
        crouch = Input.GetKey(KeyCode.C);
        jump = Input.GetButton("Jump");
        push = Input.GetKey(KeyCode.E);
    }

    void UpdateCharacterState()
    {

        //Pula
        if (jump)
        {
            _characterState = _CharacterState.jump;
        }
        //climb
        else if (jump && _characterState == _CharacterState.walking)
        {
            _characterState = _CharacterState.climb;
        }
        //Sobe
        else if ((jump && box != null) && characterController.isGrounded)
        {
            _characterState = _CharacterState.climb;
        }
        //Prepara para puxar
        else if (push && box != null)
        {
            _characterState = _CharacterState.push;
        }
        //Puxando
        else if ((push && moveInput != Vector3.zero) && box != null)
        {
            _characterState = _CharacterState.pushing;
        }
        //anda agachado
        else if (crouch && moveInput != Vector3.zero)
        {
            _characterState = _CharacterState.crouchWalking;
        }
        //agacha
        else if (crouch)
        {
            _characterState = _CharacterState.crouch;
        }
        //corre
        else if (run && moveInput != Vector3.zero)
        {
            _characterState = _CharacterState.running;


        }
        //anda
        else if (moveInput != Vector3.zero)
        {
            _characterState = _CharacterState.walking;
        }
        //parado
        else if (moveInput == Vector3.zero)
        {
            _characterState = _CharacterState.idle;
            anim.SetFloat("SpeedPercent", 0);

        }

    }

    void CharacterStateSet()
    {
        if (_characterState == _CharacterState.jump)
        {
            if (characterController.isGrounded)
            {
                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                velocityY = jumpVelocity;
            }
        }

        else if (_characterState == _CharacterState.idle)
        {
            characterMoveSpeed = 0 * moveInput.magnitude; ;
            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);
        }

        else if (_characterState == _CharacterState.walking)
        {
            characterMoveSpeed = walkSpeed * moveInput.magnitude;
            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);

        }

        else if (_characterState == _CharacterState.running)
        {
            characterMoveSpeed = runSpeed * moveInput.magnitude;

            //ChangeCollider
            characterController.height = 1.6f;
            characterController.center = new Vector3(0, 0.87f, 0);
        }

        else if (_characterState == _CharacterState.crouchWalking || _characterState == _CharacterState.crouch)
        {
            characterMoveSpeed = crouchSpeed * moveInput.magnitude;

            //ChangeCollider
            characterController.height = 1.3f;
            characterController.center = new Vector3(0, 0.70f, 0);
        }

    }

    void MoveCharacter(Vector3 moveInput)
    {
        #region PROBLEMA
        if (moveInput != Vector3.zero)
        {
            //Problema
            float targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifieldSmoothTime(turnSmoothTime));
        }
        #endregion

        #region Revisar
        currentSpeed = Mathf.SmoothDamp(currentSpeed, characterMoveSpeed, ref speedSmoothVelocity, GetModifieldSmoothTime(speedSmoothTime));

        //Pulo
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        moveVelocity = velocity;
        characterController.Move(moveVelocity * Time.deltaTime);

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }
        #endregion
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

    void Animating(bool crouching)
    {
        float animationSpeedPercent = ((_characterState == _CharacterState.running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        anim.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

}
