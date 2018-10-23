using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(WaitForSceneLoad());
    }
    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("3");

    }
}
