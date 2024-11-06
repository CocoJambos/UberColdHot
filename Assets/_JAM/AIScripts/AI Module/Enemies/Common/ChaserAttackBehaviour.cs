using System;
using System.Collections;
using UnityEngine;
using JAM.AIModule.Drone.ExtensionMethods;

namespace JAM.AIModule
{
    public class ChaserAttackBehaviour : MonoBehaviour, IAttackBehaviour, IChaser
    {
        [SerializeField] private float _targetChasedMinDistance;
        [SerializeField] private float _targetLooseMinDistance;
        
        [SerializeField] private float _attackDelay = 2f;
        [SerializeField] private Transform _droneBodyTransform;
        
        private Transform _target;
        private Coroutine _attackRoutine;
        private bool _isAttacking;
        private bool _isTargetChased;
        private WaitForSeconds _waitForSecondsBetweenAttacks;
        
        private bool IsTargetChased
        {
            set
            {
                if (_isTargetChased == value) return;
                _isTargetChased = value;
                (_isTargetChased? OnTargetChasedEvent : OnTargetLostEvent)?.Invoke();
            }
        }

        public Action OnTargetChasedEvent { get; set; }
        public Action OnTargetLostEvent { get; set; }
        
        private void Start()
        {
            _target = PlayerTransform.Get();
            _waitForSecondsBetweenAttacks = new WaitForSeconds(_attackDelay);
        }

        public void UpdateBehaviour()
        {
            CheckChaseCondition();
        }

        public void AttackTarget()
        {
            _isAttacking = true;
            _attackRoutine = StartCoroutine(AttackCycleRoutine());
        }

        public void StopAttackTarget()
        {
            _isAttacking = false;
            StopCoroutine(_attackRoutine);
        }

        public Vector3 GetTargetPosition()
        {
            Vector3 playerPos = _target.position;
            playerPos.y = _droneBodyTransform.position.y;
            return playerPos;
        }

        private IEnumerator AttackCycleRoutine()
        {
            while(_isAttacking)
            {
                AttakTargetRoutine();
                yield return _waitForSecondsBetweenAttacks;
            }
        }

        private void AttakTargetRoutine()
        {
            Debug.Log("Paw! Attack Player");
        }
        
        public void CheckChaseCondition()
        {
            float distance = CalculateDistanceToTarget();
            if(distance <= _targetChasedMinDistance)
                IsTargetChased = true;
            else if(distance >= _targetLooseMinDistance)
                IsTargetChased = false;
        }
        
        public float CalculateDistanceToTarget()
        {
            Debug.DrawLine(transform.position, _target.position,Color.cyan,Time.deltaTime);
            var distance = Vector3.Distance(transform.position.FlattenVector(), _target.position.FlattenVector());
            return distance;
        }
    }
}