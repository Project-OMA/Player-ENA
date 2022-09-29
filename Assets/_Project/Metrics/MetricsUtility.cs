using UnityEngine;

namespace ENA.Metrics
{
    public static partial class MetricsUtility
    {
        public static Activity GenerateCollisionActivity(SessionModel.Collision collision, SessionModel.Objective objective)
        {
            var activity = new Activity("collision", collision.Timestamp.ToString());
            activity.SetPosition(collision.Position);
            activity.AddEntity(objective, nameof(objective));
            return activity;
        }

        public static Activity GenerateCompletedObjectiveActivity(SessionModel.Objective objective)
        {
            throw new System.NotImplementedException();
        }

        public static Activity GenerateActionActivity(SessionModel.Action action, SessionModel.Objective objective)
        {
            var activity = new Activity(action.ActionType.ToString(), action.Timestamp.ToString());
            activity.SetPosition(action.Position);
            activity.AddEntity(objective, nameof(objective));
            return activity;
        }
    }
}