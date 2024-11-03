using JAM.AIModule.Drone;
using UnityEngine;

namespace JAM.AIModule
{
    [RequireComponent(typeof(AttackTargetFinder))]
    public class AttackBehaviour : MonoBehaviour
    {
        public void AttackPlayer()
        {
            Debug.Log("Paw! Attack Player");
        }
    }
}