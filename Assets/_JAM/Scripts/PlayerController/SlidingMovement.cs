using DG.Tweening;
using ECM2;
using UnityEngine;
using static ECM2.Character;

public class SlidingMovement : MonoBehaviour
{
    [SerializeField]
    private FirstPersonCharacterController character;
    [SerializeField]
    private float playerHeightWhileSliding = 0.5f;
    [SerializeField]
    private float slidingFriction = 0.3f;
    [SerializeField]
    private float slidingStartBoost = 10f;
    //[SerializeField]
    //private float slidingStartBoostDelay = 1f;
    //[SerializeField]
    //private float maxSlidingSideSpeed = 10f;
    //[SerializeField]
    //private float slidingSideSpeedMultiplier = 0.1f;

    [Header("Camera")]
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private TweenController moveCameraToSlidingPositionAnim;
    [SerializeField]
    private TweenController moveCameraToStandingPositionAnim;

    private CharacterMovement Movement => character.GetCharacterMovement();

    private Tween cameraMoveTween;

    protected void OnEnable()
    {
        character.MovementModeChanged += OnMovementModeChanged;
        character.BeforeSimulationUpdated += OnBeforeSimulationUpdated;
        character.CustomMovementModeUpdated += OnCustomMovementModeUpdated;
    }

    protected void OnDisable()
    {
        character.MovementModeChanged -= OnMovementModeChanged;
        character.BeforeSimulationUpdated -= OnBeforeSimulationUpdated;
        character.CustomMovementModeUpdated -= OnCustomMovementModeUpdated;
    }

    private void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMode)
    {
        if(character.IsSliding())
        {
            OnSlidingStarted();
        }
        else if(prevMovementMode == Character.MovementMode.Custom && prevCustomMode == (int)CustomMovementModes.Sliding)
        {
            OnSlidingStopped();
        }
    }

    private void OnBeforeSimulationUpdated(float deltaTime)
    {
        if(character.IsSliding())
        {
            if(character.IsGrounded())
            {
                character.CheckCollisionWithBlocks(Movement.currentGround.collider);
            }
            else
            {
                character.SetMovementMode(MovementMode.Falling);
            }
        }
    }

    private void OnCustomMovementModeUpdated(float deltaTime)
    {
        if(character.IsSliding())
        {
            SlidingMovementMode(deltaTime);
        }
    }

    private void SlidingMovementMode(float deltaTime)
    {
        if(character.useRootMotion && character.rootMotionController)
        {
            Movement.velocity = character.GetDesiredVelocity();
        }
        else
        {
            //Vector3 desiredSideVelocity = ExtractSideVelocity(character.GetDesiredVelocity());
            Movement.velocity = character.CalcVelocity(Movement.velocity, Vector3.zero, slidingFriction, false, deltaTime);
        }

        if(character.applyStandingDownwardForce)
        {
            character.ApplyDownwardsForce();
        }
    }

    public void OnSlidingStarted()
    {
        Movement.SetHeight(playerHeightWhileSliding);
        Movement.AddForce(cameraController.transform.forward * slidingStartBoost, ForceMode.Impulse);

        cameraMoveTween?.Kill();
        cameraMoveTween = moveCameraToSlidingPositionAnim.PlayTween();
    }

    public void OnSlidingStopped()
    {
        Movement.SetHeight(character.unCrouchedHeight);
        Movement.AddForce(-cameraController.transform.forward * slidingStartBoost, ForceMode.Impulse); // so the boost is not abused

        cameraMoveTween?.Kill();
        cameraMoveTween = moveCameraToStandingPositionAnim.PlayTween();
    }

    //private Vector3 ExtractSideVelocity(Vector3 velocity) => cameraController.RelativeToCamera(new Vector3(cameraController.RelativeToCamera(velocity).x, 0f, 0f));

    //private Vector3 ClampSideVelocity(Vector3 velocity)
    //{
    //    Vector3 cameraRelativeVelocity = cameraController.RelativeToCamera(velocity);
    //    float localMaxSideVelocity = Mathf.Abs(cameraRelativeVelocity.z) * slidingSideSpeedMultiplier;
    //    float currentMaxSideVelocity = Mathf.Min(localMaxSideVelocity, maxSlidingSideSpeed);
    //    Vector3 clampedCameraRelativeSideVelocity = new Vector3(Mathf.Clamp(cameraRelativeVelocity.x, -currentMaxSideVelocity, currentMaxSideVelocity), 0f, cameraRelativeVelocity.z);

    //    return cameraController.RelativeToCamera(clampedCameraRelativeSideVelocity);
    //}

    //private Vector3 ClampDesiredVelocity(Vector3 desiredVelocity, Vector3 currentVelocity)
    //{
    //    float cameraRelativeDesiredSideVelocity = cameraController.RelativeToCamera(desiredVelocity).x;
    //    cameraRelativeDesiredSideVelocity *= slidingSideSpeedMultiplier * Mathf.Abs(currentVelocity.z);

    //    Vector3 clampedCameraRelativeDesiredVelocity = new Vector3(Mathf.Clamp(cameraRelativeDesiredSideVelocity, -maxSlidingSideSpeed, maxSlidingSideSpeed), 0f, 0f);

    //    return cameraController.RelativeToCamera(clampedCameraRelativeDesiredVelocity);
    //}
}
