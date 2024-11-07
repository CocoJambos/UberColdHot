using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class DroneDestroyer : MonoBehaviour
    {
        [SerializeField] private GameObject _droneObject;
        public void DestroyDrone()
        {
            Debug.Log("DestroyDrone");
            Destroy(_droneObject);
        }
    }
}