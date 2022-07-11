using UnityEngine;

namespace ENA.UI
{
    public class SettingsCoordinator: UICoordinator
    {
        #region Variables
        public bool gyroEnabled;
        public bool minimapEnabled;
        public bool vrEnabled;
        public bool vibrationEnabled;
        public bool elementsDisappearEnabled;
        [Header("Displays")]
        [SerializeField] UIPanel settingsPanel;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            
        }
        #endregion
        #region Methods
        public void CancelChanges()
        {
            throw new System.NotImplementedException();
        }

        public void CloseSettingsPanel()
        {
            throw new System.NotImplementedException();
        }

        public void OpenSettingsPanel()
        {
            throw new System.NotImplementedException();
        }

        public void ResetToDefault()
        {
            throw new System.NotImplementedException();
        }

        public void SaveSettings()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}