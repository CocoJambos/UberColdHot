using JAM.Patterns.SM;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

namespace JAM.AIModule.Drone.States
{
    public class AttackState : IState
    {
        private AttackBehaviour _attackBehaviour;
        private DroneMovement _droneMovement;

        // private TaskCancelationTocken _attackRoutine;
    
        public AttackState(AttackBehaviour attackBehaviour, DroneMovement droneMovement)
        {
            _attackBehaviour = attackBehaviour;
            _droneMovement = droneMovement;
        }

        public void EnterState()
        {
            _attackBehaviour.AttackPlayer();
        }

        public void UpdateState()
        {
            
        }

        public void ExitState()
        {
        }
        
    }
}
