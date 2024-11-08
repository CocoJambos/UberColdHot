using System;
using UnityEngine;

namespace JAM.AIModule
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth = 100;

        private int _currentHealth;
        private bool IsDead => _currentHealth <= 0;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                OnHealthValueChanged?.Invoke(_currentHealth);
                if(_currentHealth <= 0)
                {
                    OnMinimalHealthReached?.Invoke();
                }
            }
        }

        public event Action OnMinimalHealthReached;
        public event Action<int> OnHealthValueChanged;
        public event Action OnEntityDamaged;

        private void Start()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if(IsDead)
            { return; }
            CurrentHealth -= damage;
            OnEntityDamaged?.Invoke();
        }

        public void Kill()
        {
            TakeDamage(_maxHealth);
        }
    }
}