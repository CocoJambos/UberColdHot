using ECM2;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CustomMovementModes
{
    None,
    WallRun,
}

public class FirstPersonCharacterController : Character
{
    [SerializeField] private InputHandler m_InputHandler;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private AnimationCurve m_AccelerationCurve;
    [SerializeField] private float m_WallRunMinAngle = 45f;
    [SerializeField] private float m_WallRunMaxAngle = 110f;
    [SerializeField] private float m_WallRunMinDistance = 0.3f;

    private float m_BaseMaxAcceleration;
    private bool m_WasJumpTriggered;
    
    protected override void Start()
    {
        base.Start();

        Cursor.lockState = CursorLockMode.Locked;
        m_BaseMaxAcceleration = GetMaxAcceleration();
        
        CustomMovementModeUpdated += OnCustomMovementModeUpdated;
    }
    
    protected void OnDestroy()
    {
        CustomMovementModeUpdated -= OnCustomMovementModeUpdated;
    }

    private void Update()
    {
        if(Keyboard.current.leftBracketKey.isPressed)
        {
            groundFriction -= 0.1f;
        }

        if(Keyboard.current.rightBracketKey.isPressed)
        {
            groundFriction += 0.1f;
        }

        if(Keyboard.current.oKey.isPressed)
        {
            brakingDecelerationWalking -= 0.1f;
        }

        if(Keyboard.current.pKey.isPressed)
        {
            brakingDecelerationWalking += 0.1f;
        }
        
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
            
            CheckAndTriggerBlockDisappearing(characterMovement.currentGround.collider);
        }

        if(IsFalling() || IsJumping())
        {
            if(DetectWall(out CollisionResult collisionResult) && CanWallRun(collisionResult.surfaceNormal))
            {
                SetMovementMode(MovementMode.Custom, (int)CustomMovementModes.WallRun);
            }
        }
    }
    
    protected override void OnCollided(ref CollisionResult collisionResult)
    {
        base.OnCollided(ref collisionResult);
        
        CheckAndTriggerBlockDisappearing(collisionResult.collider);
    }
    
    private void OnCustomMovementModeUpdated(float deltaTime)
    {
        CustomMovementModes customMovementMode = (CustomMovementModes)GetCustomMovementMode();

        switch(customMovementMode)
        {
            case CustomMovementModes.WallRun:
                WallRun(deltaTime);
                break;
        }
    }

    private void CheckAndTriggerBlockDisappearing(Collider collider)
    {
        if(collider.CompareTag("BasicBlock") && collider.TryGetComponent(out BlockDisappearing blockDisappearing))
        {
            blockDisappearing.TryToStartDisappearingOnPlayerTouch();
        }
    }

    private bool CanWallRun(Vector3 wallNormal)
    {
        float angle = Vector3.Angle(wallNormal, GetForwardVector());
        return angle >= m_WallRunMinAngle && angle <= m_WallRunMaxAngle;
    }

    private bool DetectWall(out CollisionResult collisionResult)
    {
        collisionResult = default;

        bool leftSweepTest = characterMovement.MovementSweepTest(GetPosition(),
            -GetRightVector(),
            m_WallRunMinDistance,
            out CollisionResult leftCollisionResult);

        bool rightSweepTest = characterMovement.MovementSweepTest(GetPosition(),
            GetRightVector(),
            m_WallRunMinDistance,
            out CollisionResult rightCollisionResult);

        if(!(leftSweepTest || rightSweepTest))
            return false;

        collisionResult = leftSweepTest ? leftCollisionResult : rightCollisionResult;
        
        return true;
    }

    #region Custom Movement Modes

    private void WallRun(float deltaTime)
    {
        
    }

    private bool IsWallRunning()
    {
        return (CustomMovementModes)GetCustomMovementMode() == CustomMovementModes.WallRun;
    }

    #endregion
}
