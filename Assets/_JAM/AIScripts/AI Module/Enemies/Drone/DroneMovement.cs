using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class DroneMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private AnimationCurve _dragCurve;
        
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _bodyObject;
        
        
        
        public float timeScale = 1f;
        
        //  ConstantForce
        public void UpdateMovement()
        {
            Debug.Log("UpdateMovement");

            Vector3 direction = _playerTarget.position - _bodyObject.position;
            _rigidbody.AddForce(direction * _speed);
            UpdateDrag();
        }

        private void UpdateDrag()
        {
            float distance = Vector3.Distance(_playerTarget.position, _bodyObject.position);
            _rigidbody.linearDamping = _dragCurve.Evaluate(Mathf.InverseLerp(10f, 0f, distance));

        }
    }
}
