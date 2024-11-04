using JAM.AIModule.Drone.ExtensionMethods;
using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class AttackTargetFinder : MonoBehaviour
    {
        [SerializeField] private float _minimalAttackDistance;
        
        // TODO: change to search 
        public Transform _target;

        private bool _isProperAttackDistance;

        private bool IsProperAttackDistance
        {
            get => _isProperAttackDistance;
            set
            {
                if (_isProperAttackDistance == value) return;
                _isProperAttackDistance = value;
                (_isProperAttackDistance? OnTargetChasedEvent : OnTargetFoundEvent)?.Invoke();
            }
        }

        public Action OnTargetFoundEvent;
        public Action OnTargetChasedEvent;
        
        private void Awake()
        {
           // target = FindObjectsByType<Player>();
        }

        private void Update()
        {
            IsProperAttackDistance = CheckDistanceCondition();
        }

        private bool CheckDistanceCondition()
        {
            return CalculateDistanceToPlayer() <= _minimalAttackDistance;
        }
        
        private float CalculateDistanceToPlayer()
        {
            Debug.DrawLine(transform.position.FlattenVector(), _target.position.FlattenVector(),Color.cyan,Time.deltaTime);
            var distance = Vector3.Distance(transform.position.FlattenVector(), _target.position.FlattenVector());
         //   Debug.Log("AttackDiatance: " + distance);
            return distance;
        }
    }
}
