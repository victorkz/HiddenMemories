using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlRB : MonoBehaviour
{
    //public GameObject Fade;
    private bool inv = false;

    // SoundChase
    //public float soundIntensity = 5f;
    public float walkSoundRadius = 1f; // 0
    public float runSoundRadius = 2f; // 1
    public float croachWalkSoundRadius = 0.5f;

    private AudioSource audioSource;
    private CharacterControlRB player;
    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider;
    private PauseScript PS;

    public float walkSpeed = 2;//Velocidade de caminhada
    public float runSpeed = 6;//Velocidade de corrida

    /*
    public float CrouchSpeed;
    public float pushingSpeed;
    */
    Animator animator;

    #region //Smooth de rotação do personagem
    [Range(0, 3)]//Slider da variavel de baixo
    public float turnSmoothTime = 0.2f;//Suavização do tempo de rotação do personagem
    float turnSmoothVelocity;
    #endregion

    #region //Smooth da velocidade do personagem
    [Range(0.0f, 0.5f)]
    public float speedSmoothTime = 0.1f;//Suavização do tempo de inicio de velocidade do personagem
    float speedSmoothVelocity;
    #endregion

    float currentSpeed;//Velocidade atual

    //Rigibody
    Rigidbody rb;

    #region Raycast
    bool RayisGrounded; //O personagem está no chão?
    bool RayisPushing;//O personagem pode empurrar?
    //Layers
    public LayerMask Box;
    public LayerMask mask;
    #endregion

    /*//Gravidade para pulo
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    */

    [Range(0, 1)]
    public float airControlPercent;
    public float jumpVelocity = 2;

    //Andar
    float animationSpeedPorcent;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        player = GetComponent<CharacterControlRB>();
        sphereCollider = GetComponent<SphereCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        PS = GetComponent<PauseScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        
        
        RayFloor();
        RayPush();

        //Pegamos input do jogador//Se eu usar um vetor tridimensional o personagem não rotaciona
        Vector2 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDir = playerInput.normalized;//Passamos o input do jogador para outro vetor, onde normalizamos ele

        //Personagem corre
        bool running = Input.GetKey(KeyCode.LeftShift);

        Move(inputDir, running);

        // Crouched
        bool crounch = Input.GetKey(KeyCode.LeftControl);

        Crouching(inputDir, crounch);

        //Empurrando
        bool push = Input.GetKey(KeyCode.E);
        Pushing(inputDir, push, RayisPushing);



        //Pulo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Jogador apertou o espaço");
            Jump();
        }

        Hide();
        UnHide();
    }
    

    private void Move(Vector3 inputDir, bool running)
    {
        //Rotação do personagem
        if (inputDir != Vector3.zero)
        {
            //Deixa a rotação do persoangem mais suave
            float playerRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;//Rotação do personagem
            rb.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, playerRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));//Calculamos o SmoothAngle                                                                                                                                                           // transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        }

        //Se o personagem esta correndo igual a runSpeed se não é walkSpeed e se não tiver nada pressionado multiplicado por magnitude será igual a zero
        float PlayerSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDampAngle(currentSpeed, PlayerSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        //Mover o personagem na direção em que ele está olhando
        //rb.transform.Translate(transform.forward * PlayerSpeed * Time.deltaTime, Space.World);
        //Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;//Movimento do personagem com o CharacterController, adicionando gravidade junto
        Vector3 velocity = transform.forward * currentSpeed;//Movimento do personagem com o CharacterController, adicionando gravidade junto

        //Movimento
        rb.transform.Translate(velocity * Time.deltaTime, Space.World);

        //Para fazer o personagem para de correr quando encostar em algo
        currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;

        //Tempo da porcentagem da animação do blendTree
        //float animationSpeedPorcent = ((running) ? 1 : .5f) * inputDir.magnitude;
        animationSpeedPorcent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPorcent, speedSmoothTime, Time.deltaTime);//Deixamos a blendTree mais suave
    }

    void Jump()
    {
        if (RayisGrounded)
        {
            //Jogador pulando
            rb.velocity = Vector2.up * jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float SmoothTime)
    {
        if (RayisGrounded)
        {
            return SmoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return SmoothTime / airControlPercent;
    }

    void RayFloor()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 0.1f, mask))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            // print(hitInfo.collider.gameObject.layer);
            RayisGrounded = true;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 0.1f, Color.green);
            RayisGrounded = false;
        }
    }

    void RayPush()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 0.5f, Box))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            RayisPushing = true;

            //print("Interagindo com a caixa");
        }

        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 0.5f, Color.green);
            RayisPushing = false;

            //print("Não interagindo com a caixa");
        }
    }

    void Crouching(Vector3 inputDir, bool crounch)
    {
        if (crounch)
        {

            animator.SetBool("IsCrouch", crounch);
            if (crounch && animationSpeedPorcent > 0)
            {
                animationSpeedPorcent = ((crounch) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
                animator.SetFloat("IsWalkingCrouch", animationSpeedPorcent, speedSmoothTime, Time.deltaTime);
                
            }
        }
        else
            animator.SetBool("IsCrouch", crounch);

    }

    void Pushing(Vector3 inputDir, bool push, bool RayisPushing)
    {
        if (push && RayisPushing)
        {
            animator.SetBool("IsPush", push);
            if (push && animationSpeedPorcent > 0)
            {
                animationSpeedPorcent = ((push) ? 1 : .5f) * inputDir.magnitude;
                //animationSpeedPorcent = ((push) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
                animator.SetFloat("IsWalkingPush", animationSpeedPorcent, speedSmoothTime, Time.deltaTime);
            }
        }
        else
            animator.SetBool("IsPush", push);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<AI>().Aware();

        }
    }
    public void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {

            other.gameObject.GetComponent<AI>().Sleep();
        }
    }
    public void Hide()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            inv = false;
            transform.gameObject.tag = "Inv";
        }

    }

    public void UnHide()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            inv = true;
            transform.gameObject.tag = "Player";
        }

    }




}







