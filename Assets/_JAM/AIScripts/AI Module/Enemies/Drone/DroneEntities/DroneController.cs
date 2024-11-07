using JAM.AIModule.Drone.States;
using JAM.Patterns.SM;
using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public abstract class DroneController : MonoBehaviour
    {
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] protected DronePhysicsMovement _dronePhysicsMovement;
        [SerializeField] private DroneDestroyer _droneDestroyer;
        
        protected IMovable _movementDriver;
        protected IAttackBehaviour _attackBehaviour;
        private AbstractStateMachine _droneStateMachine;
        protected virtual void Awake()
        {
            _droneStateMachine = new DroneStateMachine();
            
            _movementDriver.InjectAttackBehaviour(_attackBehaviour);
            _droneStateMachine.RegisterState(new PlayerChaseState(_movementDriver));
            _droneStateMachine.RegisterState(new AttackState(_attackBehaviour));
            _droneStateMachine.RegisterState(new DeadState(_droneDestroyer));
            _droneStateMachine.SetState<PlayerChaseState>();
            SubscribeEvents();
        }

        protected virtual void Update()
        {
            _attackBehaviour.UpdateBehaviour();
        }

        protected virtual void FixedUpdate()
        {
            _droneStateMachine.UpdateCurrentState(Time.fixedDeltaTime);
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached += OnMinimalHealthReachedHandler;
        }

        protected virtual void UnsubscribeEvents()
        {
            _healthManager.OnMinimalHealthReached -= OnMinimalHealthReachedHandler;
        }

        private void OnMinimalHealthReachedHandler()
        {
            _droneStateMachine.SetState<DeadState>();
        }
        
        protected void OnTargetLostEventHandler()
        {
            _droneStateMachine.SetState<PlayerChaseState>();
        }
        
        protected void OnTargetChasedEventHandler()
        {
            _droneStateMachine.SetState<AttackState>();
        }

        public void KillDrone() => _droneDestroyer.DestroyDrone();
    }
}