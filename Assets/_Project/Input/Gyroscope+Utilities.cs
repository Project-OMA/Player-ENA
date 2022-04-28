using UnityEngine;

namespace JurassicEngine
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