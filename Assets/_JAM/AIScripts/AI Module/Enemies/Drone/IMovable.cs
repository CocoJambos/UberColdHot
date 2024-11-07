using UnityEngine;

namespace JAM.AIModule.Drone
{
    public interface IMovable
    {
        void StartMovement();
        void StopMovement();
        void UpdateMovement(float deltaTime);
        void InjectAttackBehaviour(IAttackBehaviour attackBehaviour);
    }
}
