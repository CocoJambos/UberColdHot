using System;
using System.Collections;
using UnityEngine;

public class VOController : SingleBehaviour<VOController>
{
    public AudioInvoker _initAlert;
    public AudioInvoker _infoAlert;

    public AudioInvoker _playerDeathAlert;
    public AudioInvoker _gateOpenAlert;
    
    private void Start()
    {
        _initAlert.PlayAudio();
        StartCoroutine(InfoMessageRoutine());
    }
    
    public void PlayPlayerDeathMessage()
    {
        _playerDeathAlert.PlayAudio();
    }
    
    public void PlayGateOpenMessage()
    {
        _gateOpenAlert.PlayAudio();
    }

    IEnumerator InfoMessageRoutine()
    {
        yield return new WaitForSeconds(20f);
        _infoAlert.PlayAudio();
    }
}
