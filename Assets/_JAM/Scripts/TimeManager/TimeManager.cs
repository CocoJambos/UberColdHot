using UnityEngine;


public class TimeManager : SingleBehaviour<TimeManager>
{
    private float _timeScale = 1f;

    public float TimeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = Mathf.Clamp(value, 0f, 1f);
        }
    }
}
