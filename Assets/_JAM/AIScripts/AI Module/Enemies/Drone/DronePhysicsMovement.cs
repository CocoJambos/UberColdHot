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
        
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _bodyObject;

        public float originalDrag = 1f;
        public float finishDrag = 5f;
        
        public float timeScale = 1f;
        
        //  ConstantForce
        public void StartMovement()
        {
            _rigidbody.linearDamping = originalDrag;
        }

        public void UpdateMovement()
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
            _rigidbody.AddForce(movementVector * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }

        private Vector3 CalculateMovementVector()
        {
            Vector3 playerPos = GetFixedDroneTargetPos();
            Debug.DrawLine(playerPos,_bodyObject.transform.position,Color.blue,Time.deltaTime);
            return playerPos - _bodyObject.position;
        }
        
        private void UpdateDrag()
        {
            float distance = Vector3.Distance(_playerTarget.position.FlattenVector(), _bodyObject.position.FlattenVector());
            _rigidbody.linearDamping = _dragCurve.Evaluate(Mathf.InverseLerp(10f, 0f, distance));
        }

        private Vector3 GetFixedDroneTargetPos()
        {
            Vector3 playerPos = _playerTarget.position;
            playerPos.y = _droneHeight;
            return playerPos;
        }

        public void StopMovement()
        {
            _rigidbody.linearDamping = finishDrag;
        }
    }
}
