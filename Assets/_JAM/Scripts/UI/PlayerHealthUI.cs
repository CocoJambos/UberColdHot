using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private Text _healthTextElement;

    [SerializeField]
    private GameObject _deathScreen;

    private PlayerHealthManager _playerHealthManager;

    private const string Fraze = "HEALTH: ";

    void Start()
    {
        _playerHealthManager = PlayerHealthManager.Instance;
        _playerHealthManager.OnHealthValueChanged += PlayerHealthManagerOnOnHealthValueChanged;
        _playerHealthManager.OnPlayerDiedEvent += PlayerHealthManagerOnOnPlayerDiedEvent;
        SetUIValue(_playerHealthManager.CurrentHealth);
    }

    private void PlayerHealthManagerOnOnPlayerDiedEvent()
    {
        _deathScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        _playerHealthManager.OnHealthValueChanged -= PlayerHealthManagerOnOnHealthValueChanged;
        _playerHealthManager.OnPlayerDiedEvent -= PlayerHealthManagerOnOnPlayerDiedEvent;
    }

    private void PlayerHealthManagerOnOnHealthValueChanged(int healthValue)
    {
        SetUIValue(healthValue);
    }

    private void SetUIValue(int healthValue)
    {
        _healthTextElement.text = Fraze + healthValue;
    }
}