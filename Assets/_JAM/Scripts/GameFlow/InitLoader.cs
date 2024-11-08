using System;
using System.Collections;
using UnityEngine;

public class InitLoader : MonoBehaviour
{
    [SerializeField] private SceneID m_startingScene;

    private void Start()
    {
        StartCoroutine(BootstrapCO());
    }

    private IEnumerator BootstrapCO()
    {
        yield return new WaitUntil(() => SceneLoader.Singleton != null);
        SceneLoader sceneLoader = SceneLoader.Singleton;
        sceneLoader.LoadSystemScenes();

        yield return new WaitUntil(() => sceneLoader.IsLoadingActionDone);
        
        sceneLoader.LoadMainScene(m_startingScene);
        yield return new WaitUntil(() => sceneLoader.IsLoadingActionDone);
    }
}
