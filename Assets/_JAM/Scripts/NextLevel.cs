using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public float lodalLevelDelay;
    [SerializeField]
    private bool allObjectivesCompleted = false;
    [SerializeField]
    private UnityEvent onBeforeNewLevelLoaded;

    private WaitForSeconds delayRoutine;

    private void Awake()
    {
        delayRoutine = new WaitForSeconds(lodalLevelDelay);
    }

    public void SetAllObjectivesCompleted(bool areAllObjectivesCompleted)
    {
        allObjectivesCompleted = areAllObjectivesCompleted;
    }

    public void LoadNextLevelIfAllObjectivesAreCompleted()
    {
        if(!allObjectivesCompleted)
            return;

        onBeforeNewLevelLoaded.Invoke();
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(lodalLevelDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
