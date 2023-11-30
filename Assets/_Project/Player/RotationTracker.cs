using UnityEngine;
using UnityEngine.InputSystem;
using ENA.Services;

namespace ENA.Player
{
    public class RotationTracker: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] RotationComponent component;
        Quaternion targetRotation = Quaternion.identity;
        [SerializeField] InputActionReference turnLeft, turnRight;
        [Header("References")]
        [SerializeField] GyroCamera gyro;
        [SerializeField] SettingsProfile profile;
        #endregion
        #region Events
        public Event<bool> OnTurn;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            turnLeft.action.Disable();
            turnRight.action.Disable();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            turnLeft.action.Enable();
            turnRight.action.Enable();
        }
        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            component.RotateTowards(targetRotation);
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            targetRotation = Transform.rotation;
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            CheckInput();
        }
        #endregion
        #region Methods
        private void CheckInput()
        {
            if (profile.GyroEnabled) {
                GyroRotate();
                return;
            }

            if (turnLeft.action.WasPressedThisFrame()) {
                RotateLeft();
            } else if (turnRight.action.WasPressedThisFrame()) {
                RotateRight();
            }
        }

        private void GyroRotate()
        {
            var gyroAngle = gyro.Transform.eulerAngles.y;
            var playerAngle = Transform.eulerAngles.y;
            var directionGyro = Direction.CardinalFor(gyroAngle - 90);
            var directionPlayer = Direction.CardinalFor(playerAngle - 90);
            #if ENABLE_LOG
            Debug.Log($"Gyro: {directionGyro} | Player: {directionPlayer}");
            #endif

            if (directionGyro == directionPlayer) return;

            var deltaAngle = gyroAngle - playerAngle;

            if (deltaAngle > 0) {
                RotateRight();
            } else {
                RotateLeft();
            }
        }

        public void RotateLeft()
        {
            RotateBy(-90);
            OnTurn.Invoke(false);
        }

        public void RotateRight()
        {
            RotateBy(90);
            OnTurn.Invoke(true);
        }

        public void RotateBy(float angleDegreesDelta)
        {
            Vector3 eulerAngles = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y + angleDegreesDelta, eulerAngles.z);
        }
        #endregion
    }
}