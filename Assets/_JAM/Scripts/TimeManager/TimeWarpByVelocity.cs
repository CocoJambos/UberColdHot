using UnityEngine;

public class TimeWarpByVelocity : MonoBehaviour
{
    [SerializeField]
    private FirstPersonCharacterController character;
    [SerializeField]
    private AnimationCurve timeScaleByVelocity;

    private void Update()
    {
        TimeManager.Instance.TimeScale = timeScaleByVelocity.Evaluate(character.velocity.magnitude);
        //Debug.Log($"Custom time scale: {TimeManager.Instance.TimeScale}");
    }
}