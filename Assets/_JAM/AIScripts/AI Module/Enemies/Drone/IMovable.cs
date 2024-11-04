using UnityEngine;

namespace JAM.AIModule.Drone
{
    public interface IMovable
    {
        void StartMovement();

        void UpdateMovement();
        void StopMovement();
    }
}
