using JAM.Patterns.SM;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class PlayerChaseState : IState
    {
        private IMovable _movementDriver;
        
        public PlayerChaseState(IMovable droneMovement)
        {
            _movementDriver = droneMovement;
        }

        public void EnterState()
        {
            _movementDriver.StartMovement();
        }

        public void UpdateState()
        {
            _movementDriver.UpdateMovement();
        }

        public void ExitState()
        {
            _movementDriver.StopMovement();
        }
    }
}
