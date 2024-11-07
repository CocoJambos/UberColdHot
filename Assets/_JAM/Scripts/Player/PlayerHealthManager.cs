using JAM.AIModule;
using System;
using UnityEngine;

public class PlayerHealthManager : SingleBehaviour<PlayerHealthManager>
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private FirstPersonCharacterController _characterController;

    public event Action OnPlayerDiedEvent;
    public event Action<int> OnHealthValueChanged;

    public int CurrentHealth => _healthManager.CurrentHealth;

    protected override void Awake()
    {
        base.Awake();
        _healthManager.OnMinimalHealthReached += HealthManagerOnOnMinimalHealthReached;
        _healthManager.OnHealthValueChanged += HealthManagerOnOnHealthValueChanged;
    }

    private void HealthManagerOnOnHealthValueChanged(int health)
    {
        OnHealthValueChanged?.Invoke(health);
    }

    void OnDestroy()
    {
        _healthManager.OnMinimalHealthReached -= HealthManagerOnOnMinimalHealthReached;
        _healthManager.OnHealthValueChanged -= HealthManagerOnOnHealthValueChanged;
    }

    public void ApplyDamage(int damage)
    {
        _healthManager.TakeDamage(damage);
    }

    public void KillPlayer()
    {
        _healthManager.Kill();
    }
    
    private void HealthManagerOnOnMinimalHealthReached()
    {
        OnPlayerDiedEvent?.Invoke();
        _characterController.enabled = false;
    }
}
