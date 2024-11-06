using UnityEngine;

namespace JAM.AIModule.Drone
{
    public interface IMovable
    {
        void StartMovement();

        void UpdateMovement(float deltaTime);
        void StopMovement();

        void InjectAttackBehaviour(IAttackBehaviour attackBehaviour);
    }
}
