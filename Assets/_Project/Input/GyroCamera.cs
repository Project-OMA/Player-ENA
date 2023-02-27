using UnityEngine;
using System.Collections;

namespace ENA.Input
{
    public class GyroCamera: ExtendedMonoBehaviour
    {
        #region Variables
        private float initialYAngle = 0f;
        private float appliedGyroYAngle = 0f;
        private float calibrationYAngle = 0f;
        private Transform rawGyroRotation;
        private float tempSmoothing;
        private Gyroscope gyro;
        [Header("Settings")]
        [SerializeField] private float smoothing = 0.1f;
        #endregion
        #region MonoBehaviour Lifecycle
        private IEnumerator Start()
        {
            if(SystemInfo.supportsGyroscope) {
				gyro = UnityEngine.Input.gyro;
				gyro.enabled = true;

                Application.targetFrameRate = 60;
                initialYAngle = Transform.eulerAngles.y;

                rawGyroRotation = new GameObject("GyroRaw").transform;
                rawGyroRotation.SetPositionAndRotation(Transform.position, Transform.rotation);

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

            Transform.rotation = Quaternion.Slerp(Transform.rotation, rawGyroRotation.rotation, smoothing);
        }
        #endregion
        #region Methods
        private void ApplyGyroRotation()
        {
            rawGyroRotation.rotation = UnityEngine.Input.gyro.attitude;
            rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
            rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
            appliedGyroYAngle = rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
        }

        private void ApplyCalibration()
        {
            rawGyroRotation.Rotate(0f, -calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
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
            tempSmoothing = smoothing;
            smoothing = 1;
            calibrationYAngle = appliedGyroYAngle - initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
            yield return null;
            smoothing = tempSmoothing;
        }
        #endregion
    }
}