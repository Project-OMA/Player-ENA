using System;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Physics
{
    public class RotationTracker: MonoBehaviour
    {
        #region Variables
        int currentAngle = 0;
        [SerializeField] float moveSpeed;
        float rotationTimer = 0;
        bool selectedDirection = false;
        Quaternion targetRotation = Quaternion.identity;
        Direction.Cardinal cardinalDirection;
        Direction.Cardinal lastCardinalDirection;
        #endregion
        public bool IsRotating => !IsLookingAtSameDirection();
        #region Events
        public Event<bool> OnTurn;
        #endregion
        #region MonoBehaviour Lifecycle
        private void Start()
        {
            currentAngle = (int)transform.localEulerAngles.y;
            lastCardinalDirection = cardinalDirection;
            targetRotation = transform.rotation;
        }

        private void Update()
        {
            rotationTimer += Time.deltaTime;
            cardinalDirection = Direction.CardinalFor(currentAngle);
        }
        #endregion
        #region Methods
        public bool IsLookingAtSameDirection()
        {
            return Mathf.Abs(transform.eulerAngles.y - targetRotation.eulerAngles.y) < 0.001f;
        }

        public void Rotate(float axisValue)
        {
            if (IsLookingAtSameDirection() && axisValue != 0) {
                rotationTimer = 0;
                if (!selectedDirection) {
                    bool right = axisValue > 0;
                    if (right) {
                        currentAngle += 90;
                        targetRotation.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
                    } else {
                        currentAngle -= 90;
                        targetRotation.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, currentAngle, transform.localRotation.eulerAngles.z);
                    }
                    selectedDirection = true;
                    OnTurn.Invoke(right);
                }
            } else {
                selectedDirection = false;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationTimer * moveSpeed * Time.deltaTime * 2);
        }

        public void SetTrackedAngle(float angle)
        {
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, angle, targetRotation.eulerAngles.z);
        }
        #endregion
    }
}