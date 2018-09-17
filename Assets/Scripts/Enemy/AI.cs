using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

//using UnityStandardAssets.Characters.FirstPerson;

public class AI : MonoBehaviour
{

    public ControlePersonagem player; // Player
    private AI ai;
    private SphereCollider enemyRange;
    private EnemyPatrol enemyPatrol;
    public GameObject visaoDaCabeca;
    public GameObject visaoDaCabecaAbaixada;


    public GameObject spotlight;



    // Informações do inimigo

    public float viewDistance = 10f;

    /*public float soundDistanceParado = 0.5f;
    public float soundDistanceAndandoAbaixado = 2.5f;
    public float soundDistanceAndando = 5f;
    public float soundDistancePulando = 7.5f;
    public float soundDistanceCorrendo = 10f;
    public float soundDistanceInteragindoComACaixa = 12f;
    */
    // Range


    //public float fos = 360f; //fieldOfSound
    public float fov = 120f; //AngleRange
    public bool isAware = false; // estado do inimigo


    public float timeToKill = 2;


    private NavMeshAgent agent;
    private Renderer renderer;
    private BoxCollider box;





    RaycastHit hit;





    public void Start()
    {

        isAware = false;
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        enemyPatrol = GetComponent<EnemyPatrol>();


    }

    public void Update()
    {
        spotlight.GetComponent<Light>().spotAngle = fov;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * viewDistance;

        float distanciaJogadorInimigo = Vector3.Distance(player.transform.position, transform.position);
        float anguloJogadorInimigo = Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position));

        /*Vector3 forwardSoundParado = transform.TransformDirection(Vector3.forward) * soundDistanceParado;
        Vector3 forwardSoundAndandoAbaixado = transform.TransformDirection(Vector3.forward) * soundDistanceAndandoAbaixado;
        Vector3 forwardSoundAndando = transform.TransformDirection(Vector3.forward) * soundDistanceAndando;
        Vector3 forwardSoundPulando = transform.TransformDirection(Vector3.forward) * soundDistancePulando;
        Vector3 forwardSoundCorrendo = transform.TransformDirection(Vector3.forward) * soundDistanceCorrendo;
        Vector3 forwardSoundInteragindoComACaixa = transform.TransformDirection(Vector3.forward) * soundDistanceInteragindoComACaixa;
        */

        #region Visao
        if (isAware && distanciaJogadorInimigo <= viewDistance
         && anguloJogadorInimigo <= fov / 2 && hit.transform.CompareTag("Player"))
        {
            
            //Chase player
            //agent.SetDestination(player.transform.position);
            renderer.material.color = Color.red;
        }
        else
        {

            SearchForPlayer();
            renderer.material.color = Color.blue;
        }
        #endregion
        /*
        #region Audio
        if (isAware && distanciaJogadorInimigo < soundDistanceParado
         && anguloJogadorInimigo < fos
         || isAware && distanciaJogadorInimigo < soundDistanceAndandoAbaixado
         && anguloJogadorInimigo < fos
         || isAware && distanciaJogadorInimigo < soundDistanceAndando
         && anguloJogadorInimigo < fos
         || isAware && distanciaJogadorInimigo < soundDistancePulando
         && anguloJogadorInimigo < fos
         || isAware && distanciaJogadorInimigo < soundDistanceCorrendo
         && anguloJogadorInimigo < fos
         || isAware && distanciaJogadorInimigo < soundDistanceInteragindoComACaixa
         && anguloJogadorInimigo < fos)

        {
            agent.SetDestination(player.transform.position);
            renderer.material.color = Color.red;
        }
        else
        {
            //SearchforPlayerSound();
            renderer.material.color = Color.blue;
        }
        #endregion
        */
        #region DrawLinesofSight
        if (viewDistance > 0)
        {
            if (player.characterState != CharacterState.crounching) //(player.isCrounching == false)
            {
                Debug.DrawLine(transform.position, visaoDaCabeca.transform.position, Color.red);
            }
            else if (player.characterState == CharacterState.crounching)
            {
                Debug.DrawLine(transform.position, visaoDaCabecaAbaixada.transform.position, Color.red);
            }


            Debug.DrawRay(transform.localPosition, forward, Color.green);
            Debug.DrawLine(transform.position, player.transform.position, Color.red);
        }
        #endregion

        #region DrawLineOfSound
        /*if (player.characterState==player.)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundParado, Color.cyan);
        }

        else if (player.somDoPersonagem <= player.somDoPersonagemAndandoAbaixado)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundAndandoAbaixado, Color.magenta);
        }

        else if (player.somDoPersonagem <= player.somDoPersonagemAndando)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundAndando, Color.yellow);
        }
        else if (player.somDoPersonagem <= player.somDoPersonagemPulando)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundPulando, Color.blue);
        }
        else if (player.somDoPersonagem <= player.somDoPersonagemCorrendo)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundCorrendo, Color.red);
        }
        else if (player.somDoPersonagem <= player.somDoPersonagemInteragindoComACaixa)
        {
            Debug.DrawRay(transform.localPosition, forwardSoundInteragindoComACaixa, Color.red);
        }

    */
        #endregion
    }

    #region ProcurandoPeloPlayer(Visao)
    public void SearchForPlayer()
    {

        float distanciaJogadorInimigo = Vector3.Distance(player.transform.position, transform.position);
        float anguloJogadorInimigo = Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position));
        if ( player.characterState != CharacterState.crounching)
        {
            if ( /*distanciaJogadorInimigo < viewDistance &&*/ anguloJogadorInimigo <= fov / 2)
            {

                if (Physics.Linecast(transform.position, visaoDaCabeca.transform.position, out hit,-10))
                {

                    if (hit.collider.CompareTag("Player"))
                    {

                        //Aware();
                        Debug.Log("Player detectado");




                    }
                }
            }
        }
        else if (isAware == false && player.characterState == CharacterState.crounching)
        {
            if (distanciaJogadorInimigo <= fov / 2 && anguloJogadorInimigo < viewDistance)
            {

                if (Physics.Linecast(transform.position, visaoDaCabecaAbaixada.transform.position, out hit, -1))
                {

                    if (hit.collider.CompareTag("Player"))
                    {

                        Aware();
                        Debug.Log("Player detectado");


                    }
                }
            }
        }


    }
    #endregion
    /*
    #region ProcurandoPeloPlayer(Som)
    public void SearchforPlayerSound()
    {

        if (isAware == false && player.somDoPersonagem == player.somDoPersonagemParado)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistanceParado)
                {

                    Aware();
                    Debug.Log("Som Parado Detectado");
                }
            }
        }
        else if (isAware == false && player.somDoPersonagem == player.somDoPersonagemAndandoAbaixado)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistanceAndandoAbaixado)
                {

                    Aware();
                    Debug.Log("Som Andando Abaixado Detectado");
                }
            }
        }
        else if (isAware == false && player.somDoPersonagem == player.somDoPersonagemAndando)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistanceAndando)
                {

                    Aware();
                    Debug.Log("Som Andando Detectado");
                }
            }
        }

        else if (isAware == false && player.somDoPersonagem == player.somDoPersonagemPulando)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistancePulando)
                {
                    Aware();
                    Debug.Log("Som Pulando Detectado");
                }
            }
        }

        else if (isAware == false && player.somDoPersonagem == player.somDoPersonagemCorrendo)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistanceCorrendo)
                {
                    Aware();
                    Debug.Log("Som Correndo Detectado");
                }
            }
        }
        else if (isAware == false && player.somDoPersonagem == player.somDoPersonagemInteragindoComACaixa)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fos)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < soundDistanceInteragindoComACaixa)
                {
                    Aware();
                    Debug.Log("Som Interagindo Com A Caixa Detectado");
                }
            }
        }

        else
        {
            Sleep();
        }

    }
    #endregion
    */
    public void Aware()
    {
        isAware = true;
    }

    public void Sleep()
    {
        isAware = false;

    }
    #region GameOver
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyPatrol.VelocidadeZero();
            player.enabled = false;

            Invoke("GameOver", timeToKill);
        }

    }

    public void GameOver()
    {
        SceneManager.LoadScene("1");
    }
    #endregion


}
// 1- Limpar os calculos
// 2- Criar arrays de métodos 