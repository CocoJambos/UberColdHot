using ECM2;
using System;
using UnityEngine;

public class FirstPersonCharacterController : Character
{
    [SerializeField] private InputHandler m_InputHandler;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private AnimationCurve m_AccelerationCurve;

    private float m_BaseMaxAcceleration;
    private bool m_WasJumpTriggered;
    
    protected override void Start()
    {
        base.Start();

        Cursor.lockState = CursorLockMode.Locked;
        m_BaseMaxAcceleration = GetMaxAcceleration();
    }

    private void Update()
    {
        Vector2 movementInput = m_InputHandler.InputMovementContext.MovementInput;
        Vector3 movementDirection = new(movementInput.x, 0, movementInput.y);
        movementDirection = m_CameraController.RelativeToCamera(movementDirection);
        SetMovementDirection(movementDirection);
        
        Vector2 mouseInput = m_InputHandler.InputMouseContext.MouseDeltaInput;
        m_CameraController.AddCameraInput(-mouseInput.y);
        AddYawInput(mouseInput.x);

        bool isJumpPressed = m_InputHandler.InputMovementContext.IsJumpPressedInput;

        if(isJumpPressed && CanJump())
        {
            if(!m_WasJumpTriggered)
            {
                Jump();
                m_WasJumpTriggered = true;
            }
        }
        else
        {
            StopJumping();
        }
    }
    
    protected override void OnBeforeSimulationUpdate(float deltaTime)
    { 
        base.OnBeforeSimulationUpdate(deltaTime);

        if(IsWalking())
        {
            if(m_WasJumpTriggered && !m_InputHandler.InputMovementContext.IsJumpPressedInput)
            {
                m_WasJumpTriggered = false;
            }

            float normalizedSpeed = GetSpeed() / GetMaxSpeed();
            float accelerationFactor = m_AccelerationCurve.Evaluate(normalizedSpeed);
            maxAcceleration = m_BaseMaxAcceleration * accelerationFactor;
        }
    }
}
