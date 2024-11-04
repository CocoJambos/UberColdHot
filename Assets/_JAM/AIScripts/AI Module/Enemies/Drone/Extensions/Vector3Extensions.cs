using UnityEngine;

namespace JAM.AIModule.Drone.ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static Vector3 FlattenVector(this Vector3 vector)
        {
            vector.y = 0f;
            return vector;
        }
    }
}