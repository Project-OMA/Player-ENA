using MicelioUnity;
using UnityEngine;

namespace ENA
{
    public static partial class ActivityExtensions
    {
        public static void SetPosition(this Activity activity, Vector3 position)
        {
            activity.SetPosition(position.x, position.y, position.z);
        }

        public static void SendTo(this Activity activity, Micelio destination)
        {
            destination.SendActivity(activity);
        }
    }
}