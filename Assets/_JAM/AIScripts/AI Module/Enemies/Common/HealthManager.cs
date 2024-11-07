using System;
using UnityEngine;

namespace JAM.AIModule
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth = 100;
        
        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                OnHealthValueChanged?.Invoke(_currentHealth);
                if (_currentHealth <= 0)
                {
                    OnMinimalHealthReached?.Invoke();
                }
            }
        }

        public event Action OnMinimalHealthReached;
        public event Action<int> OnHealthValueChanged;

        private void Start()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public void Kill()
        {
            TakeDamage(_maxHealth);
        }
    }
}