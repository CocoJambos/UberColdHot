using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace JAM.AIModule.Drone
{
    public class DroneNavMeshMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private float _speed;
        [SerializeField] private AnimationCurve _speedCurve;
        
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Rigidbody _rigidbody;

        
        public NavMeshAgent _agent;
        
        private NavMeshPath path;
        private int currentPathIndex;
        public float stoppingDistance = 0.5f;
        
        private void Awake()
        {
            path = new NavMeshPath();
        }

        public void StartMovement()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateMovement()
        {
            CalculatePath();
        }

        public void StopMovement()
        {
            throw new System.NotImplementedException();
        }

        public void CalculatePath()
         {
             Debug.Log("CalculatePath");
             NavMesh.CalculatePath(_agent.transform.position, _playerTarget.position, NavMesh.AllAreas, path);
             
             for (int i = 0; i < path.corners.Length - 1; i++)
             {
                 Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, Time.deltaTime);
             }
             
             if(path.corners.Length > 0)
             {
                 MoveToPoint(path.corners[0]);
             }
         }

         private void MoveToPoint(Vector3 point)
         {
             Debug.Log("MoveToPoint " + point);
             var movementVector = point - _agent.transform.position;
             _rigidbody.AddForce(movementVector * _speed);
         }

    }
}