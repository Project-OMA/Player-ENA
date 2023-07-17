using ENA.Services;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    public class TaskSettingsDisplay: UIPanel
    {
        #region Variables
        [SerializeField] SettingsProfile profile;
        [Header("References")]
        [SerializeField] Toggle elementsDisappearToggle;
        [SerializeField] Toggle gyroToggle;
        [SerializeField] Toggle minimapToggle;
        [SerializeField] Toggle vrToggle;
        [SerializeField] Toggle vibrationToggle;
        #endregion
        #region Methods
        private void OnEnable()
        {
            elementsDisappearToggle.isOn = profile.ElementsDisappearEnabled;
            gyroToggle.isOn = profile.GyroEnabled;
            minimapToggle.isOn = profile.MinimapEnabled;
            vrToggle.isOn = profile.VREnabled;
            vibrationToggle.isOn = profile.VibrationEnabled;
        }
        #endregion
    }
}