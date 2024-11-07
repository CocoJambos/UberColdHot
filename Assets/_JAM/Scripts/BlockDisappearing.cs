using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockDisappearing : MonoBehaviour
{
    [SerializeField]
    private TweenController disappearingTween;
    [SerializeField]
    [FormerlySerializedAs("appear")]
    private bool appearAfterDisappearing = true;
    [SerializeField]
    [ShowIf(nameof(appearAfterDisappearing))]
    private TweenController appearingTween;
    [SerializeField]
    [ShowIf(nameof(appearAfterDisappearing))]
    private float appearingDelay = 5f;

    private bool isAppearingOrDisappearing = false;

    public void TryToStartDisappearingOnPlayerTouch()
    {
        if(isAppearingOrDisappearing)
            return;

        StartDisappearing();
        if(appearAfterDisappearing)
        {
            Invoke(nameof(StartAppearing), appearingDelay);
        }
    }

    private void StartDisappearing()
    {
        isAppearingOrDisappearing = true;
        disappearingTween.PlayTween();
    }

    private void StartAppearing()
    {
        appearingTween.PlayTween(() => isAppearingOrDisappearing = false);
    }
}
