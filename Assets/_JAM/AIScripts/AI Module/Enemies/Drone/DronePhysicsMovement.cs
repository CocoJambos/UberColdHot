using System;
using UnityEngine;
using UnityEngine.AI;
using JAM.AIModule.Drone.ExtensionMethods;

namespace JAM.AIModule.Drone
{
    public class DronePhysicsMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private ObstacleAvoider _obstacleAvoider;
        
        [SerializeField] private float _speed;
        [SerializeField] private float _droneHeight = 10f;
        [SerializeField] private AnimationCurve _dragCurve;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _bodyObject;

        private IAttackBehaviour _attackBehaviour;
        private Transform _playerTarget;

        public float originalDrag = 1f;
        public float finishDrag = 5f;
        
        private void Start()
        {
            _playerTarget = PlayerTransform.Get();
        }

        public void InjectAttackBehaviour(IAttackBehaviour attackBehaviour)
        {
            _attackBehaviour = attackBehaviour;
        }
        
        public void StartMovement()
        {
            _rigidbody.linearDamping = originalDrag;
        }

        public void UpdateMovement(float _deltaTime)
        {
            var moveVector = CalculateMovementVector();
            if(_obstacleAvoider.IsObstacleInPath(moveVector))
            {
                moveVector = _obstacleAvoider.GetAvoidanceDirection(moveVector);
            }
            MovementRoutine(moveVector);
            //UpdateDrag();
        }
        
        private void MovementRoutine(Vector3 movementVector)
        {
            _rigidbody.AddForce(movementVector * _speed, ForceMode.VelocityChange);
            Quaternion targetRotation = Quaternion.LookRotation((_playerTarget.position - transform.position));
            _rigidbody.MoveRotation(targetRotation);
        }

        private Vector3 CalculateMovementVector()
        {
           Vector3 playerPos = _attackBehaviour.GetTargetPosition();
           return (playerPos - _bodyObject.position).normalized;
        }
        
        private void UpdateDrag()
        {
            float distance = Vector3.Distance(_playerTarget.position.FlattenVector(), _bodyObject.position.FlattenVector());
            _rigidbody.linearDamping = _dragCurve.Evaluate(Mathf.InverseLerp(10f, 0f, distance));
        }

        public void StopMovement()
        {
            _rigidbody.linearDamping = finishDrag;
        }
    }
}
