using UnityEngine;
using UnityEngine.Events;

public class PlayerCollidedEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onPlayerCollided;

    public void OnPlayerCollided()
    {
        onPlayerCollided?.Invoke();
    }
}
