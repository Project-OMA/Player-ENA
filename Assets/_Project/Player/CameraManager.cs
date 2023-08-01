using ENA.Services;
using UnityEngine;

namespace ENA.Player
{
    public class CameraManager: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] GyroCamera gyroscope;
        [SerializeField] SettingsProfile profile;
        [Header("Camera Handlers")]
        [SerializeField] GameObject defaultCamera;
        [SerializeField] GameObject vrCamera;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Configure(profile);
        }
        #endregion
        #region Methods
        public void Configure(SettingsProfile profile)
        {
            SetVR(profile.VREnabled);
            SetCameraGyro(profile.GyroEnabled);
        }

        public void SetCameraGyro(bool value)
        {
            gyroscope.enabled = value;
        }

        public void SetVR(bool value)
        {
            defaultCamera?.SetActive(!value);
            vrCamera?.SetActive(value);
        }
        #endregion
    }
}