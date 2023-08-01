using MicelioUnity;
using UnityEngine;

namespace ENA.Provenance
{
    public interface IActivity
    {
        string Name {get;}
        Vector3 Position {get;}
        float Timestamp {get;}
    }

    public static class IActivityExtensions
    {
        public static Activity ToActivity(this IActivity self)
        {
            Activity activity = new(self.Name, self.Timestamp.ToString());
            activity.SetPosition(self.Position);
            return activity;
        }
    }
}