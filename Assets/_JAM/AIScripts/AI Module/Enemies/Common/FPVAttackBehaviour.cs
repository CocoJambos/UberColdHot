using System;
using UnityEngine;

namespace JAM.AIModule
{
    public class FpvAttackBehaviour : MonoBehaviour, IAttackBehaviour, IChaser
    {
        [SerializeField] private float _targetChasedMinDistance;
        [SerializeField] private HealthManager _healthManager;
        
        private Transform _target;
        private bool _isTargetChased;

        public Action OnTargetChasedEvent { get; set; }
        public Action OnTargetLostEvent { get; set; }
        
        private bool IsTargetChased
        {
            set
            {
                if (_isTargetChased == value) return;
                _isTargetChased = value;
                if(_isTargetChased)
                    OnTargetChasedEvent?.Invoke();
            }
        }
  
        private void Start()
        {
            _target = PlayerTransform.Get();
        }

        public void UpdateBehaviour()
        {
            CheckChaseCondition();
        }
        
        public void AttackTarget()
        {
            Debug.Log("BAM. Self Destroy");
            _healthManager.Kill();
        }

        public void StopAttackTarget()
        {
            // Nothing happens because we already dead ha-ha
        }
        
        public Vector3 GetTargetPosition()
        {
            return _target.position;
        }
        
        public void CheckChaseCondition()
        {
            float distance = CalculateDistanceToTarget();
            if(distance <= _targetChasedMinDistance)
                IsTargetChased = true;
        }
        
        public float CalculateDistanceToTarget()
        {
            Debug.DrawLine(transform.position, _target.position,Color.cyan,Time.deltaTime);
            var distance = Vector3.Distance(transform.position, _target.position);
            return distance;
        }
    }
}
