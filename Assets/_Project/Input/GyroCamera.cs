using UnityEngine;
using System.Collections;

namespace ENA.Input
{
    public class GyroCamera: ExtendedMonoBehaviour
    {
        #region Variables
        private float _initialYAngle = 0f;
        private float _appliedGyroYAngle = 0f;
        private float _calibrationYAngle = 0f;
        private Transform _rawGyroRotation;
        private float _tempSmoothing;
        private Gyroscope gyro;
        [Header("Settings")]
        [SerializeField] private float _smoothing = 0.1f;
        #endregion
        #region MonoBehaviour Lifecycle
        private IEnumerator Start()
        {
            if(SystemInfo.supportsGyroscope) {
				gyro = UnityEngine.Input.gyro;
				gyro.enabled = true;

                Application.targetFrameRate = 60;
                _initialYAngle = Transform.eulerAngles.y;

                _rawGyroRotation = new GameObject("GyroRaw").transform;
                _rawGyroRotation.SetPositionAndRotation(Transform.position, Transform.rotation);

                // Wait until gyro is active, then calibrate to reset starting rotation.
                yield return new WaitForSeconds(1);

                StartCoroutine(CalibrateYAngle());
			} else {
                enabled = false;
            }
        }

        private void Update()
        {
            if (gyro == null) return;

            ApplyGyroRotation();
            ApplyCalibration();

            Transform.rotation = Quaternion.Slerp(Transform.rotation, _rawGyroRotation.rotation, _smoothing);
        }
        #endregion
        #region Methods
        private void ApplyGyroRotation()
        {
            _rawGyroRotation.rotation = UnityEngine.Input.gyro.attitude;
            _rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
            _rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
            _appliedGyroYAngle = _rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
        }

        private void ApplyCalibration()
        {
            _rawGyroRotation.Rotate(0f, -_calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
        }

        public void SetEnabled(bool value)
        {
            enabled = true;
            StartCoroutine(CalibrateYAngle());
        }
        #endregion
        #region Coroutines
        private IEnumerator CalibrateYAngle()
        {
            _tempSmoothing = _smoothing;
            _smoothing = 1;
            _calibrationYAngle = _appliedGyroYAngle - _initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
            yield return null;
            _smoothing = _tempSmoothing;
        }
        #endregion
    }
}