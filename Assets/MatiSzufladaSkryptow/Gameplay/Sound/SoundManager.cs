using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class SoundManager : DontDestroySingleBehaviour<SoundManager>
{
    [SerializeField]
    private AudioSource audioSourcePrefab;

    public void Play(AudioRecord audioRecord, Vector3 position, float volumeModifier = 1f)
    {
        AudioSource spawnedAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        float randomPitchValue = audioRecord.randomPitch ? UnityEngine.Random.Range(audioRecord.minPitch, audioRecord.maxPitch) : 1f;
        spawnedAudioSource.pitch = randomPitchValue;
        spawnedAudioSource.PlayOneShot(audioRecord.audioClip, audioRecord.volume * volumeModifier);

        Destroy(spawnedAudioSource.gameObject, audioRecord.audioClip.length / randomPitchValue);
    }
}

[Serializable]
public class AudioRecord
{
    public AudioClip audioClip;
    public float volume = 1f;
    public bool randomPitch = false;
    [ShowIf(nameof(randomPitch))]
    public float minPitch = 0.8f;
    [ShowIf(nameof(randomPitch))]
    public float maxPitch = 1.2f;
}