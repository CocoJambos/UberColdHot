using JAM.AIModule;
using JAM.AIModule.Drone;
using UnityEngine;

public class FPVDroneController : DroneController
{
    [SerializeField] 
    private FpvAttackBehaviour _fpvAttackBehaviour;
    private ISeekChaser _chasingBehaviour;

    protected override void Awake()
    {
        _attackBehaviour = _fpvAttackBehaviour;
        _movementDriver = _dronePhysicsMovement;
        _chasingBehaviour = _fpvAttackBehaviour;
        base.Awake();
    }
    
    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        _chasingBehaviour.OnTargetChasedEvent += OnTargetChasedEventHandler;
    }
    
    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        _chasingBehaviour.OnTargetChasedEvent -= OnTargetChasedEventHandler;
    }

}
