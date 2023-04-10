using UnityEngine;

namespace ENA
{
    public static partial class TransformExtensions
    {
        public static void RotateTowards(this Transform self, Quaternion target, float maxDegreesDelta)
        {
            self.rotation = Quaternion.RotateTowards(self.rotation, target, maxDegreesDelta);
        }
    }
}