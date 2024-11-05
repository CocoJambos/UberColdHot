using JAM.AIModule.Drone.ExtensionMethods;
using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class AttackTargetFinder : MonoBehaviour
    {
        [SerializeField] private float _targetLockDistance;
        [SerializeField] private float _targetLooseDistance;
        
        private Transform _target;
        private bool _isTargetLocked;

        private bool IsTargetLockedDistance
        {
            set
            {
                if (_isTargetLocked == value) return;
                _isTargetLocked = value;
                (_isTargetLocked? OnTargetLostEvent : OnTargetLockedEvent)?.Invoke();
            }
        }

        public Action OnTargetLockedEvent;
        public Action OnTargetLostEvent;
        
        private void Start()
        {
            _target = PlayerTransform.Get();
        }

        private void Update() => CheckDistanceCondition();

        private void CheckDistanceCondition()
        {
            float distance = CalculateDistanceToPlayer();
            if(distance <= _targetLockDistance)
                IsTargetLockedDistance = true;
            else if(distance >= _targetLooseDistance)
                IsTargetLockedDistance = false;
        }
        
        private float CalculateDistanceToPlayer()
        {
            Debug.DrawLine(transform.position.FlattenVector(), _target.position.FlattenVector(),Color.cyan,Time.deltaTime);
            var distance = Vector3.Distance(transform.position.FlattenVector(), _target.position.FlattenVector());
            return distance;
        }
    }
}
