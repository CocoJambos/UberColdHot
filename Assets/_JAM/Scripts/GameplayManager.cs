using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : SingleBehaviour<GameplayManager>
{
    [SerializeField]
    private PauseMenu pauseMenu;

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangePauseState() => pauseMenu.ChangePauseState();
}
