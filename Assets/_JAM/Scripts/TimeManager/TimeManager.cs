using UnityEngine;


public class TimeManager : SingleBehaviour<TimeManager>
{
    private float _timeScale = 1f;
    
    public float TimeScale => _timeScale;

    public void SetCurrentTimeScale(float timeScale)
    {
        _timeScale = Mathf.Clamp(timeScale, 0f, 1f);
    }
}
