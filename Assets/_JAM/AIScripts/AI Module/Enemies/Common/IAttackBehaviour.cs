using System;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

namespace JAM.AIModule
{
    public interface IAttackBehaviour
    {
        void UpdateBehaviour();
        void AttackTarget();
        void StopAttackTarget();
        Vector3 GetAttackPosition();
    }

    public interface IPursuer 
    {
        void CheckChasingStatus();
        float CalculateFlatDistanceToTarget();
    }

    public interface ISeekAndLoseChaser : IPursuer
    {
        public Action OnTargetChasedEvent { get; set; }
        public Action OnTargetLostEvent{ get; set; }
    }
    
    public interface ISeekChaser  : IPursuer
    {
        public Action OnTargetChasedEvent { get; set; }
    }
}
