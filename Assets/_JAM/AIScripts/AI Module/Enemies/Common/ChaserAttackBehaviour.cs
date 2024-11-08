using System;
using System.Collections;
using UnityEngine;
using JAM.AIModule.Drone.ExtensionMethods;
using Random = UnityEngine.Random;

namespace JAM.AIModule
{
    public class ChaserAttackBehaviour : MonoBehaviour, IAttackBehaviour, ISeekAndLoseChaser
    {
        [SerializeField] private Transform _droneBodyTransform;
        
        [SerializeField] private float _targetChasedMinDistance;
        [SerializeField] private float _targetLooseMinDistance;
        [SerializeField] private Vector2 _chasingFlyHeightRange;
        [SerializeField] private float _attackDelay = 2f;
        [SerializeField] private int _damagePerHit = 10;
        [SerializeField] private AudioInvoker _attackAudioInvoker;
        
        private PlayerHealthManager _playerHealthManager;
        private TimeManager _timeManager;
        private Transform _target;
        private Coroutine _attackRoutine;
        private bool _isAttacking;
        private bool _isTargetChased;
        private float _chasingFlyHeight;
        
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
            _timeManager = TimeManager.Instance;
            _playerHealthManager = PlayerHealthManager.Instance;
            _chasingFlyHeight = CalculateFlyingHeight();
            _target = PlayerTransform.Get();
        }

        public void UpdateBehaviour() => CheckChasingStatus();

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

        public Vector3 GetAttackPosition()
        {
            Vector3 playerPos = _target.position;
            playerPos.y = _chasingFlyHeight;
            return playerPos;
        }
        
        public void CheckChasingStatus()
        {
            float distance = CalculateFlatDistanceToTarget();
            if(distance <= _targetChasedMinDistance)
                IsTargetChased = true;
            else if(distance >= _targetLooseMinDistance)
                IsTargetChased = false;
        }
        
        /// <summary>
        /// Calculates the distance ignoring the height value of drone
        /// </summary>
        /// <returns></returns>
        public float CalculateFlatDistanceToTarget()
        {
            var distance = Vector3.Distance(transform.position.FlattenVector(), _target.position.FlattenVector());
            return distance;
        }
        
        private IEnumerator AttackCycleRoutine()
        {
            while(_isAttacking)
            {
                AttackTargetRoutine();
                yield return StartCoroutine(AttackWaiter());
            }
        }

        private IEnumerator AttackWaiter()
        {
            float delay = _attackDelay / _timeManager.TimeScale;
            while(delay > 0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }
        }

        private void AttackTargetRoutine()
        {
            _playerHealthManager.ApplyDamage(_damagePerHit);
            _attackAudioInvoker.PlayAudio();
        }

        private float CalculateFlyingHeight() => Random.Range(_chasingFlyHeightRange.x, _chasingFlyHeightRange.y);
    }
}