using System.Linq;
using ENA.Services;
using UnityEngine;

namespace ENA.Goals
{
    public partial class ObjectiveList
    {
        #region Settings
        public void HandleObjectState(SettingsProfile profile) {
            if (!profile.ElementsDisappearEnabled) return;

            cleared.LastOrDefault()?.gameObject.SetActive(false);
        }
        #endregion
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