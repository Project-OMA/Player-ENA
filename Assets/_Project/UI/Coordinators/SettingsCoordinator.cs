using ENA.Services;
using UnityEngine;

namespace ENA.UI
{
    public class SettingsCoordinator: UICoordinator
    {
        #region Variables
        [SerializeField] SettingsProfile profile;
        [Header("Displays")]
        [SerializeField] TaskSettingsDisplay settingsPanel;
        #endregion
        #region UICoordinator Implementation
        public override void Setup() => profile.Load();
        #endregion
        #region Methods
        public void CancelChanges() => profile.Load();
        public void CloseSettingsPanel() => manager.Pop(settingsPanel);
        public void OpenSettingsPanel() => manager.Push(settingsPanel);
        public void ResetToDefault() => profile.Reset();
        public void SaveSettings() => profile.Save();
        #endregion
    }
}