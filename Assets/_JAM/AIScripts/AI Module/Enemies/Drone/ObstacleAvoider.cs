using System;
using UnityEngine;

namespace JAM.AIModule.Drone
{
    public class ObstacleAvoider : MonoBehaviour
    {
        public Vector3 AvoidVector;

        private bool _isObstacleInFront = false;


        public Transform _player;

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("OnCollisionEnter");
        }

        void OnCollisionStay(Collision collision)
        {
            Debug.Log("OnCollisionStay");
            if(collision.gameObject.layer == LayerMask.NameToLayer("Playground"))
            {
                Debug.Log("Playground");

                RecalculateAvoidVector(collision.contacts[0]);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Playground"))
            {
                AvoidVector = Vector3.zero;
            }
        }

        void RecalculateAvoidVector(ContactPoint contactPoint)
        {
            Debug.DrawRay(transform.position, contactPoint.point, Color.red, 0.5f);
            AvoidVector = transform.position - contactPoint.point;
        }


        void Update()
        {
            LookForObstacle();
        }
        
        private Vector3 ComposeToPlayerVector() => _player.position - transform.position;


        void LookForObstacle()
        {
            if (Physics.Raycast(transform.position, ComposeToPlayerVector(), out RaycastHit hit, 1000f))
            {
                _isObstacleInFront = !hit.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player"));
            }
        }
    }
}