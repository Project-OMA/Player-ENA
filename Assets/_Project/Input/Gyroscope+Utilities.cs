using UnityEngine;

namespace ENA.Utilities
{
    public static partial class GyroscopeExtensions
    {
        public static Quaternion AttitudeToUnity(this Gyroscope self)
        {
            var quaternionGyro = self.attitude;
            return new Quaternion(quaternionGyro.x, quaternionGyro.y, -quaternionGyro.z, -quaternionGyro.w);
        }
    }
}