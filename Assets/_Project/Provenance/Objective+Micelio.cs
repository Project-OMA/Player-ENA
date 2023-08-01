using MicelioUnity;
using System.Linq;

namespace ENA.Provenance
{
    public partial class Objective
    {
        #region Entity Implementation
        public Entity GetEntity()
        {
            var entity = this.ToEntity();
            entity.AddProperty("timeSpent", endTime - startTime);
            entity.AddProperty("numberOfCollisions", collisions.Count);
            entity.AddProperty("numberOfRotations", actions.Count(x => x.ActionType == Action.Type.Turn));
            entity.AddProperty("numberOfSteps", actions.Count(x => x.ActionType == Action.Type.Walk));
            return entity;
        }
        #endregion
        #region Methods
        public Activity GenerateCompletedActivity()
        {
            var activity = new Activity("foundObjective", endTime.ToString());
            activity.AddEntity(this, "objective");
            return activity;
        }
        #endregion
    }
}