using UnityEngine;

public class AudioInvoker : MonoBehaviour
{
    [SerializeField] private AudioRecord _audioRecord;

    public void PlayAudio()
    {
        SoundManager.Instance.Play(_audioRecord, transform.position);
    }
    
    public void PlayAudio(Vector3 position)
    {
        SoundManager.Instance.Play(_audioRecord, position);
    }
}
