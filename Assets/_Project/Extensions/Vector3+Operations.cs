using UnityEngine;

namespace ENA
{
    public static partial class Vector3Extensions
    {
        public static float GetAngleOnAxis(this Vector3 self, Vector3 other, Vector3 upPlane)
        {
            var axis = Vector3.up;
            Vector3 perpendicularSelf = Vector3.Cross(axis, self);
            Vector3 perpendicularOther = Vector3.Cross(axis, other);
            return Vector3.SignedAngle(perpendicularSelf, perpendicularOther, axis);
        }

        public static Vector3 Rotate(this Vector3 vector, Vector3 axis, float degrees)
        {
            return Quaternion.Euler(axis.x*degrees, axis.y*degrees, axis.z*degrees)*vector;
        }
    }
}