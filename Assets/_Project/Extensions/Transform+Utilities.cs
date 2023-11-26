using ENA.Maps;
using UnityEngine;

namespace ENA
{
    public static partial class TransformExtensions
    {
        public static bool ExtractProp(this Transform self, out Prop prop)
        {
            prop = self.TryGetComponent(out CollidableProp collidableProp) ? collidableProp.Prop : null;
            return prop != null;
        }
        public static string ExtractPropID(this Transform self, string defaultValue = "No ID")
        {
            return self.ExtractProp(out Prop prop) ? prop.ID : defaultValue;
        }

        public static void RotateTowards(this Transform self, Quaternion target, float maxDegreesDelta)
        {
            self.rotation = Quaternion.RotateTowards(self.rotation, target, maxDegreesDelta);
        }
    }
}