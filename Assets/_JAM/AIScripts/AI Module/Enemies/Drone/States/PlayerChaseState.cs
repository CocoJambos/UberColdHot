using JAM.Patterns.SM;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class PlayerChaseState : IState
    {
        private DroneMovement _droneMovement;
        
        public PlayerChaseState(DroneMovement droneMovement)
        {
            _droneMovement = droneMovement;
        }

        public void EnterState()
        {
        }

        public void UpdateState()
        {
            Debug.Log("PlayerChaseState");
            _droneMovement.UpdateMovement();
        }

        public void ExitState()
        {
        }
    }
}
