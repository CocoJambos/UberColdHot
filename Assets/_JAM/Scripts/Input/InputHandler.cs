using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementContext
{
    public Vector2 MovementInput { get; private set; }
    public void SetMovementInput(Vector2 newInput) => MovementInput = newInput;
    
    public bool IsJumpPressedInput { get; private set; }
    public void SetJumpIsPressedInput(bool newInput) => IsJumpPressedInput = newInput;
}

public class MouseContext
{
    public Vector2 MouseDeltaInput { get; private set; }
    public void SetMouseDeltaInput(Vector2 newInput) => MouseDeltaInput = newInput;
}

[CreateAssetMenu(menuName = "GameJamScriptable", fileName = "InputHandler")]
public class InputHandler : ScriptableObject, GameMappings.IGameplayActions
{
    private GameMappings m_MappingsInstance = null;
    public MouseContext InputMouseContext { get; private set; }
    public MovementContext InputMovementContext { get; private set; }
    
    private void OnEnable()
    {
        if(m_MappingsInstance == null)
            m_MappingsInstance = new();

        InputMouseContext = new();
        InputMovementContext = new();
        
        m_MappingsInstance.Gameplay.SetCallbacks(this);
        m_MappingsInstance.Gameplay.Enable();
    }

    private void OnDisable()
    {
        m_MappingsInstance.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log("MovementInput");
    }

    public void OnMouseMovement(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Performed)
        {
            return;
        }
        
        Debug.Log("MouseInput");
    }
}
