using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class ObstacleAvoider : MonoBehaviour
    {
        [SerializeField]
        private Transform _bodyTransform;

        [SerializeField] 
        private LayerMask _collisionMask;
        
        public bool IsObstacleInPath(Vector3 direction)
        {
            RaycastHit hit;
            if (Physics.Raycast(_bodyTransform.position, direction, out hit, 3f, _collisionMask))
            {
                bool isObstacle = hit.collider != null && !hit.collider.isTrigger;
                return isObstacle;
            }
            return false;
        }

        public Vector3 GetAvoidanceDirection(Vector3 directionToTarget)
        {
            Vector3 left = Vector3.Cross(Vector3.up, directionToTarget).normalized;
            Vector3 right = -left;
            Vector3 up = Vector3.up;
            Vector3 down = Vector3.down;

            if(!IsObstacleInPath(up)) return up;
            if(!IsObstacleInPath(down)) return down;

            if(!IsObstacleInPath(left)) return left;
            if(!IsObstacleInPath(right)) return right;

            return directionToTarget;
        }
    }
}