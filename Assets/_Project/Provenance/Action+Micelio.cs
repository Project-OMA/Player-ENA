using MicelioUnity;
using UnityEngine;

namespace ENA.Provenance
{
    public partial class Action
    {
        #region Static Methods
        public Activity GenerateActionActivity(Objective objective)
        {
            var activity = this.ToActivity();
            activity.AddProperty("direction", direction);
            activity.AddEntity(objective, "objective");
            return activity;
        }
        #endregion
    }
}