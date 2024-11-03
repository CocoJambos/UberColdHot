using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class DroneMovement : MonoBehaviour
    {
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private float _speed;

        [SerializeField] private Transform _bodyObject;

        public float timeScale = 1f;


        private void Update()
        {
            var step =  _speed * Time.deltaTime * timeScale; 
            _bodyObject.position = Vector3.MoveTowards(_bodyObject.position, _playerTarget.position, step);
        }
    }
}
