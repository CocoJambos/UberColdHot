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
        }

        public void UpdateState(float deltaTime)
        {
            _movementDriver.UpdateMovement(deltaTime);
        }

        public void ExitState()
        {
        }
    }
}
