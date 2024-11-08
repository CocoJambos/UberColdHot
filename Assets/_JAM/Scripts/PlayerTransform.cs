using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    private static Transform _playerTransform = null;

    public static Transform Get()
    {
        if(_playerTransform == null)
            _playerTransform = FindAnyObjectByType<FirstPersonCharacterController>().transform;

        return _playerTransform;
    }
}
