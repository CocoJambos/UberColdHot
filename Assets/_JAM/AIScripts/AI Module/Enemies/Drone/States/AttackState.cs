using JAM.Patterns.SM;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

namespace JAM.AIModule.Drone.States
{
    public class AttackState : IState
    {
        private AttackBehaviour _attackBehaviour;
        
        public AttackState(AttackBehaviour attackBehaviour)
        {
            _attackBehaviour = attackBehaviour;
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
