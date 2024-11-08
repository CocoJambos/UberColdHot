using UnityEngine;

public enum SceneID
{
    //Gameplay
    FirstLevel,
    SecondLevel,
    ThirdLevel,
    MainMenu,
    EndScreen,
    Credits,
    //Systems
    UI,
}

[CreateAssetMenu(fileName = "SceneReference", menuName = "Scriptable Objects/SceneReference")]
public class SceneReference : ScriptableObject
{
    [SerializeField] private int m_BuildIndex = 0;
    [SerializeField] private SceneID m_SceneID;
    [SerializeField] private bool m_IsSystemScene;

    public int BuildIndex => m_BuildIndex;
    public SceneID SceneID => m_SceneID;
    public bool IsSystemScene => m_IsSystemScene;
}
