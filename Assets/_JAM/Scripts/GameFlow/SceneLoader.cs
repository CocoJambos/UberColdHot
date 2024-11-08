using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private List<SceneReference> m_GameplayScenes;
    [SerializeField] private List<SceneReference> m_SystemScenes;
    
    public bool IsLoadingActionDone;
    public SceneLoader Singleton => m_Instance;
    
    private Scene m_LoadedScene;
    private bool m_isLoadingActionDone;
    private static SceneLoader m_Instance;
    
    private void Start()
    {
        m_LoadedScene = SceneManager.GetActiveScene();
        m_Instance = this;
        
        DontDestroyOnLoad(this);
    }

    public void LoadSystemScenes() => StartCoroutine(LoadSystemScenesCO());
    
    private IEnumerator LoadSystemScenesCO()
    {
        m_isLoadingActionDone = false;
        foreach(SceneReference sceneReference in m_SystemScenes)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOperation.isDone);
        }
        m_isLoadingActionDone = true;
    }

    public void LoadMainScene(SceneID sceneID, float removeSceneDelay = 0f) =>
        StartCoroutine(LoadMainSceneCO(sceneID, removeSceneDelay));

    private IEnumerator LoadMainSceneCO(SceneID sceneID, float removeSceneDelay = 0f)
    {
        m_isLoadingActionDone = false;
        if(removeSceneDelay > 0f)
        {
            yield return new WaitForSeconds(removeSceneDelay);
        }

        SceneReference sceneReference = m_GameplayScenes.First(predicate => predicate.SceneID == sceneID);
        AsyncOperation loadSceneOperation =
            SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadSceneOperation.isDone);
        Scene loadedScene = SceneManager.GetSceneByBuildIndex(sceneReference.BuildIndex);
        SceneManager.SetActiveScene(loadedScene);

        AsyncOperation removeSceneOperation = SceneManager.UnloadSceneAsync(m_LoadedScene);
        yield return new WaitUntil(() => removeSceneOperation.isDone);
        m_LoadedScene = loadedScene;
        m_isLoadingActionDone = true;
    }
}
