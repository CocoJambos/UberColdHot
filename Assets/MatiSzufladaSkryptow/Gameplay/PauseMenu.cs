using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private UIElement pauseUIElement;

    public bool IsPaused { get; private set; } = false;

    public void ChangePauseState()
    {
        IsPaused = !IsPaused;
        if(IsPaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    private void Pause()
    {
        pauseUIElement.Show();
        Time.timeScale = 0f;
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.None;
    }

    private void Unpause()
    {
        pauseUIElement.Hide();
        Time.timeScale = 1f;
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.Locked;
    }
}
