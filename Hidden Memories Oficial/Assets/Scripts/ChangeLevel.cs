using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public GameObject FadeOutPreFab;
    public string loadLevel;
    bool onTrigger = false;
    public void Start()
    {
        onTrigger = false;
    }

    private void Update()
    {
        if(onTrigger==true)
        {
            StartCoroutine(WaitChangeLevel());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            onTrigger = true;
            FadeOutPreFab.SetActive(true);
        }
    }
    
 
    private IEnumerator WaitChangeLevel()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(loadLevel);
        
    }

}
