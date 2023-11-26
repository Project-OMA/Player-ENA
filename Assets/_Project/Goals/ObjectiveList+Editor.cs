using UnityEngine;

namespace ENA.Goals
{
    public partial class ObjectiveList
    {
        #region Editor Methods
        [ContextMenu("Leave Only Next Objective")]
        void LeaveOnlyNextObjective()
        {
            var nextObjective = NextObjective;
            current.Clear();
            current.Add(nextObjective);
        }
        #endregion
    }
}