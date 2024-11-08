using ECM2;
using JAM.AIModule.Drone;
using UnityEngine;

public enum CustomMovementModes
{
    None,
    WallRun,
    Sliding,
}

public class FirstPersonCharacterController : Character
{
    [SerializeField] private InputHandler m_InputHandler;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Range(0f, 1f)] private float mouseSensitivity = 0.1f;

    [Header("Wall running")]
    [SerializeField] private float m_WallRunMinAngle = 45f;
    [SerializeField] private float m_WallRunMaxAngle = 110f;
    [SerializeField] private float m_WallRunMinDistance = 0.3f;
    [SerializeField] private float m_WallMinDistanceFromGround = 0.4f;
    [SerializeField] private float m_WallJumpScalar = 3f;
    [SerializeField] private float m_WallRunCooldown = 0.1f;
    [SerializeField] private LayerMask m_WallLayers;
    [SerializeField] private float m_FrontWallDetectionDistance = 2.2f;

    [Header("Custom Movement Modes")]
    [SerializeField] private SlidingMovement slidingMovement;

    [Header("Drone")]
    [SerializeField] private LayerMask m_droneLayerMask;
    [SerializeField] private float m_droneJumpBoostScalar = 10f;

    [Header("Audio")]
    [SerializeField] private AudioRecord m_closeBodyFallAudio;
    [SerializeField] private AudioRecord m_distantBodyFallAudio;
    [SerializeField] private float m_minLandingVelocityForDistantBodyFall = 10f;
    [SerializeField] private float m_minLandingVelocityForAnyBodyFall = 5f;

    private float m_BaseMaxAcceleration;
    private bool m_WasJumpTriggered;
    private bool m_WasWallRunTriggeredPastUpdate;
    private bool m_IsWallJumpTriggered;
    private Vector3 m_WallNormal;
    private float m_WallRunUpdateTime;
    private bool m_PastUpdateJumpState;
    private MovementMode m_PrevMovementMode;
    private int m_PrevCustomMovementMode;

    private bool CanSlide => IsWalking();

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
        Vector2 movementInput = m_InputHandler.InputMovementContext.MovementInput;
        Vector3 movementDirection = new(movementInput.x, 0, movementInput.y);
        movementDirection = m_CameraController.RelativeToCamera(movementDirection);

        if(!IsWallRunning())
            SetMovementDirection(movementDirection);

        Vector2 mouseInput = m_InputHandler.InputMouseContext.MouseDeltaInput;
        mouseInput *= mouseSensitivity;
        m_CameraController.AddCameraInput(-mouseInput.y);

        if(!IsWallRunning())
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

        if(isJumpPressed && IsWallRunning() && !m_PastUpdateJumpState)
        {
            Jump();
        }

        bool isSlidePressed = m_InputHandler.InputMovementContext.IsSlidePressedInput;
        if(isSlidePressed && CanSlide && !IsSliding())
        {
            SetMovementMode(MovementMode.Custom, (int)CustomMovementModes.Sliding);
        }
        else if(!isSlidePressed && IsSliding())
        {
            SetMovementMode(MovementMode.Walking);
        }

        m_PastUpdateJumpState = isJumpPressed;
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

            CheckCollisionWithBlocks(characterMovement.currentGround.collider);
        }

        if(IsFalling() || IsJumping())
        {
            if(DetectWall(out CollisionResult collisionResult) && CanWallRun(collisionResult.surfaceNormal))
            {
                SetMovementMode(MovementMode.Custom, (int)CustomMovementModes.WallRun);
            }
        }

        if(IsWallRunning())
        {
            if(Vector3.Dot(m_WallNormal.normalized, GetVelocity().normalized) != 0.0)
                return;

            if(characterMovement.Raycast(GetPosition(), GetVelocity().normalized, m_FrontWallDetectionDistance, m_WallLayers, out RaycastHit _))
            {
                SetMovementMode(MovementMode.Falling);
            }
        }
        else
        {
            m_WallRunUpdateTime -= deltaTime;
        }
    }

    protected override void OnCharacterMovementUpdated(float deltaTime)
    {
        base.OnCharacterMovementUpdated(deltaTime);

        TryBoostPlayerOnDrone();
    }

    protected override void OnCollided(ref CollisionResult collisionResult)
    {
        base.OnCollided(ref collisionResult);

        CheckCollisionWithBlocks(collisionResult.collider);
    }

    protected override void OnMovementModeChanged(MovementMode prevMovementMode, int prevCustomMode)
    {
        m_PrevMovementMode = prevMovementMode;
        m_PrevCustomMovementMode = prevCustomMode;

        base.OnMovementModeChanged(prevMovementMode, prevCustomMode);

        Debug.Log($"Prev Movement: {prevMovementMode.ToString()}");

        if(prevMovementMode == MovementMode.Custom)
        {
            switch((CustomMovementModes)prevCustomMode)
            {
                case CustomMovementModes.WallRun:
                    PostWallRun();
                    break;
            }

            return;
        }

        if(GetMovementMode() == MovementMode.Custom)
        {
            switch((CustomMovementModes)GetCustomMovementMode())
            {
                case CustomMovementModes.WallRun:
                    PreWallRun();
                    break;
            }
        }
    }

    protected override bool IsJumpAllowed()
    {
        if(!canJumpWhileCrouching && IsCrouched())
            return false;

        return canEverJump && (IsWalking() || IsFalling() || IsWallRunning() || IsSliding());
    }

    protected override bool DoJump()
    {
        // World up, determined by gravity direction

        Vector3 worldUp = -GetGravityDirection();

        // Don't jump if we can't move up/down.

        if(characterMovement.isConstrainedToPlane &&
            Mathf.Approximately(Vector3.Dot(characterMovement.GetPlaneConstraintNormal(), worldUp), 1.0f) && !IsWallRunning())
        {
            return false;
        }

        // Apply jump impulse along world up defined by gravity direction

        float verticalSpeed = Mathf.Max(Vector3.Dot(characterMovement.velocity, worldUp), jumpImpulse);

        characterMovement.velocity =
            Vector3.ProjectOnPlane(characterMovement.velocity, worldUp) + worldUp * verticalSpeed;

        return true;
    }

    protected override void OnLanded(Vector3 landingVelocity)
    {
        base.OnLanded(landingVelocity);

        if(m_PrevMovementMode == MovementMode.Custom && m_PrevCustomMovementMode == (int)CustomMovementModes.Sliding)
            return;

        if(Mathf.Abs(landingVelocity.y) > m_minLandingVelocityForAnyBodyFall)
        {
            AudioRecord bodyFallAudio = Mathf.Abs(landingVelocity.y) > m_minLandingVelocityForDistantBodyFall ? m_distantBodyFallAudio : m_closeBodyFallAudio;
            SoundManager.Instance.Play(bodyFallAudio, transform.position);
        }
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

    public void CheckCollisionWithBlocks(Collider collider)
    {
        if(collider.CompareTag(Tags.BasicBlock.ToString()) && collider.transform.parent.TryGetComponent(out PlayerCollidedEvents playerCollided))
        {
            playerCollided.OnPlayerCollided();
        }
    }

    private void TryBoostPlayerOnDrone()
    {
        if(!IsFalling())
            return;

        QueryTriggerInteraction cachedTriggerInteraction = characterMovement.triggerInteraction;
        characterMovement.triggerInteraction = QueryTriggerInteraction.Collide;
        bool droneDetected = characterMovement.Raycast(characterMovement.GetFootPosition(),
            Vector3.down, 3f, m_droneLayerMask, out RaycastHit raycastHit);
        characterMovement.triggerInteraction = cachedTriggerInteraction;

        if(!droneDetected || !raycastHit.collider)
            return;

        if(!raycastHit.collider.TryGetComponent(out DroneController droneController))
            return;

        LaunchCharacter(Vector3.up * m_droneJumpBoostScalar);
        droneController.KillDrone();
    }

    private bool CanWallRun(Vector3 wallNormal)
    {
        bool foundGround = characterMovement.MovementSweepTest(GetPosition(), -GetUpVector(),
            m_WallMinDistanceFromGround, out CollisionResult _);

        float angle = Vector3.Angle(wallNormal, GetForwardVector());
        return (angle >= m_WallRunMinAngle && angle <= m_WallRunMaxAngle)
               && !foundGround
               && GetMovementDirection() != Vector3.zero && !IsWallRunning()
               && m_WallRunUpdateTime <= 0.0f;
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

    private void PreWallRun()
    {
        characterMovement.SetPlaneConstraint(PlaneConstraint.ConstrainYAxis, default);
        m_WallRunUpdateTime = m_WallRunCooldown;
    }

    private void PostWallRun()
    {
        characterMovement.SetPlaneConstraint(PlaneConstraint.None, default);

        if(GetMovementMode() == MovementMode.Falling)
        {
            LaunchCharacter(m_WallNormal * m_WallJumpScalar);
            SetVelocity(GetVelocity());
        }
    }

    private void WallRun(float deltaTime)
    {
        if(!DetectWall(out CollisionResult result))
        {
            SetMovementMode(MovementMode.Falling);
        }

        Vector3 wallForward = Vector3.Cross(result.surfaceNormal, Vector3.up);

        if(result.collider && result.collider.transform.parent.TryGetComponent(out BlockDisappearing blockDisappearing))
            blockDisappearing.TryToStartDisappearingOnPlayerTouch(0.5f);

        if((GetForwardVector() - wallForward).magnitude > (GetForwardVector() - (wallForward * -1)).magnitude)
        {
            wallForward *= -1;
        }

        m_WallNormal = result.surfaceNormal;

        SetVelocity(GetSpeed() * wallForward);
        RotateTowards(GetVelocity(), deltaTime);
    }

    private bool IsWallRunning()
    {
        return (CustomMovementModes)GetCustomMovementMode() == CustomMovementModes.WallRun;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(GetPosition(), GetPosition() + GetVelocity());

        Gizmos.color = Color.red;
        Gizmos.DrawLine(GetPosition(), GetPosition() + GetMovementDirection() * 4);
    }
}

