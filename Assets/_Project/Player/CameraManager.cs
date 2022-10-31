using ENA.Input;
using ENA.Persistency;
using UnityEngine;

namespace ENA.Player
{
    public class CameraManager: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] PlayerController player;
        [Header("Camera Handlers")]
        [SerializeField] ControleGiroscopio defaultCameraGyro;
        [SerializeField] ControleGiroscopio vrCameraGyro;
        #endregion
        #region Methods
        public void SetCameraGyro(bool value)
        {
            vrCameraGyro.enabled = value;
            defaultCameraGyro.enabled = value;

            if (value) {
                Transform.localEulerAngles = new Vector3(90, 0, 0);
                Transform.SetParent(player.Transform.parent);
            } else {
                Transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        public void SetVR(bool value)
        {
            if (value) {
                player.SetCameraTransform(vrCameraGyro.transform);
            } else {
                player.SetCameraTransform(defaultCameraGyro.transform);
            }

            defaultCameraGyro.gameObject.SetActive(!value);
            vrCameraGyro.gameObject.SetActive(value);
        }
        #endregion
    }
}