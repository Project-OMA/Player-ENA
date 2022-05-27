using UnityEngine;

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
        #region Properties
        public int NumberOfRotations {get; set;}
        #endregion
        #region Delegates
        public delegate void Event(bool turnedRight);
        #endregion
        #region Events
        public event Event onTurn;
        #endregion
        #region Methods
        private void CheckRotation()
        {
            if(lastCardinalDirection != cardinalDirection) {
                NumberOfRotations++;
            }

            lastCardinalDirection = cardinalDirection;
        }

        public void Rotate(float axisValue)
        {
            bool lookingAtSameAngle = (int)transform.eulerAngles.y == (int)targetRotation.eulerAngles.y;

            if (lookingAtSameAngle && axisValue != 0) {
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
                    onTurn?.Invoke(right);
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

        private void Start()
        {
            currentAngle = (int)transform.localEulerAngles.y;
            lastCardinalDirection = cardinalDirection;

            NumberOfRotations = -2;
        }

        private void Update()
        {
            rotationTimer += Time.deltaTime;
            cardinalDirection = Direction.CardinalFor(currentAngle);

            CheckRotation();
        }
        #endregion
    }
}