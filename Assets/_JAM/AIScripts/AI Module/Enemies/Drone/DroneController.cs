using JAM.AIModule.Drone.States;
using JAM.Patterns.SM;
using UnityEngine;
using UnityEngine.Serialization;

namespace JAM.AIModule.Drone
{
    public class DroneController : MonoBehaviour
    {
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] private AttackBehaviour _attackBehaviour;
        [SerializeField] private AttackTargetFinder _targetFinder;
        
        [SerializeField] private DronePhysicsMovement _dronePhysicsMovement;
        [SerializeField] private DroneNavMeshMovement _droneNavMeshMovement;

        private IMovable _movementDriver;
        private AbstractStateMachine _droneStateMachine;

        private void Awake()
        {
            _droneStateMachine = new DroneStateMachine();

            _movementDriver = _dronePhysicsMovement;
           // _movementDriver = _droneNavMeshMovement;
            
            _droneStateMachine.RegisterState(new PlayerChaseState(_movementDriver));
            _droneStateMachine.RegisterState(new AttackState(_attackBehaviour));
            _droneStateMachine.RegisterState(new DeadState());
            _droneStateMachine.SetState<PlayerChaseState>();
            SubscribeEvents();
        }

        private void Update()
        {
            _droneStateMachine.UpdateCurrentState();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached += OnMinimalHealthReachedHandler;
            _targetFinder.OnTargetChasedEvent += OnTargetChasedEventHandler;
            _targetFinder.OnTargetFoundEvent += OnTargetFoundEventHandler;
        }

        private void UnsubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached -= OnMinimalHealthReachedHandler;
            _targetFinder.OnTargetChasedEvent -= OnTargetChasedEventHandler;
            _targetFinder.OnTargetFoundEvent -= OnTargetFoundEventHandler;
        }

        private void OnMinimalHealthReachedHandler()
        {
            _droneStateMachine.SetState<DeadState>();
        }
        
        private void OnTargetChasedEventHandler()
        {
            _droneStateMachine.SetState<AttackState>();
        }
        
        private void OnTargetFoundEventHandler()
        {
            _droneStateMachine.SetState<PlayerChaseState>();
        }
    }
}