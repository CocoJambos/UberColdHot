using System.Collections;
using UnityEngine;

public class GateAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioInvoker _audioInvoker;

    public float delayTime = 3f;
    
    IEnumerator InfoMessageRoutine()
    {
        yield return new WaitForSeconds(delayTime);
        _audioInvoker.PlayAudio();
    }
    
    public void PlayGateOpenAudio()
    {
        StartCoroutine(InfoMessageRoutine());
    }
}
