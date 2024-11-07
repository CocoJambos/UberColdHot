using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace JAM.AIModule
{
    public class FpvAttackBehaviour : MonoBehaviour, IAttackBehaviour, ISeekChaser
    {
        [SerializeField] private HealthManager _droneHealthManager;
        [SerializeField] private float _targetChasedMinDistance;
        [SerializeField] private int _damagePerExplosion = 25;
        
        private PlayerHealthManager _playerHealthManager;
        private Transform _target;
        private bool _isTargetChased;

        public Action OnTargetChasedEvent { get; set; }
        
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
            _playerHealthManager = PlayerHealthManager.Instance;
        }

        public void UpdateBehaviour()
        {
            CheckChasingStatus();
        }
        
        public void AttackTarget()
        {
            _playerHealthManager.ApplyDamage(_damagePerExplosion);
            _droneHealthManager.Kill();
        }

        public void StopAttackTarget()
        {
            // Nothing happens because we already dead ha-ha
        }
        
        public Vector3 GetAttackPosition() => _target.position;
        
        public void CheckChasingStatus()
        {
            float distance = CalculateFlatDistanceToTarget();
            if(distance <= _targetChasedMinDistance)
                IsTargetChased = true;
        }
        
        public float CalculateFlatDistanceToTarget()
        {
            Debug.DrawLine(transform.position, _target.position,Color.cyan,Time.deltaTime);
            var distance = Vector3.Distance(transform.position, _target.position);
            return distance;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!Application.isPlaying) return;
            float distance = CalculateFlatDistanceToTarget();
            
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 14;
        
            Vector3 screenPosition = Camera.current.WorldToScreenPoint(transform.position);
            if (screenPosition.z > 0) // Only draw if the object is in front of the camera
            {
                Vector2 labelPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
                UnityEditor.Handles.BeginGUI();
                GUI.Label(new Rect(labelPosition, new Vector2(100, 20)), $"Distance: {distance:F2}", style);
                UnityEditor.Handles.EndGUI();
            }
        }
        
        #endif
    }
}
