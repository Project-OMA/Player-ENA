using System;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Physics
{
    public class RotationTracker: ExtendedMonoBehaviour
    {
        #region Variables
        int currentAngle = 0;
        [SerializeField] float moveSpeed;
        float rotationTimer = 0;
        bool selectedDirection = false;
        Quaternion targetRotation = Quaternion.identity;
        #endregion
        #region Properties
        public bool IsRotating => !IsLookingAtSameDirection();
        #endregion
        #region Events
        public Event<bool> OnTurn;
        #endregion
        #region MonoBehaviour Lifecycle
        void FixedUpdate()
        {
            rotationTimer += Time.deltaTime;
            #if ENABLE_LOG
            Debug.Log($"{rotationTimer}s: {IsRotating}");
            #endif
        }

        void Start()
        {
            currentAngle = (int)Transform.localEulerAngles.y;
            targetRotation = Transform.rotation;
        }
        #endregion
        #region Methods
        public bool IsLookingAtSameDirection()
        {
            return rotationTimer >= 1;
        }

        public void GyroRotate(float gyroAngle)
        {
            var playerAngle = Transform.eulerAngles.y;
            var directionGyro = Direction.CardinalFor(gyroAngle-90);
            var directionPlayer = Direction.CardinalFor(playerAngle-90);
            #if ENABLE_LOG
            Debug.Log($"Gyro: {directionGyro} | Player: {directionPlayer}");
            #endif

            if (directionGyro != directionPlayer) {
                targetRotation = Quaternion.Euler(0, directionGyro.ToAngle(), 0);
                Transform.rotation = targetRotation;
                #if ENABLE_LOG
                Debug.Log($"Changed direction to {directionGyro.ToAngle()}!");
                #endif
            }
        }

        public void Rotate(float axisValue)
        {
            if (IsLookingAtSameDirection() && axisValue != 0) {
                rotationTimer = 0;
                if (!selectedDirection) {
                    bool right = axisValue > 0;
                    if (right) {
                        RotateRight();
                    } else {
                        RotateLeft();
                    }
                    selectedDirection = true;
                    OnTurn.Invoke(right);
                }
            } else {
                selectedDirection = false;
            }

            var t = rotationTimer * moveSpeed * Time.deltaTime * 2;
            Rotate(targetRotation, t);
        }

        private void Rotate(Quaternion targetRotation, float t)
        {
            Transform.rotation = Quaternion.Lerp(Transform.rotation, targetRotation, t);
        }

        private void RotateLeft()
        {
            currentAngle -= 90;
            targetRotation.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, currentAngle, transform.localRotation.eulerAngles.z);
        }

        private void RotateRight()
        {
            currentAngle += 90;
            targetRotation.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
        }

        public void SetTrackedAngle(float angle)
        {
            Vector3 eulerAngles = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(eulerAngles.x, angle, eulerAngles.z);
        }
        #endregion
    }
}