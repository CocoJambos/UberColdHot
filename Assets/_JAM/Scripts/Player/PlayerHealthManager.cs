using JAM.AIModule;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealthManager : SingleBehaviour<PlayerHealthManager>
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private FirstPersonCharacterController _characterController;
    [SerializeField] private AudioInvoker[] _hitSoundInvoker;
    [SerializeField] private AudioInvoker _deathSoundInvoker;
    
    public event Action OnPlayerDiedEvent;
    public event Action<int> OnHealthValueChanged;

    public int CurrentHealth => _healthManager.CurrentHealth;

    protected override void Awake()
    {
        base.Awake();
        _healthManager.OnMinimalHealthReached += HealthManagerOnOnMinimalHealthReached;
        _healthManager.OnHealthValueChanged += HealthManagerOnOnHealthValueChanged;
        _healthManager.OnEntityDamaged += HealthManagerOnOnEntityDamaged;
    }

    private void HealthManagerOnOnEntityDamaged()
    {
        if(CurrentHealth <= 0) {return;}
        _hitSoundInvoker[Random.Range(0,_hitSoundInvoker.Length)].PlayAudio();
    }

    private void HealthManagerOnOnHealthValueChanged(int health)
    {
        OnHealthValueChanged?.Invoke(health);
    }

    void OnDestroy()
    {
        _healthManager.OnMinimalHealthReached -= HealthManagerOnOnMinimalHealthReached;
        _healthManager.OnHealthValueChanged -= HealthManagerOnOnHealthValueChanged;
        _healthManager.OnEntityDamaged -= HealthManagerOnOnEntityDamaged;
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
        _deathSoundInvoker.PlayAudio();

        OnPlayerDiedEvent?.Invoke();
        _characterController.enabled = false;
    }
}
