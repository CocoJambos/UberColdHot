using UnityEngine.SceneManagement;

public class GameplayManager : SingleBehaviour<GameplayManager>
{
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
