using UnityEngine;
using SaveContainer = ENA.Input.ControleMenuPrincipal;

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
        [SerializeField] TaskSettingsDisplay settingsPanel;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            
        }
        #endregion
        #region Methods
        public void CancelChanges()
        {
            gyroEnabled = SaveContainer.giroscopioValue;
            minimapEnabled = SaveContainer.mapaValue;
            vrEnabled = SaveContainer.oculosValue;
            elementsDisappearEnabled = SaveContainer.elementosValue;
            vibrationEnabled = SaveContainer.vibrationValue;
        }

        public void CloseSettingsPanel()
        {
            manager.Pop(settingsPanel);
        }

        public void OpenSettingsPanel()
        {
            manager.Push(settingsPanel);
        }

        public void ResetToDefault()
        {
            gyroEnabled = false;
            minimapEnabled = false;
            vrEnabled = false;
            vibrationEnabled = false;
            elementsDisappearEnabled = false;
        }

        public void SaveSettings()
        {
            SaveContainer.giroscopioValue = gyroEnabled;
            SaveContainer.mapaValue = minimapEnabled;
            SaveContainer.oculosValue = vrEnabled;
            SaveContainer.elementosValue = elementsDisappearEnabled;
            SaveContainer.vibrationValue = vibrationEnabled;
        }
        
        public void SetGyro(bool value)
        {
            gyroEnabled = value;
        }

        public void SeMinimap(bool value)
        {
            minimapEnabled = value;
        }

        public void SetVR(bool value)
        {
            vrEnabled = value;
        }

        public void SetElementsDisappear(bool value)
        {
            elementsDisappearEnabled = value;
        }

        public void SetVibration(bool value)
        {
            vibrationEnabled = value;
        }
        #endregion
    }
}