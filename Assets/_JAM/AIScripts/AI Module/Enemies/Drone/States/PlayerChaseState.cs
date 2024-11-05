using JAM.Patterns.SM;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class PlayerChaseState : IState
    {
        private IMovable _movementDriver;
        private IAttackBehaviour _attackBehaviour;
        
        public PlayerChaseState(IMovable droneMovement, IAttackBehaviour attackBehaviour)
        {
            _movementDriver = droneMovement;
            _attackBehaviour = attackBehaviour;
            _movementDriver.InjectAttackBehaviour(_attackBehaviour);
        }

        public void EnterState()
        {
            _movementDriver.StartMovement();
        }

        public void UpdateState()
        {
            _movementDriver.UpdateMovement();
            _attackBehaviour.UpdateBehaviour();
        }

        public void ExitState()
        {
            _movementDriver.StopMovement();
        }
    }
}
