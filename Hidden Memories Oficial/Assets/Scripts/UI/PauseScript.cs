using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
   
    public  bool GameIsPaused = false;
   
    public GameObject pauseMenuUI;
    


    private void Start()
    {
        
        
        GameIsPaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
       
        GameOn();


    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        GamePaused();
        
        
    }

    public void Menu()
    {
        GamePaused();
        SceneManager.LoadScene("Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void GamePaused()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
    }
    public void GameOn()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;

    }
}

