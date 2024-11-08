using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockDisappearing : MonoBehaviour
{
    [SerializeField]
    private TweenController disappearingTween;
    [SerializeField]
    private float disappearingDelay = 1f;
    [SerializeField]
    [FormerlySerializedAs("appear")]
    private bool appearAfterDisappearing = true;
    [SerializeField]
    [ShowIf(nameof(appearAfterDisappearing))]
    private TweenController appearingTween;
    [SerializeField]
    [ShowIf(nameof(appearAfterDisappearing))]
    private float appearingDelay = 5f;

    [SerializeField] private AudioInvoker[] _disappearingCubeInvokers;


    private bool isAppearingOrDisappearing = false;

    public void TryToStartDisappearingOnPlayerTouch() => TryToStartDisappearingOnPlayerTouch(0f);

    public void TryToStartDisappearingOnPlayerTouch(float additionalDisappearingDelay)
    {
        if(isAppearingOrDisappearing)
            return;

        Invoke(nameof(StartDisappearing), disappearingDelay + additionalDisappearingDelay);

        if(appearAfterDisappearing)
        {
            Invoke(nameof(StartAppearing), appearingDelay);
        }
    }

    private void StartDisappearing()
    {
        isAppearingOrDisappearing = true;
        // if(_disappearingCubeInvokers != null && _disappearingCubeInvokers.Length > 0)
        //   _disappearingCubeInvokers[Random.Range(0,_disappearingCubeInvokers.Length)].PlayAudio();
        disappearingTween.PlayTween();
    }

    private void StartAppearing()
    {
        appearingTween.PlayTween(() => isAppearingOrDisappearing = false);
    }
}
