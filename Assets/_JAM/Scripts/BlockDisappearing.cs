using UnityEngine;

public class BlockDisappearing : MonoBehaviour
{
    [SerializeField]
    private TweenController disappearingTween;
    [SerializeField]
    private TweenController appearingTween;
    [SerializeField]
    private float appearingDelay = 5f;

    private bool isAppearingOrDisappearing = false;

    public void TryToStartDisappearingOnPlayerTouch()
    {
        return;
        if(isAppearingOrDisappearing)
            return;

        StartDisappearing();
        Invoke(nameof(StartAppearing), appearingDelay);
    }

    private void StartDisappearing()
    {
        isAppearingOrDisappearing = true;
        disappearingTween.Play();
    }

    private void StartAppearing()
    {
        appearingTween.Play(() => isAppearingOrDisappearing = false);
    }
}
