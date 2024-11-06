using JAM.AIModule.Drone.States;
using JAM.Patterns.SM;
using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class DroneController : MonoBehaviour
    {
        //TEMP
        public enum DroneType
        {
            Chaser,
            Kamikaze
        }
        public DroneType droneType;
        
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] private ChaserAttackBehaviour _chaserAttackBehaviour;
        [SerializeField] private FpvAttackBehaviour _fpvAttackBehaviour;
        [SerializeField] private DronePhysicsMovement _dronePhysicsMovement;
        [SerializeField] private DroneDestroyer _droneDestroyer;
        
        private IMovable _movementDriver;
        private IAttackBehaviour _attackBehaviour;
        private AbstractStateMachine _droneStateMachine;

        private void Awake()
        {
            _droneStateMachine = new DroneStateMachine();

            _movementDriver = _dronePhysicsMovement;

            // TEMP
            if(droneType == DroneType.Chaser)
            {
                _attackBehaviour = _chaserAttackBehaviour;
            }
            else if(droneType == DroneType.Kamikaze)
            {
                _attackBehaviour = _fpvAttackBehaviour;
            }
            _movementDriver.InjectAttackBehaviour(_attackBehaviour);

            _droneStateMachine.RegisterState(new PlayerChaseState(_movementDriver));
            _droneStateMachine.RegisterState(new AttackState(_attackBehaviour));
            _droneStateMachine.RegisterState(new DeadState(_droneDestroyer));
            _droneStateMachine.SetState<PlayerChaseState>();
            SubscribeEvents();
        }

        private void Update()
        {
            _attackBehaviour.UpdateBehaviour();
        }

        private void FixedUpdate()
        {
            _droneStateMachine.UpdateCurrentState(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached += OnMinimalHealthReachedHandler;
            _attackBehaviour.OnTargetLostEvent += OnTargetLostEventHandler;
            _attackBehaviour.OnTargetChasedEvent += OnTargetChasedEventHandler;
        }

        private void UnsubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached -= OnMinimalHealthReachedHandler;
            _attackBehaviour.OnTargetLostEvent -= OnTargetLostEventHandler;
            _attackBehaviour.OnTargetChasedEvent -= OnTargetChasedEventHandler;
        }

        private void OnMinimalHealthReachedHandler()
        {
            _droneStateMachine.SetState<DeadState>();
        }
        
        private void OnTargetLostEventHandler()
        {
            _droneStateMachine.SetState<PlayerChaseState>();
        }
        
        private void OnTargetChasedEventHandler()
        {
            _droneStateMachine.SetState<AttackState>();
        }
    }
}