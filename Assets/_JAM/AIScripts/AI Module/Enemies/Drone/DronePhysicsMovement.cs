using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class DronePhysicsMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private ObstacleAvoider _obstacleAvoider;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector2 _startFinishDragValuePair;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _bodyObject;

        private IAttackBehaviour _attackBehaviour;
        private TimeManager _timeManager;
        private Transform _playerTarget;

        private void Start()
        {
            _playerTarget = PlayerTransform.Get();
            _timeManager = TimeManager.Instance;
        }

        public void InjectAttackBehaviour(IAttackBehaviour attackBehaviour)
        {
            _attackBehaviour = attackBehaviour;
        }

        public void StartMovement()
        {
            _rigidbody.linearDamping = _startFinishDragValuePair.x;
        }

        public void StopMovement()
        {
            _rigidbody.linearDamping = _startFinishDragValuePair.y;
        }

        public void UpdateMovement(float deltaTime)
        {
            var moveVector = CalculateMovementVector();
            if(_obstacleAvoider.IsObstacleInPath(moveVector))
            {
                moveVector = _obstacleAvoider.GetAvoidanceDirection(moveVector);
            }
            Debug.DrawRay(_bodyObject.position, moveVector * 5f, Color.red, Time.deltaTime);
            MovementRoutine(moveVector);
            RotationRoutine(deltaTime);
        }

        private void MovementRoutine(Vector3 movementVector)
        {
            Vector3 currentRealVelocity = _rigidbody.linearVelocity / TimeManager.Instance.TimeScale;
            _rigidbody.linearVelocity = currentRealVelocity;
            _rigidbody.AddForce(movementVector * _speed * TimeManager.Instance.TimeScale, ForceMode.VelocityChange);
            _rigidbody.linearVelocity *= TimeManager.Instance.TimeScale;
        }

        private void RotationRoutine(float deltaTime)
        {
            Vector3 directionToTarget = (_playerTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion smoothedRotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                deltaTime * _rotationSpeed * _timeManager.TimeScale
            );
            _rigidbody.MoveRotation(smoothedRotation);
        }

        private Vector3 CalculateMovementVector()
        {
            Vector3 targetPos = _attackBehaviour.GetAttackPosition();
            return (targetPos - _bodyObject.position).normalized;
        }
    }
}
