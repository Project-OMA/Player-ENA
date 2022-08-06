using System;
using ENA.Services;
using UnityEngine;

namespace ENA.Persistency
{
    [Serializable]
    [CreateAssetMenu(fileName="New Profile", menuName="ENA/Settings Profile")]
    public class SettingsProfile: ScriptableObject
    {
        #region Variables
        public ENAProfile LoggedProfile {get; set;}
        [Header("Gameplay Settings")]
        public bool GyroEnabled;
        public bool MinimapEnabled;
        public bool VREnabled;
        public bool VibrationEnabled;
        public bool ElementsDisappearEnabled;
        #endregion
        #region Methods
        private void GetBool(string key, out bool value)
        {
            int rawValue = PlayerPrefs.GetInt(key);
            value = Convert.ToBoolean(rawValue);
        }

        public void Load()
        {
            GetBool(nameof(GyroEnabled), out GyroEnabled);
            GetBool(nameof(MinimapEnabled), out MinimapEnabled);
            GetBool(nameof(VREnabled), out VREnabled);
            GetBool(nameof(VibrationEnabled), out VibrationEnabled);
            GetBool(nameof(ElementsDisappearEnabled), out ElementsDisappearEnabled);
        }

        public void Reset()
        {
            ResetKey(nameof(GyroEnabled), out GyroEnabled);
            ResetKey(nameof(MinimapEnabled), out MinimapEnabled);
            ResetKey(nameof(VREnabled), out VREnabled);
            ResetKey(nameof(VibrationEnabled), out VibrationEnabled);
            ResetKey(nameof(ElementsDisappearEnabled), out ElementsDisappearEnabled);
        }

        private void ResetKey(string key, out bool value)
        {
            PlayerPrefs.DeleteKey(key);
            value = false;
        }

        public void Save()
        {
            SetBool(nameof(GyroEnabled), in GyroEnabled);
            SetBool(nameof(MinimapEnabled), in MinimapEnabled);
            SetBool(nameof(VREnabled), in VREnabled);
            SetBool(nameof(VibrationEnabled), in VibrationEnabled);
            SetBool(nameof(ElementsDisappearEnabled), in ElementsDisappearEnabled);
        }

        public void SetAcessibility(bool value)
        {
            UAP_AccessibilityManager.EnableAccessibility(value);
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