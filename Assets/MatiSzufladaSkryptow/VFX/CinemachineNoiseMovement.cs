using System;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineNoiseMovement : MonoBehaviour, INoiseMovement
{
    [SerializeField]
    private ValueProgressedByOffset amplitudeGainOffsetProgression;
    [SerializeField]
    private ValueProgressedByOffset frequencyGainOffsetProgression;

    private CinemachineBasicMultiChannelPerlin VCamNoise
    {
        get
        {
            if(getCurrentVCam != null && getCurrentVCam())
            {
                return (CinemachineBasicMultiChannelPerlin)getCurrentVCam().GetCinemachineComponent(CinemachineCore.Stage.Noise);
            }

            return null;
        }
    }

    private float CurrentProgress
    {
        get => currentProgress;
        set
        {
            currentProgress = value;
            if(VCamNoise)
            {
                VCamNoise.AmplitudeGain = amplitudeGainOffsetProgression.GetValue(CurrentProgress);
                VCamNoise.FrequencyGain = frequencyGainOffsetProgression.GetValue(CurrentProgress);
            }
        }
    }

    private Func<CinemachineCamera> getCurrentVCam;
    private float currentProgress;

    public void Init(Func<CinemachineCamera> getCurrentVCam)
    {
        this.getCurrentVCam = getCurrentVCam;
    }

    public void UpdateGainOffsetsOnVCamChanged(CinemachineCamera _)
    {
        UpdateProgress(CurrentProgress);
    }

    public void UpdateProgress(float noiseProgress) => CurrentProgress = noiseProgress;
}
