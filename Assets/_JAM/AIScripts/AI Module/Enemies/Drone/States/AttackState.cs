using JAM.Patterns.SM;

namespace JAM.AIModule.Drone.States
{
    public class AttackState : IState
    {
        private IAttackBehaviour _attackBehaviour;
        
        public AttackState(IAttackBehaviour attackBehaviour)
        {
            _attackBehaviour = attackBehaviour;
        }

        public void EnterState()
        {
            _attackBehaviour.AttackTarget();
        }

        public void UpdateState(float deltaTime)
        {
        }

        public void ExitState()
        {
            _attackBehaviour.StopAttackTarget();
        }
    }
}
