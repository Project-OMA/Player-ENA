using ENA.Maps;
using UnityEngine;

namespace ENA
{
    public static partial class TransformExtensions
    {
        public static string ExtractPropID(this Transform self, string defaultValue = "No ID")
        {
            return self.GetComponent<CollidableProp>()?.Prop.Name ?? defaultValue;
        }

        public static void RotateTowards(this Transform self, Quaternion target, float maxDegreesDelta)
        {
            self.rotation = Quaternion.RotateTowards(self.rotation, target, maxDegreesDelta);
        }
    }
}