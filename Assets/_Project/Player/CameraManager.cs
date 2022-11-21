using ENA.Input;
using ENA.Persistency;
using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.Player
{
    public class CameraManager: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] PlayerController player;
        [SerializeField] GyroCamera gyroscope;
        [Header("Camera Handlers")]
        [SerializeField] GameObject defaultCamera;
        [SerializeField] GameObject vrCamera;
        #endregion
        #region Methods
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