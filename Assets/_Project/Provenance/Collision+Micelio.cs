using MicelioUnity;
using UnityEngine;

namespace ENA.Provenance
{
    public partial class Collision
    {
        #region Static Methods
        public Activity GenerateActivity(Objective objective)
        {
            var activity = this.ToActivity();
            activity.AddEntity(objective, "objective");
            return activity;
        }
        #endregion
    }
}