using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    //public Animator animator;
    public GameObject FadeOut;
    


    #region Pause
    PauseScript ps;
    #endregion
    #region Canvas
    public GameObject canvasMenu;
    public GameObject canvasSettings;
    #endregion
    #region Atributos da Câmera
    Vector3 cameraPosInicial = new Vector3(354, 1, -620);
    Vector3 cameraPosFinal = new Vector3(460, 1, -580);
    Vector3 cameraPosPlay = new Vector3(465, 1, -620);

    Vector3 cameraRotInicial = new Vector3(0, 0, 0);
    Vector3 cameraRotFinal = new Vector3(0, 70, 0);
    #endregion
    #region EstadoDoClick
    public bool playClick = false;
    public bool settingsClick = false;
    public bool backClick = false;
    #endregion


    void Start()
    {
        
        ps = GetComponent<PauseScript>();

        playClick = false;
        settingsClick = false;
        backClick = false;

        DisableSettings();
    }

    public void LateUpdate()
    {

        if (settingsClick == true)
        {
           
            
            DisableMenu();
            
            Invoke("EnableSettings", 0.5f);
            Settings();
        }

        if (backClick == true)
        {

            Back();
            DisableSettings();

            Invoke("EnableMenu", 0.5f);


        }

        if (playClick == true)
        {
            FadeOut.SetActive(true);

        }
    }
    public void PlayGame()
    {
        playClick = true;
        settingsClick = false;
        backClick = false;

        Time.timeScale = 1f;
        StartCoroutine(WaitForSceneLoad());

    }

    public void Settings()
    {

        settingsClick = true;
        backClick = false;

        if (transform.position != cameraRotFinal)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPosFinal, 0.05f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, cameraRotFinal, 0.05f);
        }


    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        settingsClick = false;
        backClick = true;

        if (transform.position != cameraRotInicial)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPosInicial, 0.05f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, cameraRotInicial, 0.05f);
        }

    }

    void EnableMenu()
    {
        canvasMenu.SetActive(true);
    }
    void DisableMenu()
    {
        canvasMenu.SetActive(false);
    }
    void EnableSettings()
    {
        canvasSettings.SetActive(true);
    }
    void DisableSettings()
    {
        canvasSettings.SetActive(false);
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Loading");

    }


   

  

}


