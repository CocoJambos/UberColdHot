using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public float lodalLevelDelay;
    
    private WaitForSeconds delayRoutine;

    private void Awake()
    {
        delayRoutine = new WaitForSeconds(lodalLevelDelay);    
    }

    public void LoadNextLevel()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(lodalLevelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
