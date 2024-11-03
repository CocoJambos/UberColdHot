using System;
using UnityEngine;

namespace JAM.AIModule
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth = 100;
        
        private int _currentHealth;

        public Action OnMinimalHealthReached;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                OnMinimalHealthReached?.Invoke();
            }
        }

        public void Kill()
        {
            TakeDamage(_maxHealth);
        }
    }
}