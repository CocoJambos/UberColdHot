using UnityEngine;

namespace JAM.AIModule.Drone
{
    public interface IMovable
    {
        void UpdateMovement(float deltaTime);
        void InjectAttackBehaviour(IAttackBehaviour attackBehaviour);
    }
}
