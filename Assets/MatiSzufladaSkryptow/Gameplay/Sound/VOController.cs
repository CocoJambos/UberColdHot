using System.Collections;
using UnityEngine;

public class VOController : SingleBehaviour<VOController>
{
    public bool _playInitAlert = false;

    public AudioSource _initAlert;
    public AudioInvoker _infoAlert;

    public AudioInvoker _playerDeathAlert;
    public AudioInvoker _gateOpenAlert;

    private void Start()
    {
        if(_playInitAlert)
        {
            _initAlert.Play();
        }

        StartCoroutine(InfoMessageRoutine());
    }

    public void PlayPlayerDeathMessage()
    {
        _playerDeathAlert.PlayAudio(PlayerTransform.Get().position);
    }

    public void PlayGateOpenMessage()
    {
        _gateOpenAlert.PlayAudio(PlayerTransform.Get().position);
    }

    IEnumerator InfoMessageRoutine()
    {
        yield return new WaitForSeconds(20f);
        _infoAlert.PlayAudio(PlayerTransform.Get().position);
    }
}
