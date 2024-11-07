using JAM.AIModule;
using JAM.AIModule.Drone;
using UnityEngine;

public class ChaserDroneController : DroneController
{
    [SerializeField] 
    private ChaserAttackBehaviour _chaserAttackBehaviour;
    private ISeekAndLoseChaser _chasingBehaviour;

    protected override void Awake()
    {
        _attackBehaviour = _chaserAttackBehaviour;
        _movementDriver = _dronePhysicsMovement;
        _chasingBehaviour = _chaserAttackBehaviour;
        base.Awake();
    }
    
    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        _chasingBehaviour.OnTargetChasedEvent += OnTargetChasedEventHandler;
        _chasingBehaviour.OnTargetLostEvent += OnTargetLostEventHandler;

    }
    
    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        _chasingBehaviour.OnTargetChasedEvent -= OnTargetChasedEventHandler;
        _chasingBehaviour.OnTargetLostEvent -= OnTargetLostEventHandler;
    }
}
