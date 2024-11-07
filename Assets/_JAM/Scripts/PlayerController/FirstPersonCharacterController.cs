using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCharacterController : Character
{
    [SerializeField] private InputHandler m_InputHandler;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private AnimationCurve m_AccelerationCurve;

    [Header("Custom Movement Modes")]
    [SerializeField] private SlidingMovement slidingMovement;

    private bool CanSlide => IsWalking();

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

        bool isSlidePressed = m_InputHandler.InputMovementContext.IsSlidePressedInput;
        if(isSlidePressed && CanSlide && !IsSliding())
        {
            SetMovementMode(MovementMode.Custom, (int)ECustomMovementMode.Sliding);
        }
        else if(!isSlidePressed && IsSliding())
        {
            SetMovementMode(MovementMode.Walking);
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
    }

    public void CheckAndTriggerBlockDisappearing(Collider collider)
    {
        if(collider.CompareTag(Tags.BasicBlock.ToString()) && collider.TryGetComponent(out BlockDisappearing blockDisappearing))
        {
            blockDisappearing.TryToStartDisappearingOnPlayerTouch();
        }
    }

    protected override void OnCollided(ref CollisionResult collisionResult)
    {
        base.OnCollided(ref collisionResult);

        CheckAndTriggerBlockDisappearing(collisionResult.collider);
    }
}
