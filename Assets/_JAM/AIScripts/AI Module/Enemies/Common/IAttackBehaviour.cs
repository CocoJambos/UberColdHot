using System;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

namespace JAM.AIModule
{
    public interface IAttackBehaviour
    {
        public Action OnTargetChasedEvent { get; set; }
        public Action OnTargetLostEvent{ get; set; }
        void UpdateBehaviour();
        void AttackTarget();
        void StopAttackTarget();
        Vector3 GetTargetPosition();
    }

    public interface IChaser
    {
        void CheckChaseCondition();
        float CalculateDistanceToTarget();
    }
}
