using UnityEngine;

namespace ENA
{
    public static partial class Vector3Extensions
    {
        public static float AngleForDirection(this Vector3 self)
        {
            return Mathf.Atan2(self.z, self.x) * Mathf.Rad2Deg;
        }

        public static float AngleForDirection(this Vector3 self, Vector3 target)
        {
            var delta = target - self;
            return delta.AngleForDirection();
        }

        public static Vector3 Rotate(this Vector3 vector, Vector3 axis, float degrees)
        {
            return Quaternion.Euler(axis.x*degrees, axis.y*degrees, axis.z*degrees)*vector;
        }
    }
}