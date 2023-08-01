using System;
using UnityEngine;

namespace ENA.Services
{
    [Serializable]
    [CreateAssetMenu(fileName="New Profile", menuName="ENA/Settings Profile")]
    public class SettingsProfile: ScriptableObject
    {
        #region Variables
        public ENAProfile LoggedProfile {get; set;}
        [Header("Gameplay Settings")]
        public bool ElementsDisappearEnabled;
        public bool GyroEnabled;
        public bool MinimapEnabled;
        public bool VREnabled;
        public bool VibrationEnabled;
        #endregion
        #region Methods
        private void GetBool(string key, out bool value)
        {
            int rawValue = PlayerPrefs.GetInt(key);
            value = Convert.ToBoolean(rawValue);
        }

        public void Load()
        {
            GetBool(nameof(ElementsDisappearEnabled), out ElementsDisappearEnabled);
            GetBool(nameof(GyroEnabled), out GyroEnabled);
            GetBool(nameof(MinimapEnabled), out MinimapEnabled);
            GetBool(nameof(VREnabled), out VREnabled);
            GetBool(nameof(VibrationEnabled), out VibrationEnabled);
        }

        public void Reset()
        {
            ResetKey(nameof(ElementsDisappearEnabled), out ElementsDisappearEnabled);
            ResetKey(nameof(GyroEnabled), out GyroEnabled);
            ResetKey(nameof(MinimapEnabled), out MinimapEnabled);
            ResetKey(nameof(VREnabled), out VREnabled);
            ResetKey(nameof(VibrationEnabled), out VibrationEnabled);
        }

        private void ResetKey(string key, out bool value)
        {
            PlayerPrefs.DeleteKey(key);
            value = false;
        }

        public void Save()
        {
            SetBool(nameof(ElementsDisappearEnabled), in ElementsDisappearEnabled);
            SetBool(nameof(GyroEnabled), in GyroEnabled);
            SetBool(nameof(MinimapEnabled), in MinimapEnabled);
            SetBool(nameof(VREnabled), in VREnabled);
            SetBool(nameof(VibrationEnabled), in VibrationEnabled);
        }

        public void SetAcessibility(bool value)
        {
            // UAP_AccessibilityManager.EnableAccessibility(value);
        }

        private void SetBool(string key, in bool value)
        {
            int rawValue = Convert.ToInt32(value);
            PlayerPrefs.SetInt(key, rawValue);
        }

        public void SetElementsDisappear(bool value)
        {
            ElementsDisappearEnabled = value;
        }

        public void SetGyro(bool value)
        {
            GyroEnabled = value;
        }

        public void SetMinimap(bool value)
        {
            MinimapEnabled = value;
        }

        public void SetVibration(bool value)
        {
            VibrationEnabled = value;
        }

        public void SetVR(bool value)
        {
            VREnabled = value;
        }
        #endregion
    }
}