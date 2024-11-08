using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private UIDocument pauseUIElement;

    public bool IsPaused { get; private set; } = false;

    public void Start()
    {
        pauseUIElement.enabled = false;
    }

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
        pauseUIElement.enabled = true;
        Time.timeScale = 0f;
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.None;
    }

    private void Unpause()
    {
        pauseUIElement.enabled = false;
        Time.timeScale = 1f;
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.Locked;
    }
}
