using JAM.Patterns.SM;

namespace JAM.AIModule.Drone
{
    public class DeadState : IState
    {
        private DroneDestroyer _destroyer;

        public DeadState(DroneDestroyer droneDestroyer)
        {
            _destroyer = droneDestroyer;
        }
        
        public void EnterState()
        {
            _destroyer.DestroyDrone();
        }

        public void UpdateState(float deltaTime)
        {
            
        }
        
        public void ExitState()
        {
        }
    }
}
